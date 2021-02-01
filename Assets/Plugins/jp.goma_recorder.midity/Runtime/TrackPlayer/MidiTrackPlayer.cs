using System;

namespace Midity
{
    public class MidiTrackPlayer
    {
        #region Parameters

        public MidiTrackPlayer(MidiTrack track, Action<MTrkEvent> onPush, bool canLoop)
        {
            this.track = track;
            this.onPush = onPush;
            this.canLoop = canLoop;
        }

        public readonly MidiTrack track;
        public Action<MTrkEvent> onPush;

        #endregion

        #region MIDI signal emission

        private int _headIndex;
        private uint _lastTick;
        private float _lastTime;
        public float LastTime => IsFinished ? track.TotalSeconds : _lastTime;
        public bool canLoop = true;
        public bool IsFinished => _headIndex >= track.Events.Count;

        public void ResetHead(float time)
        {
            if (!canLoop && time >= track.TotalSeconds)
            {
                _headIndex = track.Events.Count;
                return;
            }

            _lastTime = time % track.TotalSeconds;
            ResetHead(track.ConvertSecondToTicks(_lastTime));
        }

        private void ResetHead(uint targetTick)
        {
            if (!canLoop && targetTick > track.TotalTicks) return;
            targetTick %= track.TotalTicks;
            for (var i = 0; i < track.Events.Count; i++)
                if (targetTick >= track.Events[i].Ticks)
                {
                    _lastTick = targetTick;
                    _headIndex = i;
                    return;
                }
        }

        private void PlayByDeltaTicks(uint deltaTicks)
        {
            if (IsFinished) return;
            _lastTick += deltaTicks;
            _lastTime = track.ConvertTicksToSecond(_lastTick);

            while (true)
            {
                if (_lastTick < track.Events[_headIndex].Ticks) return;

                onPush?.Invoke(track.Events[_headIndex]);
                _headIndex++;
                if (_headIndex == track.Events.Count)
                {
                    if (!canLoop) return;
                    _headIndex = 0;
                    ResetHead(0);
                    _lastTick %= track.TotalTicks;
                }
            }
        }

        public void PlayByDeltaTime(float deltaTime)
        {
            if (IsFinished || deltaTime < 0 || !canLoop && _headIndex > track.Events.Count - 1)
                return;

            _lastTime += deltaTime;
            var currentTick = track.ConvertSecondToTicks(_lastTime);

            var deltaTick = currentTick - _lastTick;
            while (track.Events[_headIndex].Ticks - _lastTick <= deltaTick)
            {
                deltaTick -= track.Events[_headIndex].Ticks - _lastTick;
                _lastTick = track.Events[_headIndex].Ticks;
                onPush?.Invoke(track.Events[_headIndex]);
                _headIndex++;
                if (_headIndex == track.Events.Count)
                    if (canLoop)
                    {
                        deltaTime = _lastTime - track.TotalSeconds;
                        ResetHead(0f);
                        PlayByDeltaTime(deltaTime);
                    }
                    else
                    {
                        _lastTime = track.TotalSeconds;
                        return;
                    }
            }
        }

        #endregion
    }
}