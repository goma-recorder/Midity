using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class OffNoteEventLabel : NoteEventLabel
    {
        public OffNoteEventLabel() : base()
        {
        }

        public OffNoteEventLabel(OffNoteEvent offNoteEvent) : base(offNoteEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<OffNoteEventLabel, UxmlTraits>
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