namespace Midity
{
    public sealed class MarkerEvent : MetaEvent
    {
        public const byte META_ID = 0x06;
        public override byte MetaId => META_ID;
        public string text;

        internal MarkerEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public MarkerEvent(string text) : this(0, text)
        {
        }
    }
}