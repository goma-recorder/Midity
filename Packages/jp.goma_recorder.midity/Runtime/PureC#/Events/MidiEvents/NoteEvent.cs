using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class NoteEvent : MidiEvent
    {
        public readonly bool isNoteOn;

        private byte _noteNumber;
        private byte _velocity;

        internal NoteEvent(uint ticks, bool isNoteOn, byte channel, byte noteNumber, byte velocity) : base(ticks,
            channel)
        {
            this.isNoteOn = isNoteOn;
            NoteNumber = noteNumber;
            Velocity = velocity;
        }

        internal NoteEvent(uint ticks, bool isNoteOn, byte channel, NoteName noteName, NoteOctave noteOctave,
            byte velocity) : this(ticks, isNoteOn, channel, (noteName, noteOctave).ToNoteNumber(), velocity)
        {
        }

        public byte Status => (byte) ((isNoteOn ? 0x90 : 0x80) | Channel);

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

        public byte Velocity
        {
            get => _velocity;
            internal set
            {
                if (isNoteOn)
                    _velocity = value;
            }
        }


        protected override Type ToString(List<string> list)
        {
            list.Add(isNoteOn.ToString());
            list.Add(NoteNumber.ToString());
            list.Add(Velocity.ToString());
            return typeof(NoteEvent);
        }
    }
}