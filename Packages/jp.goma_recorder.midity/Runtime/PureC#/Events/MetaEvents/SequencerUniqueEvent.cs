﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Midity
{
    public sealed class SequencerUniqueEvent : MetaEvent
    {
        public const byte MetaNumber = 0x7f;
        public override byte MetaId => MetaNumber;
        public byte[] data;

        internal SequencerUniqueEvent(uint ticks, byte[] data) : base(ticks)
        {
            this.data = data;
        }

        public SequencerUniqueEvent(byte[] data) : this(0, data)
        {
        }

        protected override Type ToString(List<string> list)
        {
            list.AddRange(data.Select(n => n.ToString()));
            return typeof(SequencerUniqueEvent);
        }
    }
}