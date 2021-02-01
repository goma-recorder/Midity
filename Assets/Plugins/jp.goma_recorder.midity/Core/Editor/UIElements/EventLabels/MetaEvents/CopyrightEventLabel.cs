using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class CopyrightEventLabel : MetaEventLabel
    {
        public CopyrightEventLabel() : base()
        {
        }

        public CopyrightEventLabel(CopyrightEvent copyrightEvent) : base(copyrightEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<CopyrightEventLabel, UxmlTraits>
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