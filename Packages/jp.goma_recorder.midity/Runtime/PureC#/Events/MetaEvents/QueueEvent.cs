using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class QueueEvent : MetaEvent
    {
        public const byte META_ID = 0x07;
        public override byte MetaId => META_ID;
        public string text;

        internal QueueEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public QueueEvent(string text) : this(0, text)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(text);
            return typeof(QueueEvent);
        }
    }
}