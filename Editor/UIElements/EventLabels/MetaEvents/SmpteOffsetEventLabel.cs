using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class SmpteOffsetEventLabel : MetaEventLabel
    {
        public SmpteOffsetEventLabel() : base()
        {
        }

        public SmpteOffsetEventLabel(SmpteOffsetEvent smpteOffsetEvent) : base(smpteOffsetEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<SmpteOffsetEventLabel, UxmlTraits>
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