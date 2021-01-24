namespace Midity
{
    public sealed class SequencerUniqueEvent : MetaEvent
    {
        public const byte META_ID = 0x7f;
        public override byte MetaId => META_ID;
        public byte[] data;

        internal SequencerUniqueEvent(uint ticks, byte[] data) : base(ticks)
        {
            this.data = data;
        }

        public SequencerUniqueEvent(byte[] data) : this(0, data)
        {
        }
    }
}