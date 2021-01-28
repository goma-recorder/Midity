using System;
using System.Collections.Generic;
using System.Linq;

namespace Midity
{
    public class MidiTrack
    {
        private readonly List<MTrkEvent> _events = new List<MTrkEvent>();
        private readonly List<TempoEvent> _tempoEvents = new List<TempoEvent>();
        private readonly List<NoteEventPair> _noteEventPairs = new List<NoteEventPair>();
        private TrackNameEvent _trackNameEvent;

        internal MidiTrack(MidiFile midiFile, string name, uint deltaTime)
        {
            MidiFile = midiFile;
            DeltaTime = deltaTime;
            _events.Add(new TrackNameEvent(0, name) {Track = this});
            _events.Add(new EndOfTrackEvent(0) {Track = this});
        }

        internal MidiTrack(MidiFile midiFile, uint deltaTime, List<MTrkEvent> events)
        {
            MidiFile = midiFile;
            DeltaTime = deltaTime;
            _events = events;

            var noteTable = new List<OnNoteEvent>();
            foreach (var x in Events)
            {
                x.Track = this;
                switch (x)
                {
                    case OnNoteEvent onNoteEvent:
                        noteTable.Add(onNoteEvent);
                        break;
                    case OffNoteEvent offNoteEvent:
                        var on = noteTable.FirstOrDefault(xx => xx.NoteNumber == offNoteEvent.NoteNumber);
                        if (!(@on is null))
                        {
                            noteTable.Remove(@on);
                            _noteEventPairs.Add(new NoteEventPair(@on, offNoteEvent));
                        }

                        break;
                    case TrackNameEvent trackNameEvent:
                        _trackNameEvent = trackNameEvent;
                        break;
                    case TempoEvent tempoEvent:
                        _tempoEvents.Add(tempoEvent);
                        break;
                }
            }

            TotalSeconds = ConvertTicksToSecond(TotalTicks);
        }

        public MidiFile MidiFile { get; private set; }
        public uint DeltaTime { get; }
        public uint TotalTicks => Events.Last().Ticks;
        public float TotalSeconds { get; private set; }

        public IReadOnlyList<MTrkEvent> Events => _events;
        public IReadOnlyList<NoteEventPair> NoteEventPairs => _noteEventPairs;
        public IReadOnlyList<TempoEvent> TempoEvents => _tempoEvents;

        public string Name
        {
            get => _trackNameEvent?.name;
            set
            {
                if (_trackNameEvent == null)
                {
                    _trackNameEvent = new TrackNameEvent(0u, value);
                    _events.Insert(0, _trackNameEvent);
                    return;
                }

                _trackNameEvent.name = value;
            }
        }

        public uint ConvertSecondToTicks(float time)
        {
            var ticks = 0u;
            var tempo = 120f;
            ticks += (uint) Math.Floor(time / TotalSeconds) * TotalTicks;
            time %= TotalSeconds;
            var offsetTicks = 0u;
            foreach (var tempoEvent in _tempoEvents)
            {
                var length = (tempoEvent.Ticks - offsetTicks) * 60 / (tempo * DeltaTime);
                if (time > length)
                {
                    ticks += tempoEvent.Ticks - offsetTicks;
                    time -= length;
                    tempo = tempoEvent.Tempo;
                    offsetTicks = tempoEvent.Ticks;
                }
            }

            ticks += (uint) (time * tempo / 60 * DeltaTime);
            return ticks;
        }

        public uint ConvertSecondToTicks(float time, float offsetTime)
        {
            time += offsetTime;
            return ConvertSecondToTicks(time) - ConvertSecondToTicks(offsetTime);
        }

        public float ConvertTicksToSecond(uint tick)
        {
            var tempo = 120f;
            var time = 0f;
            var offsetTicks = 0u;
            foreach (var tempoEvent in _tempoEvents)
            {
                var length = tempoEvent.Ticks - offsetTicks;
                if (tick > length)
                {
                    time += length * 60 / (tempo * DeltaTime);
                    tick -= length;
                    tempo = tempoEvent.Tempo;
                    offsetTicks = tempoEvent.Ticks;
                }
                else
                {
                    break;
                }
            }

            time += tick * 60 / (tempo * DeltaTime);
            return time;
        }

        private (int index, int compare) GetEventIndexAtTick(uint tick)
        {
            var min = 0;
            var max = Events.Count - 1;
            var compare = 0;

            var mid = 0;
            while (min <= max)
            {
                mid = min + (max - min) / 2;
                compare = tick.CompareTo(Events[mid].Ticks);
                switch (compare)
                {
                    case 1:
                        min = mid + 1;
                        break;
                    case -1:
                        max = mid - 1;
                        break;
                    case 0:
                        while (mid > 0 && tick == Events[mid - 1].Ticks)
                            mid--;
                        return (mid, compare);
                }
            }

            return (mid, compare);
        }

        public MTrkEvent GetEventAtTick(uint tick)
        {
            var (index, compare) = GetEventIndexAtTick(tick);
            return compare == 0 ? Events[index] : null;
        }

