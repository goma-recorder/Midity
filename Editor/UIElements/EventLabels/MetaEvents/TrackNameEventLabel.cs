using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class TrackNameEventLabel : MetaEventLabel
    {
        public TrackNameEventLabel() : base()
        {
        }

        public TrackNameEventLabel(TrackNameEvent trackNameEvent) : base(trackNameEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<TrackNameEventLabel, UxmlTraits>
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