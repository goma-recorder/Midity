using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class DeviceNameEvent : MetaEvent
    {
        public const byte MetaNumber = 0x09;
        public override byte MetaId => MetaNumber;
        public string name;

        internal DeviceNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public DeviceNameEvent(string name) : this(0, name)
        {
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(ProgramNameEvent);
        }
    }
}