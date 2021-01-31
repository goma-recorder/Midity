namespace Midity
{
    public sealed class SequenceNumberEvent : MetaEvent
    {
        public const byte META_ID = 0x00;
        public override byte MetaId => META_ID;
        public ushort number;

        internal SequenceNumberEvent(uint ticks, ushort number) : base(ticks)
        {
            this.number = number;
        }

        public SequenceNumberEvent(ushort number) : this(0, number)
        {
        }
    }
}