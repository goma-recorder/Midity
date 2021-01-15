namespace Midity
{
    public abstract class MidiEvent : MTrkEvent
    {
        protected abstract byte StatusHead { get; }
        private byte _channel;

        internal MidiEvent(uint ticks, byte channel) : base(ticks)
        {
            Channel = channel;
        }

        public sealed override byte Status => (byte) (StatusHead | Channel);

        public byte Channel
        {
            get => _channel;
            internal set =>
                SetIfInRange(nameof(Channel), out _channel, value, 16);
        }
    }
}