namespace Midity
{
    public sealed class OffNoteEvent : NoteEvent
    {
        public const byte STATUS_HEAD = 0x80;

        internal OffNoteEvent(uint ticks, byte channel, byte noteNumber) : base(ticks, channel, noteNumber)
        {
        }

        internal OffNoteEvent(uint ticks, byte channel, NoteName noteName, NoteOctave noteOctave) : base(ticks, channel,
            noteName, noteOctave)
        {
        }

        protected override byte StatusHead => STATUS_HEAD;
    }
}