using System;
using System.IO;
using System.Text;
using static Midity.NoteKey;

namespace Midity
{
    public static class MidiSerializer
    {
        private static byte[] Concat(byte[] bytes1, byte[] bytes2)
        {
            var mergedBytes = new byte[bytes1.Length + bytes2.Length];
            Buffer.BlockCopy(bytes1, 0, mergedBytes, 0, bytes1.Length);
            Buffer.BlockCopy(bytes2, 0, mergedBytes, bytes1.Length, bytes2.Length);
            return mergedBytes;
        }

        private static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void SerializeFile(MidiFile midiFile, Stream stream)
        {
            var stringBytes = midiFile.encoding.GetBytes("MThd");
            stream.Write(stringBytes);
            // Chunk length
            WriteBEUint(6, 4);
            WriteBEUint(midiFile.format, 2);
            WriteBEUint((uint) midiFile.Tracks.Count, 2);
            WriteBEUint(midiFile.DeltaTime, 2);

            void WriteBEUint(uint value, int length)
            {
                for (var i = 0; i < length; i++)
                    stream.WriteByte((byte) ((value >> (8 * (length - i - 1))) & 0xff));
            }

            foreach (var midiTrack in midiFile.Tracks)
                SerializeTrack(midiTrack, midiFile.encoding, stream);
        }

        internal static void SerializeTrack(MidiTrack midiTrack, Encoding encoding, Stream stream)
        {
            var stringBytes = encoding.GetBytes("MTrk");
            // UnityEngine.Debug.Log(stream.Position);
            stream.Write(stringBytes);
            // UnityEngine.Debug.Log(stream.Position);
            var chunkEndDataPosition = stream.Position;
            stream.Seek(4, SeekOrigin.Current);
            // UnityEngine.Debug.Log(stream.Position);
            foreach (var mTrkEvent in midiTrack.Events) SerializeEvent(mTrkEvent, encoding, stream);

            var chunkEnd = stream.Position - chunkEndDataPosition - 4;
            stream.Seek(chunkEndDataPosition, SeekOrigin.Begin);
            for (var i = 0; i < 4; i++)
                stream.WriteByte((byte) ((chunkEnd >> (8 * (4 - i - 1))) & 0xff));
            stream.Seek(0, SeekOrigin.End);
        }

