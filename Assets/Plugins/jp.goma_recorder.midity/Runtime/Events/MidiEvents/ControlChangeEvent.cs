namespace Midity
{
    public sealed class ControlChangeEvent : MidiEvent
    {
        public const byte STATUS_HEAD = 0xb0;
        public Controller controller;
        public byte data;

        internal ControlChangeEvent(uint ticks, byte channel, Controller controller, byte data) : base(ticks,
            channel)
        {
            this.controller = controller;
            this.data = data;
        }

        public ControlChangeEvent(byte channel, Controller controller, byte data) : this(0, channel,
            controller, data)
        {
        }

        protected override byte StatusHead => STATUS_HEAD;
    }
}