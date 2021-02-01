using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class SequenceNumberEventLabel : MetaEventLabel
    {
        public SequenceNumberEventLabel() : base()
        {
        }

        public SequenceNumberEventLabel(SequenceNumberEvent sequenceNumberEvent) : base(sequenceNumberEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<SequenceNumberEventLabel, UxmlTraits>
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