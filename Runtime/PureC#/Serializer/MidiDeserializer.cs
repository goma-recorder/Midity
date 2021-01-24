using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Midity
{
    public sealed class MidiDeserializer
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;

        public MidiDeserializer(Stream stream, Encoding encoding)
        {
            _stream = stream;
            _stream.Position = 0;
            _encoding = encoding ?? Encoding.ASCII;
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
            if (_stream.ReadChars(4, _encoding) != "MThd")
                throw new FormatException("Can't find header chunk.");

            // Chunk length
            if (_stream.ReadBEUInt(4) != 6u)
                throw new FormatException("Length of header chunk must be 6.");

            // Format
            var format = (Format) _stream.ReadBEUShort();

            // Number of tracks
            var trackCount = _stream.ReadBEUInt(2);

            // Ticks per quarter note
            var tpqn = _stream.ReadBEUInt(2);
            if ((tpqn & 0x8000u) != 0)
                throw new FormatException("SMPTE time code is not supported.");
            var midiFile = new MidiFile(tpqn, _encoding, format);

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
            var byteCount = _stream.Length - _stream.Position;
            var bytes = _stream.ReadBytes((uint) byteCount);
            _stream.Position -= byteCount;

            for (var i = 0; i < trackCount; i++)
                ReadTrack(i, midiFile);
            return (midiFile, bytes);
        }

        public void ReadTrack(int trackNumber, MidiFile midiFile)
        {
            // Chunk type
            if (_stream.ReadChars(4, _encoding) != "MTrk")
                throw new FormatException("Can't find track chunk.");

            // Chunk length
            long chunkEnd = _stream.ReadBEUInt(4);
            chunkEnd += _stream.Position;

            // MIDI event sequence
            var events = new List<MTrkEvent>();
            byte stat = 0;
            var ticks = 0u;
            while (_stream.Position < chunkEnd)
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
            ticks += _stream.ReadMultiByteValue();

            // Status byte
            if ((_stream.PeekByte() & 0x80u) != 0)
                status = _stream.ReadByteValue();
            else if (status == 0)
                throw new Exception("Status byte required for running status.");

            switch (status)
            {
                case var s when 0x80 <= s && s <= 0xef:
                    var channel = (byte) (status & 0x0f);
                    return ReadMidiEvent(ticks, status, channel);
                case 0xff:
                    var metaNumber = _stream.ReadByteValue();
                    var length = _stream.ReadMultiByteValue();
                    var lastPosition = _stream.Position;
                    var metaEvent = ReadMetaEvent(ticks, metaNumber, length);
                    if (length != _stream.Position - lastPosition)
                    {
                        var value = _stream.Position - lastPosition - length;
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
                    return new SequenceNumberEvent(ticks, _stream.ReadBEUShort());
                // 01
                case TextEvent.META_ID:
                    return new TextEvent(ticks, _stream.ReadChars(length, _encoding));
                // 02
                case CopyrightEvent.META_ID:
                    return new CopyrightEvent(ticks, _stream.ReadChars(length, _encoding));
                // 03
                case TrackNameEvent.META_ID:
                    return new TrackNameEvent(ticks, _stream.ReadChars(length, _encoding));
                // 04
                case InstrumentNameEvent.META_ID:
                    return new InstrumentNameEvent(ticks, _stream.ReadChars(length, _encoding));
                // 05
                case LyricEvent.META_ID:
                    return new LyricEvent(ticks, _stream.ReadChars(length, _encoding));
                // 06
                case MarkerEvent.META_ID:
                    return new MarkerEvent(ticks, _stream.ReadChars(length, _encoding));
                // 07
                case CuePointEvent.META_ID:
                    return new CuePointEvent(ticks, _stream.ReadChars(length, _encoding));
                // 08
                case ProgramNameEvent.META_ID:
                    return new ProgramNameEvent(ticks, _stream.ReadChars(length, _encoding));
                // 09
                case DeviceNameEvent.META_ID:
                    return new DeviceNameEvent(ticks, _stream.ReadChars(length, _encoding));
                // 20
                case ChannelPrefixEvent.META_ID:
                    return new ChannelPrefixEvent(ticks, _stream.ReadByteValue());
                // 21
                case PortNumberEvent.META_ID:
                    return new PortNumberEvent(ticks, _stream.ReadByteValue());
                // 2f
                case EndOfTrackEvent.META_ID:
                    return new EndOfTrackEvent(ticks);
                // 51
                case TempoEvent.META_ID:
                    return new TempoEvent(ticks, _stream.ReadBEUInt((byte) length));
                // 54
                case SmpteOffsetEvent.META_ID:
                    bytes = _stream.ReadBytes(length);
                    return new SmpteOffsetEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4]);
                // 58
                case TimeSignatureEvent.META_ID:
                    bytes = _stream.ReadBytes(length);
                    return new TimeSignatureEvent(ticks, bytes[0], bytes[1], bytes[2], bytes[3]);
                // 59
                case KeyEvent.META_ID:
                    var keyAccidentalSign = (KeyAccidentalSign) (sbyte) _stream.ReadByte();
                    var tonality = (Tonality) _stream.ReadByte();
                    return new KeyEvent(ticks, (keyAccidentalSign, tonality).ToKey());
                // 7f
                case SequencerUniqueEvent.META_ID:
                    return new SequencerUniqueEvent(ticks, _stream.ReadBytes(length));
                // Unknown
                default:
                    return new UnknownMetaEvent(ticks, metaNumber, _stream.ReadBytes(length));
            }
        }

        private MTrkEvent ReadSysExEvent(uint ticks)
        {
            var bytes = _stream.ReadBytes(_stream.ReadMultiByteValue() - 1);
            if (_stream.ReadByte() == 0xf7)
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
                    var noteNumber = _stream.ReadByteValue();
                    var velocity = (status & 0xe0u) == 0xc0u ? (byte) 0 : _stream.ReadByteValue();
                    var isNoteOn = (status & 0xf0) == 0x90 && velocity != 0;
                    if (isNoteOn)
                        return new OnNoteEvent(ticks, channel, noteNumber, velocity);
                    else
                        return new OffNoteEvent(ticks, channel, noteNumber);
                // a0
                case PolyphonicKeyPressureEvent.STATUS_HEAD:
                    return new PolyphonicKeyPressureEvent(ticks, channel, _stream.ReadByteValue(),
                        _stream.ReadByteValue());
                // b0
                case ControlChangeEvent.STATUS_HEAD:
                    return new ControlChangeEvent(ticks, channel, (Controller) _stream.ReadByte(),
                        _stream.ReadByteValue());
                // c0
                case ProgramChangeEvent.STATUS_HEAD:
                    return new ProgramChangeEvent(ticks, channel, (GeneralMidiInstrument) _stream.ReadByte());
                // d0
                case ChannelPressureEvent.STATUS_HEAD:
                    return new ChannelPressureEvent(ticks, channel, _stream.ReadByteValue());
                // e0
                case PitchBendEvent.STATUS_HEAD:
                    var byte1 = _stream.ReadByteValue();
                    var byte2 = _stream.ReadByteValue();
                    return new PitchBendEvent(ticks, channel, byte1, byte2);
                default:
                    throw new Exception("Unknown midi event");
            }
        }
    }

    internal static class StreamReaderExtension
    {
        public static byte PeekByte(this Stream stream)
        {
            var data = stream.ReadByteValue();
            stream.Position--;
            return data;
        }

        public static byte ReadByteValue(this Stream stream)
        {
            return (byte) stream.ReadByte();
        }

        public static byte[] ReadBytes(this Stream stream, uint len)
        {
            var bytes = new byte[len];
            stream.Read(bytes, 0, (int) len);
            return bytes;
        }

        public static string ReadChars(this Stream stream, uint length, Encoding encoding)
        {
            return encoding.GetString(stream.ReadBytes(length));
        }

        public static uint ReadBEUInt(this Stream stream, byte length)
        {
            var number = 0u;
            for (byte i = 0; i < length; i++) number += (uint) stream.ReadByteValue() << ((length - i - 1) * 8);

            return number;
        }

        public static short ReadBEShort(this Stream stream)
        {
            var bytes = stream.ReadBytes(2);
            return (short) ((bytes[0] << 8) + bytes[1]);
        }

        public static ushort ReadBEUShort(this Stream stream)
        {
            var bytes = stream.ReadBytes(2);
            return (ushort) ((bytes[0] << 8) + bytes[1]);
        }

        public static uint ReadMultiByteValue(this Stream stream)
        {
            var v = 0u;
            while (true)
            {
                uint b = stream.ReadByteValue();
                v += b & 0x7fu;
                if (b < 0x80u) break;
                v <<= 7;
            }

            return v;
        }
    }
}