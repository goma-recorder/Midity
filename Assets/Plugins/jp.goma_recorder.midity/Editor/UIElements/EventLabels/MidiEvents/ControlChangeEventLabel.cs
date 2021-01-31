﻿using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Midity.Editor
{
    public sealed class ControlChangeEventLabel : MidiEventLabel
    {
        public ControlChangeEventLabel() : base()
        {
        }

        public ControlChangeEventLabel(ControlChangeEvent controlChangeEvent) : base(controlChangeEvent)
        {
        }

        public new class UxmlFactory : UxmlFactory<ControlChangeEventLabel, UxmlTraits>
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