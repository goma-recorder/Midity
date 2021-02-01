using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class PitchBendEventLabel : MidiEventLabel
    {
        public PitchBendEventLabel() : base()
        {
        }

        public PitchBendEventLabel(PitchBendEvent pitchBendEvent) : base(pitchBendEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<PitchBendEventLabel, UxmlTraits>
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