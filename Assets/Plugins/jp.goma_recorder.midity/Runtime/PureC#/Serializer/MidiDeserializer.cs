using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            var format = (Format) _reader.ReadBEUShort();

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
            var ticks = 0u;
            while (_reader.Position < chunkEnd)
            {
                var mtrkEvent = ReadEvent(ref stat, ticks);
                events.Add(mtrkEvent);
                ticks = mtrkEvent.Ticks;
            }

            midiFile.AddTrack(trackNumber, events);
        }

        internal MTrkEvent ReadEvent(ref byte status, uint ticks)
        {
            // Delta time
            ticks += _reader.ReadMultiByteValue();

            // Status byte
            if ((_reader.PeekByte() & 0x80u) != 0)
                status = _reader.ReadByte();
            else if (status == 0)
                throw new Exception("Status byte required for running status.");

            switch (status)
            {
                case var s when 0x80 <= s && s <= 0xef:
                    var channel = (byte) (status & 0x0f);
                    return ReadMidiEvent(ticks, status, channel);
                case 0xff:
                    var metaNumber = _reader.ReadByte();
                    var length = _reader.ReadMultiByteValue();
                    var lastPosition = _reader.Position;
                    var metaEvent = ReadMetaEvent(ticks, metaNumber, length);
                    if (length != _reader.Position - lastPosition)
                    {
                        var value = _reader.Position - lastPosition - length;
                        throw new Exception($"Data length does not match. {value} {(value > 0 ? "over" : "under")}");
                    }

                    return metaEvent;
                case 0xf0:
                    return ReadSysExEvent(ticks);
                default:
                    throw new Exception("Invalid status byte found.");
            }
        }

        private MetaEvent ReadMetaEvent(uint ticks, byte metaNumber, uint length)
        {
            byte[] bytes;
            switch (metaNumber)
            {
                // 00
                case SequenceNumberEvent.META_ID:
                    return new SequenceNumberEvent(ticks, _reader.ReadBEUShort());
                // 01
                case TextEvent.META_ID:
                    return new TextEvent(ticks, _reader.ReadChars(length));
                // 02
                case CopyrightEvent.META_ID:
                    return new CopyrightEvent(ticks, _reader.ReadChars(length));
                // 03
                case TrackNameEvent.META_ID:
                    return new TrackNameEvent(ticks, _reader.ReadChars(length));
                // 04
                case InstrumentNameEvent.META_ID:
                    return new InstrumentNameEvent(ticks, _reader.ReadChars(length));
                // 05
                case LyricEvent.META_ID:
                    return new LyricEvent(ticks, _reader.ReadChars(length));
                // 06
                case MarkerEvent.META_ID:
                    return new MarkerEvent(ticks, _reader.ReadChars(length));
                // 07
                case QueueEvent.META_ID:
                    return new QueueEvent(ticks, _reader.ReadChars(length));
                // 08
                case ProgramNameEvent.META_ID:
                    return new ProgramNameEvent(ticks, _reader.ReadChars(length));
                // 09
                case DeviceNameEvent.META_ID:
                    return new DeviceNameEvent(ticks, _reader.ReadChars(length));
                // 20
                case ChannelPrefixEvent.META_ID:
                    return new ChannelPrefixEvent(ticks, _reader.ReadByte());
                // 21
                case PortNumberEvent.META_ID:
                    return new PortNumberEvent(ticks, _reader.ReadByte());
                // 2f
                case EndOfTrackEvent.META_ID:
                    return new EndOfTrackEvent(ticks);
                // 51
                case TempoEvent.META_ID:
                    return new TempoEvent(ticks, _reader.ReadBEUInt((byte) length));
                // 54
                case SmpteOffsetEvent.META_ID:
                    bytes = _reader.ReadBytes(length);
                    return new SmpteOffsetEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4]);
                // 58
                case BeatEvent.META_ID:
                    bytes = _reader.ReadBytes(length);
                    return new BeatEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3]);
                // 59
                case KeyEvent.META_ID:
                    var keyAccidentalSign = (KeyAccidentalSign) (sbyte) _reader.ReadByte();
                    var tonality = (Tonality) _reader.ReadByte();
                    return new KeyEvent(ticks, (keyAccidentalSign, tonality).ToKey());
                // 7f
                case SequencerUniqueEvent.META_ID:
                    return new SequencerUniqueEvent(ticks, _reader.ReadBytes(length));
                // Unknown
                default:
                    return new UnknownMetaEvent(ticks, metaNumber, _reader.ReadBytes(length));
            }
        }

        private MTrkEvent ReadSysExEvent(uint ticks)
        {
            var bytes = _reader.ReadBytes(_reader.ReadMultiByteValue() - 1);
            if (_reader.ReadByte() == 0xf7)
                return new SysExEvent(ticks, bytes);
            throw new Exception();
        }

        private MTrkEvent ReadMidiEvent(uint ticks, byte status, byte channel)
        {
            switch (status & 0xf0)
            {
                // 80
                case OffNoteEvent.STATUS_HEAD:
                // 90
                case OnNoteEvent.STATUS_HEAD:
                    var noteNumber = _reader.ReadByte();
                    var velocity = (status & 0xe0u) == 0xc0u ? (byte) 0 : _reader.ReadByte();
                    var isNoteOn = (status & 0xf0) == 0x90 && velocity != 0;
                    if (isNoteOn)
                        return new OnNoteEvent(ticks, channel, noteNumber, velocity);
                    else
                        return new OffNoteEvent(ticks, channel, noteNumber);
                // a0
                case PolyphonicKeyPressureEvent.STATUS_HEAD:
                    return new PolyphonicKeyPressureEvent(ticks, channel, _reader.ReadByte(), _reader.ReadByte());
                // b0
                case ControlChangeEvent.STATUS_HEAD:
                    return new ControlChangeEvent(ticks, channel, (Controller) _reader.ReadByte(), _reader.ReadByte());
                // c0
                case ProgramChangeEvent.STATUS_HEAD:
                    return new ProgramChangeEvent(ticks, channel, (GeneralMidiInstrument) _reader.ReadByte());
                // d0
                case ChannelPressureEvent.STATUS_HEAD:
                    return new ChannelPressureEvent(ticks, channel, _reader.ReadByte());
                // e0
                case PitchBendEvent.STATUS_HEAD:
                    var byte1 = _reader.ReadByte();
                    var byte2 = _reader.ReadByte();
                    return new PitchBendEvent(ticks, channel, byte1, byte2);
                default:
                    throw new Exception("Unknown midi event");
            }
        }
    }
}