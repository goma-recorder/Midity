using System;
using System.Collections.Generic;

namespace Midity
{
    public abstract class NoteEvent : MidiEvent
    {
        private byte _noteNumber;

        internal NoteEvent(uint ticks, byte channel, byte noteNumber) : base(ticks,
            channel)
        {
            NoteNumber = noteNumber;
        }

        internal NoteEvent(uint ticks, byte channel, NoteName noteName, NoteOctave noteOctave) : this(ticks, channel,
            (noteName, noteOctave).ToNoteNumber())
        {
        }

        public byte NoteNumber
        {
            get => _noteNumber;
            internal set =>
                SetIfInRange(nameof(NoteNumber), out _noteNumber, value, 131);
        }

        public NoteName NoteName
        {
            get => NoteEnumUtil.ToNoteName(NoteNumber);
            internal set => NoteNumber = (value, NoteOctave).ToNoteNumber();
        }

        public NoteOctave NoteOctave
        {
            get => NoteEnumUtil.ToNoteOctave(NoteNumber);
            internal set => NoteNumber = (NoteName, value).ToNoteNumber();
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(NoteNumber.ToString());
            return typeof(NoteEvent);
        }
    }
}