namespace Midity
{
    public sealed class ProgramNameEvent : MetaEvent
    {
        public const byte META_ID = 0x08;
        public override byte MetaId => META_ID;
        public string name;

        internal ProgramNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public ProgramNameEvent(string name) : this(0, name)
        {
        }
    }
}