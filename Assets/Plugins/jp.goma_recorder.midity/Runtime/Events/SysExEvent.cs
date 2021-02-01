namespace Midity
{
    public sealed class SysExEvent : MTrkEvent
    {
        public byte[] data;

        internal SysExEvent(uint ticks, byte[] data) : base(ticks)
        {
            this.data = data;
        }

        public SysExEvent(byte[] data) : this(0, data)
        {
        }

        public override byte Status => 0xf0;
    }
}