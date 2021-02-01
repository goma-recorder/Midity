namespace Midity
{
    public abstract class MetaEvent : MTrkEvent
    {
        protected MetaEvent(uint ticks) : base(ticks)
        {
        }

        public sealed override byte Status => 0xff;
        public abstract byte MetaId { get; }
    }
}