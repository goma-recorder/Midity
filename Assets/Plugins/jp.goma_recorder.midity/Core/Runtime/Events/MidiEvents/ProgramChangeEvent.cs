namespace Midity
{
    public sealed class ProgramChangeEvent : MidiEvent
    {
        public const byte STATUS_HEAD = 0xc0;
        public GeneralMidiInstrument instrument;

        internal ProgramChangeEvent(uint ticks, byte channel, GeneralMidiInstrument instrument) : base(ticks, channel)
        {
            this.instrument = instrument;
        }

        public ProgramChangeEvent(byte channel, GeneralMidiInstrument instrument) : this(0, channel, instrument)
        {
        }

        protected override byte StatusHead => STATUS_HEAD;
    }
}