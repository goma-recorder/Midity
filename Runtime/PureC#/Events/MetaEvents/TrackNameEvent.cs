namespace Midity
{
    public sealed class TrackNameEvent : MetaEvent
    {
        public const byte META_ID = 0x03;
        public override byte MetaId => META_ID;
        public string name;

        internal TrackNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public TrackNameEvent(string name) : this(0, name)
        {
        }
    }
}