using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class TextEvent : MetaEvent
    {
        public const byte MetaNumber = 0x01;
        public override byte MetaId => MetaNumber;
        public string text;

        internal TextEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public TextEvent(string text) : this(0, text)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(text);
            return typeof(TextEvent);
        }
    }
}