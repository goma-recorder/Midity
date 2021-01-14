using System;
using System.Collections.Generic;

namespace Midity
{
    public sealed class KeyEvent : MetaEvent
    {
        public const byte MetaNumber = 0x59;
        public override byte MetaId => MetaNumber;
        public NoteKey noteKey;

        internal KeyEvent(uint ticks, NoteKey noteKey) : base(ticks)
        {
            this.noteKey = noteKey;
        }

        public KeyEvent(NoteKey noteKey) : this(0, noteKey)
        {
        }

        public KeyAccidentalSign KeyAccidentalSign
        {
            get => noteKey.ToKeyAccidentalSign_Tonality().keyAccidentalSign;
            set => noteKey = (value, Tonality).ToKey();
        }

        public Tonality Tonality
        {
            get => noteKey.ToKeyAccidentalSign_Tonality().tonality;
            set => noteKey = (KeyAccidentalSign, value).ToKey();
        }

        protected override Type ToString(List<string> list)
        {
            list.Add(noteKey.ToString());
            return typeof(KeyEvent);
        }
    }
}