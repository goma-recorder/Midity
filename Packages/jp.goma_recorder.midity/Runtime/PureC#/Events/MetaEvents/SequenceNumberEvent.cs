using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class SequenceNumberEvent : MetaEvent
    {
        public const byte MetaNumber = 0x00;
        public override byte MetaId => MetaNumber;
        public ushort number;

        internal SequenceNumberEvent(uint ticks, ushort number) : base(ticks)
        {
            this.number = number;
        }

        public SequenceNumberEvent(ushort number) : this(0, number)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(number.ToString());
            return typeof(SequenceNumberEvent);
        }
    }
}