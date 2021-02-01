using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class TimeSignatureEventLabel : MetaEventLabel
    {
        public TimeSignatureEventLabel() : base()
        {
        }

        public TimeSignatureEventLabel(TimeSignatureEvent timeSignatureEvent) : base(timeSignatureEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<TimeSignatureEventLabel, UxmlTraits>
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