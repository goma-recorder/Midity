using UnityEngine.UIElements;

namespace Midity.Editor
{
    public abstract class MidiEventLabel : MTrkEventLabel
    {
        public MidiEventLabel() : base()
        {
        }

        public MidiEventLabel(MidiEvent midiEvent) : base(midiEvent)
        {
            AddParameterElement(nameof(midiEvent.Channel), new Label(midiEvent.Channel.ToString()));
        }
    }
}