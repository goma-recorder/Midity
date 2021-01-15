using System;
using System.Collections.Generic;
using System.Linq;

namespace Midity
{
    public sealed class UnknownMetaEvent : MetaEvent
    {
        public readonly byte[] data;
        public readonly byte metaId;
        public override byte MetaId => metaId;

        internal UnknownMetaEvent(uint ticks, byte metaId, byte[] data) : base(ticks)
        {
            this.metaId = metaId;
            this.data = data;
        }

        protected override Type ToString(List<string> list)
        {
            list.AddRange(data.Select(d => d.ToString()));
            return typeof(UnknownMetaEvent);
        }
    }
}