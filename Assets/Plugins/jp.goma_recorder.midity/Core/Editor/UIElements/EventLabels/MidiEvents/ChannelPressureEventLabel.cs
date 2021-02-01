using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class ChannelPressureEventLabel : MidiEventLabel
    {
        public ChannelPressureEventLabel() : base()
        {
        }

        public ChannelPressureEventLabel(ChannelPressureEvent channelPressureEvent) : base(channelPressureEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<ChannelPressureEventLabel, UxmlTraits>
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