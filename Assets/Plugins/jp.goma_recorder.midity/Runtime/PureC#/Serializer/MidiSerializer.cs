﻿using System;
using System.IO;
using System.Text;

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
            WriteBEUint((uint) midiFile.format, 2);
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
            var lastTicks = 0u;
            foreach (var mTrkEvent in midiTrack.Events)
            {
                SerializeEvent(mTrkEvent, encoding, stream, lastTicks);
                lastTicks = mTrkEvent.Ticks;
            }

            var chunkEnd = stream.Position - chunkEndDataPosition - 4;
            stream.Seek(chunkEndDataPosition, SeekOrigin.Begin);
            for (var i = 0; i < 4; i++)
                stream.WriteByte((byte) ((chunkEnd >> (8 * (4 - i - 1))) & 0xff));
            stream.Seek(0, SeekOrigin.End);
        }

        internal static void SerializeEvent(MTrkEvent mTrkEvent, Encoding encoding, Stream stream, uint lastTicks)
        {
            switch (mTrkEvent)
            {
                case OnNoteEvent onNoteEvent:
                    WriteEvent(onNoteEvent.NoteNumber, onNoteEvent.Velocity);
                    break;
                case OffNoteEvent offNoteEvent:
                    WriteEvent(offNoteEvent.NoteNumber, 0);
                    break;
                case PolyphonicKeyPressureEvent polyphonicKeyPressureEvent:
                    WriteEvent(polyphonicKeyPressureEvent.noteNumber, polyphonicKeyPressureEvent.pressure);
                    break;
                case ControlChangeEvent controlChangeEvent:
                    WriteEvent((byte) controlChangeEvent.controller, controlChangeEvent.data);
                    break;
                case ProgramChangeEvent programChangeEvent:
                    WriteEvent((byte) programChangeEvent.instrument);
                    break;
                case ChannelPressureEvent channelPressureEvent:
                    WriteEvent(channelPressureEvent.pressure);
                    break;
                case PitchBendEvent pitchBendEvent:
                    WriteEvent(pitchBendEvent.UpperBits, pitchBendEvent.LowerBits);
                    break;
                case SequenceNumberEvent sequenceNumberEvent:
                    WriteBytesDataMetaEvent(
                        sequenceNumberEvent,
                        (byte) (sequenceNumberEvent.number >> 8),
                        (byte) (sequenceNumberEvent.number & 0xff));
                    break;
                case TextEvent textEvent:
                    WriteTextMetaEvent(textEvent, textEvent.text);
                    break;
                case CopyrightEvent copyrightEvent:
                    WriteTextMetaEvent(copyrightEvent, copyrightEvent.text);
                    break;
                case TrackNameEvent trackNameEvent:
                    WriteTextMetaEvent(trackNameEvent, trackNameEvent.name);
                    break;
                case InstrumentNameEvent instrumentNameEvent:
                    WriteTextMetaEvent(instrumentNameEvent, instrumentNameEvent.name);
                    break;
                case LyricEvent lyricEvent:
                    WriteTextMetaEvent(lyricEvent, lyricEvent.lyric);
                    break;
                case MarkerEvent markerEvent:
                    WriteTextMetaEvent(markerEvent, markerEvent.text);
                    break;
                case CuePointEvent cuePointEvent:
                    WriteTextMetaEvent(cuePointEvent, cuePointEvent.text);
                    break;
                case ProgramNameEvent programNameEvent:
                    WriteTextMetaEvent(programNameEvent, programNameEvent.name);
                    break;
                case DeviceNameEvent deviceNameEvent:
                    WriteTextMetaEvent(deviceNameEvent, deviceNameEvent.name);
                    break;
                case ChannelPrefixEvent channelPrefixEvent:
                    WriteBytesDataMetaEvent(channelPrefixEvent, channelPrefixEvent.data);
                    break;
                case PortNumberEvent portNumberEvent:
                    WriteBytesDataMetaEvent(portNumberEvent, portNumberEvent.Number);
                    break;
                case EndOfTrackEvent endOfTrackEvent:
                    WriteBytesDataMetaEvent(endOfTrackEvent);
                    break;
                case TempoEvent tempoEvent:
                    WriteBytesDataMetaEvent(
                        tempoEvent,
                        (byte) ((tempoEvent.TickTempo >> 16) & 0xff),
                        (byte) ((tempoEvent.TickTempo >> 8) & 0xff),
                        (byte) (tempoEvent.TickTempo & 0xff));
                    break;
                case SmpteOffsetEvent smpteOffsetEvent:
                    WriteBytesDataMetaEvent(
                        smpteOffsetEvent,
                        smpteOffsetEvent.hr,
                        smpteOffsetEvent.mn,
                        smpteOffsetEvent.se,
                        smpteOffsetEvent.fr,
                        smpteOffsetEvent.ff);
                    break;
                case TimeSignatureEvent timeSignatureEvent:
                    WriteBytesDataMetaEvent(
                        timeSignatureEvent,
                        timeSignatureEvent.numerator,
                        timeSignatureEvent.denominator,
                        timeSignatureEvent.midiClocksPerClick,
                        timeSignatureEvent.numberOfNotated32nds);
                    break;
                case KeyEvent keyEvent:
                    WriteBytesDataMetaEvent(
                        keyEvent,
                        (byte) keyEvent.KeyAccidentalSign,
                        (byte) keyEvent.Tonality);
                    break;
                case SequencerUniqueEvent sequencerUniqueEvent:
                    WriteBytesDataMetaEvent(sequencerUniqueEvent, sequencerUniqueEvent.data);
                    break;
                case UnknownMetaEvent unknownEvent:
                    WriteBytesDataMetaEvent(unknownEvent, unknownEvent.data);
                    break;
                case SysExEvent sysExEvent:
                    var sysExData = new byte[sysExEvent.data.Length + 1];
                    Buffer.BlockCopy(sysExEvent.data, 0, sysExData, 0, sysExEvent.data.Length);
                    sysExData[sysExData.Length - 1] = 0xf7;
                    var dataLengthBytes = ToMultiBytes((uint) sysExData.Length);
                    WriteEvent(Concat(dataLengthBytes, sysExData));
                    break;
            }

            void WriteEvent(params byte[] data)
            {
                var tickBytes = ToMultiBytes(mTrkEvent.Ticks - lastTicks);
                stream.Write(tickBytes);
                stream.WriteByte(mTrkEvent.Status);
                stream.Write(data);
            }

            void WriteMetaEvent(MetaEvent metaEvent, params byte[] data)
            {
                data = new byte[data.Length + 1];
                data[0] = metaEvent.MetaId;
                WriteEvent(data);
            }

            void WriteTextMetaEvent(MetaEvent metaEvent, string text)
            {
                WriteMetaEvent(metaEvent);
                var textBytes = encoding.GetBytes(text);
                var textLengthBytes = ToMultiBytes((uint) textBytes.Length);
                stream.Write(textLengthBytes);
                stream.Write(textBytes);
            }

            void WriteBytesDataMetaEvent(MetaEvent metaEvent, params byte[] data)
            {
                WriteMetaEvent(metaEvent);
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