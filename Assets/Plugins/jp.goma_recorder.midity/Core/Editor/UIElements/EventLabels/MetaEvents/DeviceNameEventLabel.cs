using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class DeviceNameEventLabel : MetaEventLabel
    {
        public DeviceNameEventLabel() : base()
        {
        }

        public DeviceNameEventLabel(DeviceNameEvent deviceNameEvent) : base(deviceNameEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<DeviceNameEventLabel, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}