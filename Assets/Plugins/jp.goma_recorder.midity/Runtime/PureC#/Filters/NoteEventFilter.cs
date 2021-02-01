using System;
using UnityEngine;

namespace Midity
{
    public enum NoteTypeFilter
    {
        Both,
        On,
        Off
    }

    [Serializable]
    public sealed class NoteEventFilter : MTrkEventFilter
    {
        public NoteTypeFilter noteTypeFilter;
        public bool isNoteNameFilterEnabled = false;

        [SerializeField] private bool[] _noteNameFilterValueTable =
        {
            true, true, true, true, true, true, true, true, true, true, true, true
        };

        public bool GetNoteNameFilter(NoteName noteName)
        {
            return _noteNameFilterValueTable[(int) noteName];
        }

        public void SetNoteNameFilter(NoteName noteName, bool value)
        {
            _noteNameFilterValueTable[(int) noteName] = value;
        }

        public bool isNoteRangeFilterEnabled = false;
        [SerializeField] private byte _minNoteNumber = 0;
        [SerializeField] private byte _maxNoteNumber = 131;

        public byte MinNoteNumber
        {
            get => _minNoteNumber;
            set => _minNoteNumber = Math.Min(value, MaxNoteNumber);
        }

        public byte MaxNoteNumber
        {
            get => _maxNoteNumber;
            set => _maxNoteNumber = Math.Min(Math.Max(MinNoteNumber, value), (byte) 131);
        }

        public override MTrkEvent Filter(MTrkEvent mTrkEvent)
        {
            if (!(mTrkEvent is NoteEvent noteEvent)) return null;
            switch (noteTypeFilter)
            {
                case NoteTypeFilter.On when noteEvent is OffNoteEvent:
                case NoteTypeFilter.Off when noteEvent is OnNoteEvent:
                    return null;
            }

            if (isNoteRangeFilterEnabled &&
                (noteEvent.NoteNumber < MinNoteNumber || MaxNoteNumber < noteEvent.NoteNumber))
                return null;
            if (isNoteNameFilterEnabled && !GetNoteNameFilter(noteEvent.NoteName))
                return null;
            return noteEvent;
        }
    }
}