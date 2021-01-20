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

        internal MidiTrack(string name, uint deltaTime)
        {
            DeltaTime = deltaTime;
            _events.Add(new TrackNameEvent(0, name));
            _events.Add(new EndPointEvent(0));
        }

        internal MidiTrack(uint deltaTime, List<MTrkEvent> events)
        {
            DeltaTime = deltaTime;
            _events = events;

            var noteTable = new List<OnNoteEvent>();
            foreach (var x in Events)
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

            AllSeconds = ConvertTicksToSecond(AllTicks);
        }

        public uint DeltaTime { get; }
        public uint AllTicks => Events.Last().Ticks;
        public float AllSeconds { get; private set; }

        public IReadOnlyList<MTrkEvent> Events => _events;
        public IReadOnlyList<NoteEventPair> NoteEventPairs => _noteEventPairs;
        public uint Bars => (AllTicks + DeltaTime * 4 - 1) / (DeltaTime * 4);

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
            ticks += (uint) Math.Floor(time / AllSeconds) * AllTicks;
            time %= AllSeconds;
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

        private void Validation<T>(T mTrkEvent) where T : MTrkEvent
        {
            switch (mTrkEvent)
            {
                case TrackNameEvent trackNameEvent:
                    throw new AggregateException($"{nameof(TrackNameEvent)}");
                case CopyrightEvent copyrightEvent:
                    throw new AggregateException($"{nameof(CopyrightEvent)}");
                case EndPointEvent endPointEvent:
                    throw new AggregateException($"{nameof(EndPointEvent)}");
            }
        }

        private void RegistTempoEvent(MTrkEvent mTrkEvent)
        {
            if (!(mTrkEvent is TempoEvent tempoEvent)) return;
            _tempoEvents.Add(tempoEvent);
            AllSeconds = ConvertTicksToSecond(AllTicks);
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
            if (!(endPointEvent is EndPointEvent)) throw new Exception();
            _events.RemoveAt(Events.Count - 2);
            _events.Add(endPointEvent);
            RegistTempoEvent(mTrkEvent);
        }

        public void AddEvent(MTrkEvent mTrkEvent, uint ticks)
        {
            Validation(mTrkEvent);
            for (var i = 0; i < Events.Count; i++)
                if (_events[i].Ticks > ticks)
                    _events.Insert(i, mTrkEvent);

            mTrkEvent.Ticks = ticks;
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
                AllSeconds = ConvertTicksToSecond(AllTicks);
            }
        }

        // public NoteEventPair AddNoteEvent(MTrkEvent rootEvent, NoteEvent onNoteEvent, uint length,
        //     float absoluteTime)
        // {
        //     var absoluteTick = ConvertSecondToTicks(absoluteTime);
        //     return AddNoteEvent(rootEvent, onNoteEvent, length, absoluteTick);
        // }
        //
        // private NoteEventPair AddNoteEvent(OnNoteEvent onNoteEvent, uint length, float absoluteTime)
        // {
        //     AddEvent(onNoteEvent, absoluteTime);
        //     var offNoteEvent = AddOffNoteEvent(onNoteEvent, length);
        //     return new NoteEventPair(onNoteEvent, offNoteEvent, length);
        // }
        //
        // private NoteEventPair AddNoteEvent(OnNoteEvent onNoteEvent, uint length, uint absoluteTick)
        // {
        //     AddEvent(onNoteEvent, absoluteTick);
        //     var offNoteEvent = AddOffNoteEvent(onNoteEvent, length);
        //     return new NoteEventPair(onNoteEvent, offNoteEvent, length);
        // }
        //
        // private NoteEventPair AddNoteEvent(MTrkEvent rootEvent, OnNoteEvent onNoteEvent, uint length,
        //     int ticks = 0)
        // {
        //     AddEvent(rootEvent, onNoteEvent, ticks);
        //     var offNoteEvent = AddOffNoteEvent(onNoteEvent, length);
        //     return new NoteEventPair(onNoteEvent, offNoteEvent, length);
        // }
        //
        // private OffNoteEvent AddOffNoteEvent(NoteEvent onNoteEvent, uint length)
        // {
        //     var offNoteEvent = new OffNoteEvent(0, onNoteEvent.Channel, onNoteEvent.NoteNumber);
        //     AddEvent(onNoteEvent, offNoteEvent, (int) length);
        //     return offNoteEvent;
        // }
    }
}