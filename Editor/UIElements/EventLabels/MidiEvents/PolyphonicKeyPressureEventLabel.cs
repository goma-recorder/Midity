using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class PolyphonicKeyPressureEventLabel : MidiEventLabel
    {
        public PolyphonicKeyPressureEventLabel() : base()
        {
        }

        public PolyphonicKeyPressureEventLabel(PolyphonicKeyPressureEvent polyphonicKeyPressureEvent) : base(polyphonicKeyPressureEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<PolyphonicKeyPressureEventLabel, UxmlTraits>
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