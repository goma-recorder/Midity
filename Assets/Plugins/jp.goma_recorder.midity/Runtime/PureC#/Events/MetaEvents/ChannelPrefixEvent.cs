using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class ChannelPrefixEvent : MetaEvent
    {
        public const byte META_ID = 0x20;
        public override byte MetaId => META_ID;
        public byte data;

        internal ChannelPrefixEvent(uint ticks, byte data) : base(ticks)
        {
            this.data = data;
        }

        public ChannelPrefixEvent(byte data) : this(0, data)
        {
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(ChannelPrefixEvent);
        }
    }
}