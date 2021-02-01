using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class CuePointEventLabel : MetaEventLabel
    {
        public CuePointEventLabel() : base()
        {
        }

        public CuePointEventLabel(CuePointEvent cuePointEvent) : base(cuePointEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<CuePointEventLabel, UxmlTraits>
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