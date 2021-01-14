using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class ProgramNameEvent : MetaEvent
    {
        public const byte MetaNumber = 0x08;
        public override byte MetaId => MetaNumber;
        public string name;

        internal ProgramNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public ProgramNameEvent(string name) : this(0, name)
        {
        }

        protected override Type ToString(List<string> list)
        {
            return typeof(ProgramNameEvent);
        }
    }
}