using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class EndOfTrackEvent : MetaEvent
    {
        public const byte META_ID = 0x2f;
        public override byte MetaId => META_ID;

        internal EndOfTrackEvent(uint ticks) : base(ticks)
        {
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(EndOfTrackEvent);
        }
    }
}