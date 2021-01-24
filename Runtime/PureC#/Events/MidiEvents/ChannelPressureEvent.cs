namespace Midity
{
    public sealed class ChannelPressureEvent : MidiEvent
    {
        public const byte STATUS_HEAD = 0xd0;
        public byte pressure;

        internal ChannelPressureEvent(uint ticks, byte channel, byte pressure) : base(ticks, channel)
        {
            this.pressure = pressure;
        }

        public ChannelPressureEvent(byte channel, byte pushData) : this(0, channel, pushData)
        {
        }

        protected override byte StatusHead => STATUS_HEAD;
    }
}