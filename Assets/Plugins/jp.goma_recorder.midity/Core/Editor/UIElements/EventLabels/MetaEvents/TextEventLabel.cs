using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class TextEventLabel : MetaEventLabel
    {
        public TextEventLabel() : base()
        {
        }

        public TextEventLabel(TextEvent textEvent) : base(textEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<TextEventLabel, UxmlTraits>
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