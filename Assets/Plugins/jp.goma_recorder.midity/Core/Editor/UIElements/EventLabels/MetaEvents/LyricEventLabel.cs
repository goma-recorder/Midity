using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class LyricEventLabel : MetaEventLabel
    {
        public LyricEventLabel() : base()
        {
        }

        public LyricEventLabel(LyricEvent lyricEvent) : base(lyricEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<LyricEventLabel, UxmlTraits>
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