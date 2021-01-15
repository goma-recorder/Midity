using System.Text;
using System.IO;

namespace Midity
{
    // MIDI binary data stream reader
    internal sealed class MidiDataStreamReader
    {
        #region Constructor

        internal MidiDataStreamReader(Stream stream, Encoding encoding)
        {
            _stream = stream;
            _stream.Position = 0;
            this.encoding = encoding;
        }

        #endregion

        #region Internal members

        private readonly Stream _stream;
        public readonly Encoding encoding;

        #endregion

        #region Current reading position

        public long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public long Length => _stream.Length;

        #endregion

        #region Reader methods

        public byte PeekByte()
        {
            var data = ReadByte();
            _stream.Position--;
            return data;
        }

        public byte ReadByte()
        {
            return (byte) _stream.ReadByte();
        }

        public byte[] ReadBytes(uint len)
        {
            var bytes = new byte[len];
            _stream.Read(bytes, 0, (int) len);
            return bytes;
        }

        public string ReadChars(uint length)
        {
            return encoding.GetString(ReadBytes(length));
        }

        public uint ReadBEUInt(byte length)
        {
            var number = 0u;
            for (byte i = 0; i < length; i++) number += (uint) ReadByte() << ((length - i - 1) * 8);

            return number;
        }

        public short ReadBEShort()
        {
            var bytes = ReadBytes(2);
            return (short) ((bytes[0] << 8) + bytes[1]);
        }

        public ushort ReadBEUShort()
        {
            var bytes = ReadBytes(2);
            return (ushort) ((bytes[0] << 8) + bytes[1]);
        }

        public uint ReadMultiByteValue()
        {
            var v = 0u;
            while (true)
            {
                uint b = ReadByte();
                v += b & 0x7fu;
                if (b < 0x80u) break;
                v <<= 7;
            }

            return v;
        }

        public static uint ReadMultiByteValue(byte[] bytes)
        {
            var i = 0;
            var v = 0u;
            while (true)
            {
                uint b = bytes[i];
                i++;
                v += b & 0x7fu;
                if (b < 0x80u) break;
                v <<= 7;
            }

            return v;
        }

        #endregion
    }
}