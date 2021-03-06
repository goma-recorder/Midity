﻿namespace Midity
{
    public sealed class TempoEvent : MetaEvent
    {
        public const byte META_ID = 0x51;
        public override byte MetaId => META_ID;
        private uint _tickTempo;

        internal TempoEvent(uint ticks, uint tickTempo) : base(ticks)
        {
            TickTempo = tickTempo;
        }

        public TempoEvent(uint tickTempo) : this(0, tickTempo)
        {
        }

        public TempoEvent(float tempo) : this((uint) (60000000f / tempo))
        {
        }

        public uint TickTempo
        {
            get => _tickTempo;
            internal set
                => SetIfInRange(nameof(TickTempo), out _tickTempo, value, 0xff_ff_ff);
        }

        public float Tempo
        {
            get => 60000000f / TickTempo;
            internal set => _tickTempo = (uint) (60000000f / value);
        }
    }
}