using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class EndPointEvent : MetaEvent
    {
        public const byte MetaNumber = 0x2f;
        public override byte MetaId => MetaNumber;

        internal EndPointEvent(uint ticks) : base(ticks)
        {
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(EndPointEvent);
        }
    }
}