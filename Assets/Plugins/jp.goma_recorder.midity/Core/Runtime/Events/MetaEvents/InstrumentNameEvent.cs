namespace Midity
{
    public sealed class InstrumentNameEvent : MetaEvent
    {
        public const byte META_ID = 0x04;
        public override byte MetaId => META_ID;
        public string name;

        internal InstrumentNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public InstrumentNameEvent(string name) : this(0, name)
        {
        }
    }
}