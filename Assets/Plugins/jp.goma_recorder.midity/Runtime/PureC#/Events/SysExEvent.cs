using System;
using System.Collections.Generic;
using System.Linq;

namespace Midity
{
    public sealed class SysExEvent : MTrkEvent
    {
        public byte[] data;

        internal SysExEvent(uint ticks, byte[] data) : base(ticks)
        {
            this.data = data;
        }

        public SysExEvent(byte[] data) : this(0, data)
        {
        }

        public override byte Status => 0xf0;

        protected override Type ToString(List<string> list)
        {
            list.AddRange(data.Select(d => d.ToString()));
            return typeof(SysExEvent);
        }
    }
}