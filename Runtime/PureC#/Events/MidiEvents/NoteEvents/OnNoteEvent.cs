namespace Midity
{
    public sealed class OnNoteEvent : NoteEvent
    {
        public const byte STATUS_HEAD = 0x90;
        private byte _velocity;

        internal OnNoteEvent(uint ticks, byte channel, byte noteNumber, byte velocity) : base(ticks, channel,
            noteNumber)
        {
            Velocity = velocity;
        }

        internal OnNoteEvent(uint ticks, byte channel, NoteName noteName, NoteOctave noteOctave, byte velocity) : base(
            ticks, channel, noteName, noteOctave)
        {
            Velocity = velocity;
        }

        public byte Velocity
        {
            get => _velocity;
            set => SetIfInRange(nameof(Velocity), out _velocity, value, 1, 255);
        }

        protected override byte StatusHead => STATUS_HEAD;
    }
}