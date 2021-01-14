using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Midity.NoteKey;

namespace Midity
{
    public sealed class MidiDeserializer
    {
        private readonly MidiDataStreamReader _reader;

        public MidiDeserializer(Stream stream, Encoding encoding)
        {
            _reader = new MidiDataStreamReader(stream, encoding ?? Encoding.ASCII);
        }

        public MidiDeserializer(Stream stream, int codePage)
            : this(stream, Encoding.GetEncoding(codePage))
        {
        }

        public MidiDeserializer(Stream stream, string codeName)
            : this(stream, Encoding.GetEncoding(codeName))
        {
        }

        private (MidiFile, uint) LoadMidiFile()
        {
            // Chunk type
            if (_reader.ReadChars(4) != "MThd")
                throw new FormatException("Can't find header chunk.");

            // Chunk length
            if (_reader.ReadBEUInt(4) != 6u)
                throw new FormatException("Length of header chunk must be 6.");

            // Format
            var format = (byte) _reader.ReadBEUShort();

            // Number of tracks
            var trackCount = _reader.ReadBEUInt(2);

            // Ticks per quarter note
            var tpqn = _reader.ReadBEUInt(2);
            if ((tpqn & 0x8000u) != 0)
                throw new FormatException("SMPTE time code is not supported.");
            var midiFile = new MidiFile(tpqn, _reader.encoding, format);

            return (midiFile, trackCount);
        }

        public MidiFile Load()
        {
            var (midiFile, trackCount) = LoadMidiFile();
            for (var i = 0; i < trackCount; i++)
                ReadTrack(i, midiFile);
            return midiFile;
        }

        public (MidiFile midiFile, byte[] trackBytes) LoadTrackBytes()
        {
            var (midiFile, trackCount) = LoadMidiFile();
            var byteCount = _reader.Length - _reader.Position;
            var bytes = _reader.ReadBytes((uint) byteCount);
            _reader.Position -= byteCount;

            for (var i = 0; i < trackCount; i++)
                ReadTrack(i, midiFile);
            return (midiFile, bytes);
        }

        public void ReadTrack(int trackNumber, MidiFile midiFile)
        {
            // Chunk type
            if (_reader.ReadChars(4) != "MTrk")
                throw new FormatException("Can't find track chunk.");

            // Chunk length
            long chunkEnd = _reader.ReadBEUInt(4);
            chunkEnd += _reader.Position;

            // MIDI event sequence
            var events = new List<MTrkEvent>();
            byte stat = 0;
            while (_reader.Position < chunkEnd)
            {
                var mtrkEvent = ReadEvent(ref stat);
                events.Add(mtrkEvent);
            }

            midiFile.AddTrack(trackNumber, events);
        }

        internal MTrkEvent ReadEvent(ref byte status)
        {
            // Delta time
            var ticks = _reader.ReadMultiByteValue();

            // Status byte
            if ((_reader.PeekByte() & 0x80u) != 0)
                status = _reader.ReadByte();
            else if (status == 0)
                throw new Exception("Status byte required for running status.");

            switch (status)
            {
                case var stat when 0x80 <= stat && stat <= 0xef:
                    var channel = (byte) (status & 0x0f);
                    return ReadMidiEvent(ticks, status, channel);
                case 0xff:
                    var metaNumber = _reader.ReadByte();
                    var length = _reader.ReadMultiByteValue();
                    return ReadMetaEvent(ticks, metaNumber, length);
                case 0xf0:
                    return ReadSysExEvent(ticks);
                default:
                    throw new Exception("Invalid status byte found.");
            }
        }

