using System;

namespace Midity
{
    public abstract class MTrkEvent
    {
        protected MTrkEvent(uint ticks)
        {
            Ticks = ticks;
        }

        public abstract byte Status { get; }
        public MidiTrack Track { get; internal set; }
        public uint Ticks { get; internal set; }
        public float Seconds => Track?.ConvertTicksToSecond(Ticks) ?? -1f;

        private static void ThrowException(string name, long input, long min, long max)
        {
            if (input < min || max < input)
                throw new Exception($"{name} is outside of range.({min}-{max})");
        }

        protected static void SetIfInRange(string name, out byte target, byte input, byte max)
        {
            SetIfInRange(name, out target, input, 0, max);
        }

        protected static void SetIfInRange(string name, out byte target, byte input, byte min, byte max)
        {
            ThrowException(name, input, min, max);
            target = input;
        }

        protected static void SetIfInRange(string name, out uint target, uint input, uint max)
        {
            SetIfInRange(name, out target, input, 0, max);
        }

        protected static void SetIfInRange(string name, out uint target, uint input, uint min, uint max)
        {
            ThrowException(name, input, min, max);
            target = input;
        }

        protected static void SetIfInRange(string name, out short target, short input, short min, short max)
        {
            ThrowException(name, input, min, max);
            target = input;
        }
    }
}