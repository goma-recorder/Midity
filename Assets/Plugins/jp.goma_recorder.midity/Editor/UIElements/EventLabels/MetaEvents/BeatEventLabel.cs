using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class BeatEventLabel : MetaEventLabel
    {
        public BeatEventLabel() : base()
        {
        }

        public BeatEventLabel(BeatEvent beatEvent) : base(beatEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<BeatEventLabel, UxmlTraits>
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