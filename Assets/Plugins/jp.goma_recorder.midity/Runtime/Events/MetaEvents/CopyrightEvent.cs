namespace Midity
{
    public sealed class CopyrightEvent : MetaEvent
    {
        public const byte META_ID = 0x02;
        public override byte MetaId => META_ID;
        public string text;

        internal CopyrightEvent(uint ticks, string text) : base(ticks)
        {
            this.text = text;
        }

        public CopyrightEvent(string text) : this(0, text)
        {
        }
    }
}