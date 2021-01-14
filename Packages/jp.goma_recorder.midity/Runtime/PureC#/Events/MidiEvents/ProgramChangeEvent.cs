using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class ProgramChangeEvent : MidiEvent
    {
        public const byte StatusHead = 0xc0;
        public GeneralMidiInstrument instrument;

        internal ProgramChangeEvent(uint ticks, byte channel, GeneralMidiInstrument instrument) : base(ticks, channel)
        {
            this.instrument = instrument;
        }

        public ProgramChangeEvent(byte channel, GeneralMidiInstrument instrument) : this(0, channel, instrument)
        {
        }

        public byte Status => (byte) (StatusHead | Channel);

        protected override Type ToString(List<string> list)
        {
            list.Add(instrument.ToString());
            return typeof(ProgramChangeEvent);
        }
    }
}