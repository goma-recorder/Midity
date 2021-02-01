using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class KeyEventLabel : MetaEventLabel
    {
        public KeyEventLabel() : base()
        {
        }

        public KeyEventLabel(KeyEvent keyEvent) : base(keyEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<KeyEventLabel, UxmlTraits>
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