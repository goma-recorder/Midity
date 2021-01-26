namespace Midity
{
    public sealed class NoteEventPair
    {
        public readonly OnNoteEvent onNoteEvent;
        public readonly OffNoteEvent offNoteEvent;

        internal NoteEventPair(OnNoteEvent onNoteEvent, OffNoteEvent offNoteEvent)
        {
            this.onNoteEvent = onNoteEvent;
            this.offNoteEvent = offNoteEvent;
            this.onNoteEvent.NoteEventPair = this.offNoteEvent.NoteEventPair = this;
        }

        public uint OnTick => onNoteEvent.Ticks;
        public uint OffTick => offNoteEvent.Ticks;
        public uint LengthTick => OffTick - OnTick;

        public byte Channel
        {
            get => onNoteEvent.Channel;
            set => onNoteEvent.Channel = offNoteEvent.Channel = value;
        }

        public byte NoteNumber
        {
            get => onNoteEvent.NoteNumber;
            set => onNoteEvent.NoteNumber = offNoteEvent.NoteNumber = value;
        }

        public NoteName NoteName
        {
            get => onNoteEvent.NoteName;
            set => onNoteEvent.NoteName = offNoteEvent.NoteName = value;
        }

        public NoteOctave NoteOctave
        {
            get => onNoteEvent.NoteOctave;
            set => onNoteEvent.NoteOctave = offNoteEvent.NoteOctave = value;
        }

        public byte Velocity
        {
            get => onNoteEvent.Velocity;
            set => onNoteEvent.Velocity = value;
        }
    }
}