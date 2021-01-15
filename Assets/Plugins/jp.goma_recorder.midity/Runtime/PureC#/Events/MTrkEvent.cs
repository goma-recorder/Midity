using System;
using System.Collections.Generic;
using System.Text;

namespace Midity
{
    public abstract class MTrkEvent
    {
        protected MTrkEvent(uint ticks)
        {
            Ticks = ticks;
        }

        public abstract byte Status { get; }
        public uint Ticks { get; internal set; }

        protected abstract Type ToString(List<string> list);

        public override string ToString()
        {
            var list = new List<string>();
            var sb = new StringBuilder();
            var typeName = ToString(list).Name;
            sb.Append($"{typeName}: {Ticks}, ");
            sb.Append(string.Join(", ", list));
            return sb.ToString();
        }

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