using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class ChannelPrefixEventLabel : MetaEventLabel
    {
        public ChannelPrefixEventLabel() : base()
        {
        }

        public ChannelPrefixEventLabel(ChannelPrefixEvent channelPrefixEvent) : base(channelPrefixEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<ChannelPrefixEventLabel, UxmlTraits>
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