        private MTrkEvent ReadMetaEvent(uint ticks, byte metaNumber, uint length)
        {
            byte[] bytes;
            switch (metaNumber)
            {
                // 00
                case SequenceNumberEvent.MetaNumber:
                    bytes = _reader.ReadBytes(length);
                    var number = (ushort) ((bytes[0] << 8) | bytes[1]);
                    return new SequenceNumberEvent(ticks, number);
                // 01
                case TextEvent.MetaNumber:
                    return new TextEvent(ticks, _reader.ReadChars(length));
                // 02
                case CopyrightEvent.MetaNumber:
                    return new CopyrightEvent(ticks, _reader.ReadChars(length));
                // 03
                case TrackNameEvent.MetaNumber:
                    return new TrackNameEvent(ticks, _reader.ReadChars(length));
                // 04
                case InstrumentNameEvent.MetaNumber:
                    return new InstrumentNameEvent(ticks, _reader.ReadChars(length));
                // 05
                case LyricEvent.MetaNumber:
                    return new LyricEvent(ticks, _reader.ReadChars(length));
                // 06
                case MarkerEvent.MetaNumber:
                    return new MarkerEvent(ticks, _reader.ReadChars(length));
                // 07
                case QueueEvent.MetaNumber:
                    return new QueueEvent(ticks, _reader.ReadChars(length));
                // 08
                case ProgramNameEvent.MetaNumber:
                    return new ProgramNameEvent(ticks, _reader.ReadChars(length));
                // 09
                case DeviceNameEvent.MetaNumber:
                    return new DeviceNameEvent(ticks, _reader.ReadChars(length));
                // 20
                case ChannelPrefixEvent.MetaNumber:
                    return new ChannelPrefixEvent(ticks, _reader.ReadByte());
                // 21
                case PortNumberEvent.MetaNumber:
                    return new PortNumberEvent(ticks, _reader.ReadByte());
                // 2f
                case EndPointEvent.MetaNumber:
                    return new EndPointEvent(ticks);
                // 51
                case TempoEvent.MetaNumber:
                    return new TempoEvent(ticks, _reader.ReadBEUInt((byte) length));
                // 54
                case SmpteOffsetEvent.MetaNumber:
                    bytes = _reader.ReadBytes(length);
                    return new SmpteOffsetEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4]);
                // 58
                case BeatEvent.MetaNumber:
                    bytes = _reader.ReadBytes(length);
                    return new BeatEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3]);
                // 59
                case KeyEvent.MetaNumber:
                    var keyAccidentalSign = (KeyAccidentalSign) (sbyte) _reader.ReadByte();
                    var tonality = (Tonality) _reader.ReadByte();
                    return new KeyEvent(ticks, (keyAccidentalSign, tonality).ToKey());
                // 7f
                case SequencerUniqueEvent.MetaNumber:
                    return new SequencerUniqueEvent(ticks, _reader.ReadBytes(length));
                // Unknown
                default:
                    return new UnknownMetaEvent(ticks, metaNumber, _reader.ReadBytes(length));
            }
        }

        private MTrkEvent ReadSysExEvent(uint ticks)
        {
            var length = _reader.ReadMultiByteValue() - 1;
            var bytes = new byte[length];

            for (var i = 0; i < length; i++)
                bytes[i] = _reader.ReadByte();

            if (_reader.ReadByte() == 0xf7)
                return new SysExEvent(ticks, bytes);
            throw new Exception();
        }

        private MTrkEvent ReadMidiEvent(uint ticks, byte status, byte channel)
        {
            switch (status & 0xf0)
            {
                // note event
                case 0x80:
                case 0x90:
                    var noteNumber = _reader.ReadByte();
                    var velocity = (status & 0xe0u) == 0xc0u ? (byte) 0 : _reader.ReadByte();
                    var isNoteOn = (status & 0xf0) == 0x90 && velocity != 0;
                    return new NoteEvent(ticks, isNoteOn, channel, noteNumber, velocity);
                // a0
                case PolyphonicKeyPressureEvent.StatusHead:
                    return new PolyphonicKeyPressureEvent(ticks, channel, _reader.ReadByte(), _reader.ReadByte());
                // b0
                case ControlChangeEvent.StatusHead:
                    var controlChangeNumber = _reader.ReadByte();
                    var bytes = (status & 0xe0u) == 0xc0u ? (byte) 0 : _reader.ReadByte();
                    return new ControlChangeEvent(ticks, channel, controlChangeNumber, bytes);
                // c0
                case ProgramChangeEvent.StatusHead:
                    var programNumber = _reader.ReadBEUShort();
                    return new ProgramChangeEvent(ticks, channel, programNumber);
                // d0
                case ChannelPressureEvent.StatusHead:
                    return new ChannelPressureEvent(ticks, channel, _reader.ReadByte());
                // e0
                case PitchBendEvent.StatusHead:
                    var byte1 = _reader.ReadByte();
                    var byte2 = _reader.ReadByte();
                    return new PitchBendEvent(ticks, channel, byte1, byte2);
                default:
                    throw new Exception("Unknown midi event");
            }
        }
    }
}