        internal static void SerializeEvent(MTrkEvent mTrkEvent, Encoding encoding, Stream stream)
        {
            switch (mTrkEvent)
            {
                case NoteEvent noteEvent:
                    WriteEvent(
                        noteEvent.Status,
                        noteEvent.NoteNumber,
                        noteEvent.Velocity);
                    break;
                case PolyphonicKeyPressureEvent polyphonicKeyPressureEvent:
                    WriteEvent(
                        polyphonicKeyPressureEvent.Status,
                        polyphonicKeyPressureEvent.noteNumber,
                        polyphonicKeyPressureEvent.pressure);
                    break;
                case ControlChangeEvent controlChangeEvent:
                    WriteEvent(
                        controlChangeEvent.Status,
                        controlChangeEvent.controlChangeNumber,
                        controlChangeEvent.data);
                    break;
                case ProgramChangeEvent programChangeEvent:
                    WriteEvent(
                        programChangeEvent.Status,
                        (byte) ((programChangeEvent.programNumber >> 8) & 0xff),
                        (byte) (programChangeEvent.programNumber & 0xff));
                    break;
                case ChannelPressureEvent channelPressureEvent:
                    WriteEvent(
                        channelPressureEvent.Status,
                        channelPressureEvent.pressure);
                    break;
                case PitchBendEvent pitchBendEvent:
                    WriteEvent(
                        pitchBendEvent.Status,
                        pitchBendEvent.byte1,
                        pitchBendEvent.byte2);
                    break;
                case SequenceNumberEvent sequenceNumberEvent:
                    WriteBytesDataMetaEvent(
                        SequenceNumberEvent.EventNumber,
                        (byte) (sequenceNumberEvent.number >> 8),
                        (byte) (sequenceNumberEvent.number & 0x00ff));
                    break;
                case TextEvent textEvent:
                    WriteTextMetaEvent(TextEvent.EventNumber, textEvent.text);
                    break;
                case CopyrightEvent copyrightEvent:
                    WriteTextMetaEvent(CopyrightEvent.EventNumber, copyrightEvent.text);
                    break;
                case TrackNameEvent trackNameEvent:
                    WriteTextMetaEvent(TrackNameEvent.EventNumber, trackNameEvent.name);
                    break;
                case InstrumentNameEvent instrumentNameEvent:
                    WriteTextMetaEvent(InstrumentNameEvent.EventNumber, instrumentNameEvent.name);
                    break;
                case LyricEvent lyricEvent:
                    WriteTextMetaEvent(LyricEvent.EventNumber, lyricEvent.lyric);
                    break;
                case MarkerEvent markerEvent:
                    WriteTextMetaEvent(MarkerEvent.EventNumber, markerEvent.text);
                    break;
                case QueueEvent queueEvent:
                    WriteTextMetaEvent(QueueEvent.EventNumber, queueEvent.text);
                    break;
                case ChannelPrefixEvent channelPrefixEvent:
                    WriteBytesDataMetaEvent(
                        ChannelPrefixEvent.EventNumber,
                        channelPrefixEvent.data);
                    break;
                case EndPointEvent endPointEvent:
                    WriteBytesDataMetaEvent(
                        EndPointEvent.EventNumber);
                    break;
                case TempoEvent tempoEvent:
                    WriteBytesDataMetaEvent(
                        TempoEvent.EventNumber,
                        (byte) ((tempoEvent.TickTempo >> 16) & 0xff),
                        (byte) ((tempoEvent.TickTempo >> 8) & 0xff),
                        (byte) (tempoEvent.TickTempo & 0xff));
                    break;
                case SmpteOffsetEvent smpteOffsetEvent:
                    WriteBytesDataMetaEvent(
                        SmpteOffsetEvent.EventNumber,
                        smpteOffsetEvent.hr,
                        smpteOffsetEvent.mn,
                        smpteOffsetEvent.se,
                        smpteOffsetEvent.fr,
                        smpteOffsetEvent.ff);
                    break;
                case BeatEvent beatEvent:
                    WriteBytesDataMetaEvent(
                        BeatEvent.EventNumber,
                        beatEvent.nn,
                        beatEvent.dd,
                        beatEvent.cc,
                        beatEvent.bb);
                    break;
                case KeyEvent keyEvent:
                    sbyte keyNumber = 0;
                    switch (keyEvent.noteKey)
                    {
                        case CFlatMajor:
                        case AFlatMinor:
                            keyNumber = -7;
                            break;
                        case GFlatMajor:
                        case EFlatMinor:
                            keyNumber = -6;
                            break;
                        case DFlatMajor:
                        case BFlatMinor:
                            keyNumber = -5;
                            break;
                        case AFlatMajor:
                        case FMinor:
                            keyNumber = -4;
                            break;
                        case EFlatMajor:
                        case CMinor:
                            keyNumber = -3;
                            break;
                        case BFlatMajor:
                        case GMinor:
                            keyNumber = -2;
                            break;
                        case FMajor:
                        case DMinor:
                            keyNumber = -1;
                            break;
                        case CMajor:
                        case AMinor:
                            keyNumber = 0;
                            break;
                        case GMajor:
                        case EMinor:
                            keyNumber = 1;
                            break;
                        case DMajor:
                        case BMinor:
                            keyNumber = 2;
                            break;
                        case AMajor:
                        case FSharpMinor:
                            keyNumber = 3;
                            break;
                        case EMajor:
                        case CSharpMinor:
                            keyNumber = 4;
                            break;
                        case BMajor:
                        case GSharpMinor:
                            keyNumber = 5;
                            break;
                        case FSharpMajor:
                        case DSharpMinor:
                            keyNumber = 6;
                            break;
                        case CSharpMajor:
                        case ASharpMinor:
                            keyNumber = 7;
                            break;
                    }

                    WriteBytesDataMetaEvent(
                        KeyEvent.EventNumber,
                        (byte) keyNumber,
                        (byte) (keyEvent.noteKey.IsMajor() ? 0 : 1));
                    break;
                case SequencerUniqueEvent sequencerUniqueEvent:
                    WriteBytesDataMetaEvent(SequencerUniqueEvent.EventNumber, sequencerUniqueEvent.data);
                    break;
                case UnknownMetaEvent unknownEvent:
                    WriteBytesDataMetaEvent(unknownEvent.eventNumber, unknownEvent.data);
                    break;
                case SysExEvent sysExEvent:
                    var sysExData = new byte[sysExEvent.data.Length + 1];
                    Buffer.BlockCopy(sysExEvent.data, 0, sysExData, 0, sysExEvent.data.Length);
                    sysExData[sysExData.Length - 1] = 0xf7;
                    var dataLengthBytes = ToMultiBytes((uint) sysExData.Length);
                    WriteEvent(0xf0, Concat(dataLengthBytes, sysExData));
                    break;
            }

            void WriteEvent(byte status, params byte[] data)
            {
                var tickBytes = ToMultiBytes(mTrkEvent.Ticks);
                stream.Write(tickBytes);
                stream.WriteByte(status);
                stream.Write(data);
            }

            void WriteMetaEvent(byte eventNumber, params byte[] data)
            {
                data = new byte[data.Length + 1];
                data[0] = eventNumber;
                WriteEvent(0xff, data);
            }

            void WriteTextMetaEvent(byte eventNumber, string text)
            {
                WriteMetaEvent(eventNumber);
                var textBytes = encoding.GetBytes(text);
                var textLengthBytes = ToMultiBytes((uint) textBytes.Length);
                stream.Write(textLengthBytes);
                stream.Write(textBytes);
            }

            void WriteBytesDataMetaEvent(byte eventNumber, params byte[] data)
            {
                WriteMetaEvent(eventNumber);
                var dataLengthBytes = ToMultiBytes((uint) data.Length);
                stream.Write(dataLengthBytes);
                stream.Write(data);
            }
        }

        private static byte[] ToMultiBytes(uint value)
        {
            var count = GetBytesCount(value);
            var bytes = new byte[count];
            var isLastByte = true;
            for (var i = count - 1; i >= 0; i--)
            {
                var num = value & 0b0111_1111;
                if (!isLastByte)
                    num |= 0b1000_0000;
                isLastByte = false;
                bytes[i] = (byte) num;
                value >>= 7;
            }

            return bytes;
        }

        private static int GetBytesCount(uint value)
        {
            var count = 1;
            uint n = 0b1000_0000;
            while (true)
            {
                if (value < n)
                    return count;
                n <<= 7;
                count++;
            }
        }
    }
}