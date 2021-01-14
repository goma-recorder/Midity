using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class CopyrightEvent : MetaEvent
    {
        public const byte MetaNumber = 0x02;
        public override byte MetaId => MetaNumber;
        public string text;

        internal CopyrightEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public CopyrightEvent(string text) : this(0, text)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(text);
            return typeof(CopyrightEvent);
        }
    }
}