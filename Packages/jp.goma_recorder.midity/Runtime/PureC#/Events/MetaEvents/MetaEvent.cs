namespace Midity
{
    public abstract class MetaEvent : MTrkEvent
    {
        protected MetaEvent(uint ticks) : base(ticks)
        {
        }

        public abstract byte MetaId { get; }
    }
}