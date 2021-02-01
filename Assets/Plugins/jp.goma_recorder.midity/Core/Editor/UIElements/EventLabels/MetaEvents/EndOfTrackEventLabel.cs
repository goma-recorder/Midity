using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class EndOfTrackEventLabel : MetaEventLabel
    {
        public EndOfTrackEventLabel() : base()
        {
        }

        public EndOfTrackEventLabel(EndOfTrackEvent endOfTrackEvent) : base(endOfTrackEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<EndOfTrackEventLabel, UxmlTraits>
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