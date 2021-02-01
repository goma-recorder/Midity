using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class ProgramChangeEventLabel : MidiEventLabel
    {
        public ProgramChangeEventLabel() : base()
        {
        }

        public ProgramChangeEventLabel(ProgramChangeEvent programChangeEvent) : base(programChangeEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<ProgramChangeEventLabel, UxmlTraits>
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