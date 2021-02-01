using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class SequencerUniqueEventLabel : MetaEventLabel
    {
        public SequencerUniqueEventLabel() : base()
        {
        }

        public SequencerUniqueEventLabel(SequencerUniqueEvent sequencerUniqueEvent) : base(sequencerUniqueEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<SequencerUniqueEventLabel, UxmlTraits>
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