        public MTrkEvent GetEventAfterTick(uint tick)
        {
            var (index, compare) = GetEventIndexAtTick(tick);

            switch (compare)
            {
                case 0:
                    return Events[index];
                case 1:
                    return ++index < Events.Count ? Events[index] : null;
                case -1:
                    while (index > 0)
                    {
                        if (Events[index - 1].Ticks == Events[index].Ticks)
                            index--;
                        else
                            return Events[index];
                    }

                    return Events[index];
                default:
                    return null;
            }
        }

        public MTrkEvent GetEventBeforeTick(uint tick)
        {
            var (index, compare) = GetEventIndexAtTick(tick);
            switch (compare)
            {
                case 0:
                    return Events[index];
                case 1:
                    while (index > 0)
                    {
                        if (Events[index - 1].Ticks == Events[index].Ticks)
                            index--;
                        else
                            return Events[index];
                    }

                    return Events[index];
                case -1:
                    index--;
                    if (index <= 0) return null;
                    while (index > 0)
                    {
                        if (Events[index - 1].Ticks == Events[index].Ticks)
                            index--;
                        else
                            return Events[index];
                    }

                    return Events[index];
                default:
                    return null;
            }
        }

        private void Validation<T>(T mTrkEvent) where T : MTrkEvent
        {
            switch (mTrkEvent)
            {
                case TrackNameEvent trackNameEvent:
                    throw new AggregateException($"{nameof(TrackNameEvent)}");
                case CopyrightEvent copyrightEvent:
                    throw new AggregateException($"{nameof(CopyrightEvent)}");
                case EndOfTrackEvent endPointEvent:
                    throw new AggregateException($"{nameof(EndOfTrackEvent)}");
            }
        }

        private void RegistTempoEvent(MTrkEvent mTrkEvent)
        {
            if (!(mTrkEvent is TempoEvent tempoEvent)) return;
            _tempoEvents.Add(tempoEvent);
            TotalSeconds = ConvertTicksToSecond(TotalTicks);
        }

        private void AddFirst(MTrkEvent mTrkEvent, uint offsetTicks)
        {
            var index = 0;
            for (; index < Events.Count; index++)
            {
                var e = Events[index];
                if (e.Ticks == 0)
                    switch (e)
                    {
                        case TempoEvent tempoEvent:
                        case TrackNameEvent trackNameEvent:
                        case CopyrightEvent copyrightEvent:
                            _events.RemoveAt(index);
                            _events.Insert(0, e);
                            continue;
                    }

                break;
            }

            _events.Insert(index, mTrkEvent);
            Events[index + 1].Ticks += offsetTicks;
            RegistTempoEvent(mTrkEvent);
        }

        private void SortEnd(MTrkEvent mTrkEvent)
        {
            if (Events.Last() != mTrkEvent)
                return;
            var endPointEvent = Events[Events.Count - 2];
            if (!(endPointEvent is EndOfTrackEvent)) throw new Exception();
            _events.RemoveAt(Events.Count - 2);
            _events.Add(endPointEvent);
            RegistTempoEvent(mTrkEvent);
        }

        public void AddEvent(MTrkEvent mTrkEvent, uint ticks)
        {
            Validation(mTrkEvent);
            mTrkEvent.Ticks = ticks;
            mTrkEvent.Track = this;

            for (var i = 0; i < Events.Count; i++)
                if (_events[i].Ticks > ticks)
                    _events.Insert(i, mTrkEvent);

            _events.Add(mTrkEvent);
            SortEnd(mTrkEvent);
        }

        public void AddEvent(MTrkEvent mTrkEvent, float time)
        {
            var ticks = ConvertSecondToTicks(time);
            AddEvent(mTrkEvent, ticks);
        }

        public void AddEvent(MTrkEvent originalEvent, MTrkEvent newEvent, int relativeTicks = 0)
        {
            AddEvent(newEvent, (uint) (originalEvent.Ticks + relativeTicks));
        }

        public void RemoveEvent(MTrkEvent mTrkEvent)
        {
            Validation(mTrkEvent);
            _events.Remove(mTrkEvent);
            if (mTrkEvent is TempoEvent tempoEvent)
            {
                _tempoEvents.Remove(tempoEvent);
                TotalSeconds = ConvertTicksToSecond(TotalTicks);
            }
        }

        public void MoveEvent(MTrkEvent mTrkEvent, uint tick)
        {
            RemoveEvent(mTrkEvent);
            AddEvent(mTrkEvent, tick);
        }

        public NoteEventPair AddNoteEvent(uint ticks, byte channel, byte noteNumber, byte velocity, uint length)
        {
            var onNoteEvent = new OnNoteEvent(ticks, channel, noteNumber, velocity);
            AddEvent(onNoteEvent, ticks);
            var offNoteEvent = new OffNoteEvent(ticks + length, channel, noteNumber);
            return new NoteEventPair(onNoteEvent, offNoteEvent);
        }
    }
}