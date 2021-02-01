using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class OnNoteEventLabel : NoteEventLabel
    {
        public OnNoteEventLabel() : base()
        {
        }

        public OnNoteEventLabel(OnNoteEvent onNoteEvent) : base(onNoteEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<OnNoteEventLabel, UxmlTraits>
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