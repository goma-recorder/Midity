using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class MarkerEventLabel : MetaEventLabel
    {
        public MarkerEventLabel() : base()
        {
        }

        public MarkerEventLabel(MarkerEvent markerEvent) : base(markerEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<MarkerEventLabel, UxmlTraits>
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