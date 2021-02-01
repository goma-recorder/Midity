using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public abstract class NoteEventLabel : MidiEventLabel
    {
        public NoteEventLabel() : base()
        {
        }

        public NoteEventLabel(NoteEvent noteEvent) : base(noteEvent)
        {
            AddParameterElement(nameof(noteEvent.NoteName), new Label(noteEvent.NoteName.ToString()));
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