using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Midity.Editor
{
    public abstract class MTrkEventLabel : VisualElement
    {
        public MTrkEventLabel()
        {
        }

        public MTrkEventLabel(MTrkEvent mTrkEvent)
        {
            var eventName = mTrkEvent.GetType().Name;
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

            // Remove Event
            eventName = eventName.Remove(eventName.Length - 5);
            var eventNameLabel = new Label(eventName);
            eventNameLabel.style.width = 100;
            Add(eventNameLabel);
            AddParameterElement("Ticks", mTrkEvent.Ticks.ToString(), 100);
            // AddParameterElement("Ticks", (int) mTrkEvent.Ticks, 100, x => Debug.Log(x.newValue));
        }

        public static MTrkEventLabel Instantiate<T>(T mTrkEvent) where T : MTrkEvent
        {
            switch (mTrkEvent)
            {
                case OnNoteEvent onNoteEvent:
                    return new OnNoteEventLabel(onNoteEvent);
                case OffNoteEvent offNoteEvent:
                    return new OffNoteEventLabel(offNoteEvent);
                case PolyphonicKeyPressureEvent polyphonicKeyPressureEvent:
                    return new PolyphonicKeyPressureEventLabel(polyphonicKeyPressureEvent);
                case ControlChangeEvent controlChangeEvent:
                    return new ControlChangeEventLabel(controlChangeEvent);
                case ProgramChangeEvent programChangeEvent:
                    return new ProgramChangeEventLabel(programChangeEvent);
                case ChannelPressureEvent channelPressureEvent:
                    return new ChannelPressureEventLabel(channelPressureEvent);
                case PitchBendEvent pitchBendEvent:
                    return new PitchBendEventLabel(pitchBendEvent);
                case SequenceNumberEvent sequenceNumberEvent:
                    return new SequenceNumberEventLabel(sequenceNumberEvent);
                case TextEvent textEvent:
                    return new TextEventLabel(textEvent);
                case CopyrightEvent copyrightEvent:
                    return new CopyrightEventLabel(copyrightEvent);
                case TrackNameEvent trackNameEvent:
                    return new TrackNameEventLabel(trackNameEvent);
                case InstrumentNameEvent instrumentNameEvent:
                    return new InstrumentNameEventLabel(instrumentNameEvent);
                case LyricEvent lyricEvent:
                    return new LyricEventLabel(lyricEvent);
                case MarkerEvent markerEvent:
                    return new MarkerEventLabel(markerEvent);
                case QueueEvent queueEvent:
                    return new QueueEventLabel(queueEvent);
                case ProgramNameEvent programNameEvent:
                    return new ProgramNameEventLabel(programNameEvent);
                case DeviceNameEvent deviceNameEvent:
                    return new DeviceNameEventLabel(deviceNameEvent);
                case ChannelPrefixEvent channelPrefixEvent:
                    return new ChannelPrefixEventLabel(channelPrefixEvent);
                case PortNumberEvent portNumberEvent:
                    return new PortNumberEventLabel(portNumberEvent);
                case EndOfTrackEvent endOfTrackEvent:
                    return new EndOfTrackEventLabel(endOfTrackEvent);
                case TempoEvent tempoEvent:
                    return new TempoEventLabel(tempoEvent);
                case SmpteOffsetEvent smpteOffsetEvent:
                    return new SmpteOffsetEventLabel(smpteOffsetEvent);
                case BeatEvent beatEvent:
                    return new BeatEventLabel(beatEvent);
                case KeyEvent keyEvent:
                    return new KeyEventLabel(keyEvent);
                case SequencerUniqueEvent sequencerUniqueEvent:
                    return new SequencerUniqueEventLabel(sequencerUniqueEvent);
                case UnknownMetaEvent unknownEvent:
                    return new UnknownMetaEventLabel(unknownEvent);
                case SysExEvent sysExEvent:
                    return new SysExEventLabel(sysExEvent);
                default:
                    return null;
            }
        }

        protected VisualElement AddParameterElement(string text,
            VisualElement parameter, int? parameterWidth = null)
        {
            var label = new Label(text + ':');
            if (!(parameterWidth is null))
                parameter.style.width = (int) parameterWidth;
            var visualElement = new VisualElement();
            visualElement.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            visualElement.Add(label);
            visualElement.Add(parameter);
            Add(visualElement);
            return visualElement;
        }

        protected VisualElement AddParameterElement(string text, string parameter, int? parameterWidth = null)
        {
            return AddParameterElement(text, new Label(parameter), parameterWidth);
        }

        protected VisualElement AddParameterElement(string text, int parameter, int? parameterWidth = null,
            EventCallback<ChangeEvent<int>> callback = null)
        {
            var integerField = new IntegerField(text) {value = parameter};
            Add(integerField);
            integerField.labelElement.style.minWidth = new StyleLength(StyleKeyword.Auto);
            if (!(parameterWidth is null))
                integerField.contentContainer.style.width = (int) parameterWidth;
            integerField.RegisterValueChangedCallback(callback);
            return integerField;
        }
    }
}