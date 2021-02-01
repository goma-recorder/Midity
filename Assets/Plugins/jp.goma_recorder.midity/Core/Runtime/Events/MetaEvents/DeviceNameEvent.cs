namespace Midity
{
    public sealed class DeviceNameEvent : MetaEvent
    {
        public const byte META_ID = 0x09;
        public override byte MetaId => META_ID;
        public string name;

        internal DeviceNameEvent(uint ticks, string name) : base(ticks)
        {
            this.name = name;
        }

        public DeviceNameEvent(string name) : this(0, name)
        {
        }
    }
}