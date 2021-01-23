using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class ProgramNameEventLabel : MetaEventLabel
    {
        public ProgramNameEventLabel() : base()
        {
        }

        public ProgramNameEventLabel(ProgramNameEvent programNameEvent) : base(programNameEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<ProgramNameEventLabel, UxmlTraits>
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