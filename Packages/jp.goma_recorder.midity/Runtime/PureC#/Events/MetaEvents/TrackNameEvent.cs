﻿using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class TrackNameEvent : MetaEvent
    {
        public const byte MetaNumber = 0x03;
        public override byte MetaId => MetaNumber;
        public string name;

        internal TrackNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public TrackNameEvent(string name) : this(0, name)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(name);
            return typeof(TrackNameEvent);
        }
    }
}