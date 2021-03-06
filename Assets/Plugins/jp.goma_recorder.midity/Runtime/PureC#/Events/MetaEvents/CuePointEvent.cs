﻿namespace Midity
{
    public sealed class CuePointEvent : MetaEvent
    {
        public const byte META_ID = 0x07;
        public override byte MetaId => META_ID;
        public string text;

        internal CuePointEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public CuePointEvent(string text) : this(0, text)
        {
        }
    }
}