using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class QueueEventLabel : MetaEventLabel
    {
        public QueueEventLabel() : base()
        {
        }

        public QueueEventLabel(QueueEvent queueEvent) : base(queueEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<QueueEventLabel, UxmlTraits>
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