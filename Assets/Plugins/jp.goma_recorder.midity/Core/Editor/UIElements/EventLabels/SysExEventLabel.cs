using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class SysExEventLabel : MTrkEventLabel
    {
        public SysExEventLabel() : base()
        {
        }

        public SysExEventLabel(SysExEvent sysExEvent) : base(sysExEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<SysExEventLabel, UxmlTraits>
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