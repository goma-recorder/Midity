using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class PortNumberEvent : MetaEvent
    {
        public const byte MetaNumber = 0x21;
        public override byte MetaId => MetaNumber;
        private byte _number;

        internal PortNumberEvent(uint ticks, byte number) : base(ticks)
        {
            _number = number;
        }

        public PortNumberEvent(byte number) : this(0, number)
        {
        }

        public byte Number
        {
            get => _number;
            set => SetIfInRange(nameof(Number), out _number, value, 0x7F);
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(PortNumberEvent);
        }
    }
}