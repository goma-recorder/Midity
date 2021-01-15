using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class PitchBendEvent : MidiEvent
    {
        public const byte STATUS_HEAD = 0xe0;
        private byte _upperBits;
        private byte _lowerBits;

        internal PitchBendEvent(uint ticks, byte channel, byte upperBits, byte lowerBits) : base(ticks, channel)
        {
            UpperBits = upperBits;
            LowerBits = lowerBits;
        }

        public PitchBendEvent(byte channel, short position) : base(0, channel)
        {
            Position = position;
        }

        public PitchBendEvent(byte channel, PitchWheelStep pitchWheelStep) : this(channel, (short) pitchWheelStep)
        {
        }

        protected override byte StatusHead => STATUS_HEAD;

        public byte UpperBits
        {
            get => _upperBits;
            set => SetIfInRange(nameof(UpperBits), out _upperBits, value, 0x7F);
        }

        public byte LowerBits
        {
            get => _lowerBits;
            set => SetIfInRange(nameof(LowerBits), out _lowerBits, value, 0x7F);
        }

        public short Position
        {
            get
            {
                // Turn the two bytes into a 14 bit value
                short fourteenBits = UpperBits;
                fourteenBits <<= 7;
                fourteenBits += LowerBits;
                fourteenBits -= 8192;
                return fourteenBits;
            }
            set
            {
                SetIfInRange(nameof(Position), out value, value, -8192, 8191);
                value += 8192;
                LowerBits = (byte) (value & 0x7F);
                value >>= 7;
                UpperBits = (byte) (value & 0x7F);
            }
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(PitchBendEvent);
        }
    }
}