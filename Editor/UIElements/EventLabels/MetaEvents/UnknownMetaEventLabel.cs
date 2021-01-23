using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class UnknownMetaEventLabel : MetaEventLabel
    {
        public UnknownMetaEventLabel() : base()
        {
        }

        public UnknownMetaEventLabel(UnknownMetaEvent unknownMetaEvent) : base(unknownMetaEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<UnknownMetaEventLabel, UxmlTraits>
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