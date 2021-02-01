using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class TempoEventLabel : MetaEventLabel
    {
        public TempoEventLabel() : base()
        {
        }

        public TempoEventLabel(TempoEvent tempoEvent) : base(tempoEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<TempoEventLabel, UxmlTraits>
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