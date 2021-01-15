using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class LyricEvent : MetaEvent
    {
        public const byte META_ID = 0x05;
        public override byte MetaId => META_ID;
        public string lyric;

        internal LyricEvent(uint ticks, string lyric) : base(ticks)
        {
            this.lyric = lyric;
        }

        public LyricEvent(string lyric) : this(0, lyric)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(lyric);
            return typeof(LyricEvent);
        }
    }
}