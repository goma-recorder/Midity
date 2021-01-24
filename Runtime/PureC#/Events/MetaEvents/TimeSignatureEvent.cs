using System;

namespace Midity
{
    public sealed class TimeSignatureEvent : MetaEvent
    {
        public const byte META_ID = 0x58;
        public override byte MetaId => META_ID;
        public byte numerator;
        public byte denominator;
        public byte midiClocksPerClick;
        public byte numberOfNotated32nds;

        internal TimeSignatureEvent(uint ticks, byte numerator, byte denominator, byte midiClocksPerClick,
            byte numberOfNotated32nds) : base(ticks)
        {
            this.numerator = numerator;
            this.denominator = denominator;
            this.midiClocksPerClick = midiClocksPerClick;
            this.numberOfNotated32nds = numberOfNotated32nds;
        }

        public TimeSignatureEvent(byte numerator, byte denominator, byte midiClocksPerClick,
            byte numberOfNotated32nds) : this(
            0, numerator, denominator, midiClocksPerClick, numberOfNotated32nds)
        {
        }

        public byte TopNumber => numerator;
        public byte BottomNumber => (byte) Math.Pow(2, denominator);
    }
}