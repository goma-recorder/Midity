using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class BeatEvent : MetaEvent
    {
        public const byte MetaNumber = 0x58;
        public override byte MetaId => MetaNumber;
        public byte numerator;
        public byte denominator;
        public byte midiClocksPerClick;
        public byte numberOfNotated32nds;

        internal BeatEvent(uint ticks, byte numerator, byte denominator, byte midiClocksPerClick,
            byte numberOfNotated32nds) : base(ticks)
        {
            this.numerator = numerator;
            this.denominator = denominator;
            this.midiClocksPerClick = midiClocksPerClick;
            this.numberOfNotated32nds = numberOfNotated32nds;
        }

        public BeatEvent(byte numerator, byte denominator, byte midiClocksPerClick, byte numberOfNotated32nds) : this(0,
            numerator, denominator, midiClocksPerClick, numberOfNotated32nds)
        {
        }

        public byte TopNumber => numerator;
        public byte BottomNumber => (byte) Math.Pow(2, denominator);

        protected override Type ToString(List<string> list)
        {
            list.Add(numerator.ToString());
            list.Add(denominator.ToString());
            list.Add(midiClocksPerClick.ToString());
            list.Add(numberOfNotated32nds.ToString());
            return typeof(BeatEvent);
        }
    }
}