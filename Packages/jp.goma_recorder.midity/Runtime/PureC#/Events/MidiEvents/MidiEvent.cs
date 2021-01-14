using System;

namespace Midity
{
    public abstract class MidiEvent : MTrkEvent
    {
        private byte _channel;

        internal MidiEvent(uint ticks, byte channel) : base(ticks)
        {
            Channel = channel;
        }

        public byte Channel
        {
            get => _channel;
            internal set =>
                SetIfInRange(nameof(Channel), out _channel, value, 16);
        }
    }
}