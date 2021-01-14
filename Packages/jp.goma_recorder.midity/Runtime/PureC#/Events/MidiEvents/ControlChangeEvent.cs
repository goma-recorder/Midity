using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class ControlChangeEvent : MidiEvent
    {
        public const byte StatusHead = 0xb0;
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

        public byte Status => (byte) (StatusHead | Channel);

        protected override Type ToString(List<string> list)
        {
            list.Add(controller.ToString());
            list.Add(data.ToString());
            return typeof(ControlChangeEvent);
        }
    }
}