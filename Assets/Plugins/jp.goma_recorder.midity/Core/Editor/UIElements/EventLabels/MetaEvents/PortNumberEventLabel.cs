using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class PortNumberEventLabel : MetaEventLabel
    {
        public PortNumberEventLabel() : base()
        {
        }

        public PortNumberEventLabel(PortNumberEvent portNumberEvent) : base(portNumberEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<PortNumberEventLabel, UxmlTraits>
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