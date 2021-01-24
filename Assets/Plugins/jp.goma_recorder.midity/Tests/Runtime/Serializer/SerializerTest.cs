using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Midity.Tests
{
    public class SerializerTest
    {
        private const string CodeName = "utf-8";

        public class File
        {
            [Test]
            public void SerializeMidiFile()
            {
                var path = Directory.GetCurrentDirectory() +
                           "/Assets/Plugins/jp.goma_recorder.midity/Tests/Runtime/Sample.mid";
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    var midiFile1 = new MidiDeserializer(memoryStream, CodeName).Load();
                    using (memoryStream)
                    {
                        MidiSerializer.SerializeFile(midiFile1, memoryStream);
                        var midiFile2 = new MidiDeserializer(memoryStream, CodeName).Load();
                        Assert.That(midiFile1.Tracks.Count == midiFile2.Tracks.Count);

                        for (var i = 0; i < midiFile1.Tracks.Count; i++)
                        {
                            Assert.That(midiFile1.Tracks[i].Events.Count == midiFile2.Tracks[i].Events.Count);
                            for (var j = 0; j < midiFile1.Tracks[i].Events.Count; j++)
                                Assert.That(
                                    midiFile1.Tracks[i].Events[j].GetType() ==
                                    midiFile2.Tracks[i].Events[j].GetType());
                        }
                    }
                }
            }
        }

        public class Track
        {
            [Test]
            public void SerializeTrack()
            {
                var track1 = new MidiTrack(null, "Name", 94);
                MidiTrack track2;
                using (var stream = new MemoryStream())
                {
                    var encoding = Encoding.GetEncoding(CodeName);
                    MidiSerializer.SerializeTrack(track1, encoding, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var midiFIle = new MidiFile(94, encoding);
                    new MidiDeserializer(stream, encoding).ReadTrack(0, midiFIle);
                    track2 = midiFIle.Tracks[0];
                }

                Assert.That(track1.Events.Count == track2.Events.Count);
                for (var i = 0; i < track1.Events.Count; i++)
                    Assert.That(track1.Events[i].GetType() == track2.Events[i].GetType());
            }
        }

        public class Events
        {
            private static T ReDeserialize<T>(T mTrkEvent) where T : MTrkEvent
            {
                var encoding = Encoding.GetEncoding(CodeName);
                using (var stream = new MemoryStream())
                {
                    MidiSerializer.SerializeEvent(mTrkEvent, encoding, stream, 0);
                    stream.Seek(0, SeekOrigin.Begin);
                    var deserializer = new MidiDeserializer(stream, encoding);
                    byte status = 0;
                    return (T) deserializer.ReadEvent(ref status, 0);
                }
            }

            private byte GetRandomByte()
            {
                return (byte) UnityEngine.Random.Range(0, 0xff);
            }

            private (uint ticks, byte channel) GetRandomValue()
            {
                var ticks = (uint) UnityEngine.Random.Range(0, 0xffffff);
                var channel = (byte) UnityEngine.Random.Range(0, 0xf);
                return (ticks, channel);
            }

            [Test]
            public void OnNoteEvent()
            {
                var ticks = 300u;
                var channel = (byte) 2;
                var noteName = NoteName.A;
                var noteOctave = NoteOctave.Zero;
                var velocity = (byte) 5;

                var x = new OnNoteEvent(ticks, channel, noteName, noteOctave, velocity);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Channel == y.Channel);
                Assert.That(x.NoteName == y.NoteName);
                Assert.That(x.NoteOctave == y.NoteOctave);
                Assert.That(x.Velocity == y.Velocity);
            }

            [Test]
            public void OffNoteEvent()
            {
                var ticks = 300u;
                var channel = (byte) 2;
                var noteName = NoteName.A;
                var noteOctave = NoteOctave.Zero;

                var x = new OffNoteEvent(ticks, channel, noteName, noteOctave);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Channel == y.Channel);
                Assert.That(x.NoteName == y.NoteName);
                Assert.That(x.NoteOctave == y.NoteOctave);
            }

            [Test]
            public void PolyphonicKeyPressureEvent()
            {
                var ticks = 45212u;
                byte channel = 15;
                byte noteNumber = 125;
                byte pressure = 21;

                var x = new PolyphonicKeyPressureEvent(ticks, channel, noteNumber, pressure);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Channel == y.Channel);
                Assert.That(x.noteNumber == y.noteNumber);
                Assert.That(x.pressure == y.pressure);
            }

            [Test]
            public void ControlChangeEvent()
            {
                var ticks = 300u;
                byte channel = 3;

                var x = new ControlChangeEvent(ticks, channel, (Controller) GetRandomByte(), GetRandomByte());
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Status == y.Status);
                Assert.That(x.controller == y.controller);
                Assert.That(x.data == y.data);
            }

            [Test]
            public void ProgramChangeEvent()
            {
                var ticks = 6253u;
                byte channel = 7;

                var x = new ProgramChangeEvent(ticks, channel, (GeneralMidiInstrument) GetRandomByte());
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Channel == y.Channel);
                Assert.That(x.instrument == y.instrument);
            }

            [Test]
            public void ChannelPressureEvent()
            {
                var value = GetRandomValue();

                var x = new ChannelPressureEvent(value.ticks, value.channel, GetRandomByte());
                var y = ReDeserialize(x);

                Assert.That(x.pressure == y.pressure);
            }

            [Test]
            public void PitchBendEvent()
            {
                var value = GetRandomValue();

                var x = new PitchBendEvent(5, PitchWheelStep.WholeStepDown);
                var y = ReDeserialize(x);

                Assert.That(x.UpperBits == y.UpperBits);
                Assert.That(x.LowerBits == y.LowerBits);
                Assert.That(x.Position == y.Position);
            }

            [Test]
            public void SequenceNumberEvent()
            {
                var ticks = 300u;
                var number = (ushort) 6000;

                var x = new SequenceNumberEvent(ticks, number);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.number == y.number);
            }

            [Test]
            public void TextEvent()
            {
                var ticks = 3000u;
                var text = "s123あいう機";

                var x = new TextEvent(ticks, text);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.text == y.text);
            }

            [Test]
            public void CopyrightEvent()
            {
                var ticks = 2929u;
                var text = "MIT license";

                var x = new CopyrightEvent(ticks, text);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.text == y.text);
            }

            [Test]
            public void TrackNameEvent()
            {
                var ticks = 10u;
                var trackName = "piano";

                var x = new TrackNameEvent(ticks, trackName);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.name == y.name);
            }

            [Test]
            public void InstrumentNameEvent()
            {
                var ticks = 200000u;
                var name = "guitar";

                var x = new InstrumentNameEvent(ticks, name);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.name == y.name);
            }

            [Test]
            public void LyricEvent()
            {
                var ticks = 29470u;
                var lyric = "歌詞です";

                var x = new LyricEvent(ticks, lyric);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.lyric == y.lyric);
            }

            [Test]
            public void MarkerEvent()
            {
                var ticks = 20202u;
                var text = "tetext";

                var x = new MarkerEvent(ticks, text);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.text == y.text);
            }

            [Test]
            public void CuePointEvent()
            {
                var ticks = 20202u;
                var text = "queue";

                var x = new CuePointEvent(ticks, text);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.text == y.text);
            }

            [Test]
            public void ProgramNameEvent()
            {
                var value = GetRandomValue();
                var name = "nname";

                var x = new ProgramNameEvent(value.ticks, name);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.name == y.name);
            }

            [Test]
            public void DeviceNameEvent()
            {
                var value = GetRandomValue();
                var name = "nname";

                var x = new DeviceNameEvent(value.ticks, name);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.name == y.name);
            }

            [Test]
            public void ChannelPrefixEvent()
            {
                var ticks = 200836u;
                byte data = 128;

                var x = new ChannelPrefixEvent(ticks, data);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.data == y.data);
            }

            [Test]
            public void PortNumberEvent()
            {
                var value = GetRandomValue();

                var x = new PortNumberEvent(value.ticks, 129);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.Number == y.Number);
            }

            [Test]
            public void EndOfTrackEvent()
            {
                var ticks = 18246u;

                var x = new EndOfTrackEvent(ticks);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
            }

            [Test]
            public void TempoEvent()
            {
                var ticks = 9843758u;
                uint tempo = 0xff_ff_ff;
                var x = new TempoEvent(ticks, tempo);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.TickTempo == y.TickTempo);
            }

            [Test]
            public void SmpteOffsetEvent()
            {
                var ticks = 6276540u;
                byte hr = 223;
                byte mn = 145;
                byte se = 126;
                byte fr = 36;
                byte ff = 255;

                var x = new SmpteOffsetEvent(ticks, hr, mn, se, fr, ff);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.hr == y.hr);
                Assert.That(x.mn == y.mn);
                Assert.That(x.se == y.se);
                Assert.That(x.fr == y.fr);
                Assert.That(x.ff == y.ff);
            }

            [Test]
            public void TimeSignatureEvent()
            {
                var ticks = 19864u;
                byte nn = 0;
                byte dd = 0;
                byte cc = 0;
                byte bb = 0;

                var x = new TimeSignatureEvent(ticks, nn, dd, cc, bb);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.numerator == y.numerator);
                Assert.That(x.denominator == y.denominator);
                Assert.That(x.midiClocksPerClick == y.midiClocksPerClick);
                Assert.That(x.numberOfNotated32nds == y.numberOfNotated32nds);
            }

            [Test]
            public void KeyEvent()
            {
                var ticks = 2534u;
                var key = NoteKey.AFlatMinor;

                var x = new KeyEvent(ticks, key);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.noteKey == y.noteKey);
            }

            [Test]
            public void SequencerUniqueEvent()
            {
                var ticks = 275684u;
                var data = new byte[] {0, 1, 2, 3, 4, 5, 6};

                var x = new SequencerUniqueEvent(ticks, data);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.data.SequenceEqual(y.data));
            }

            [Test]
            public void UnknownMetaEvent()
            {
                var ticks = 176846u;
                byte eventNumber = 0xf1;
                var data = new byte[] {0xf1, 1, 2, 3, 4, 5, 6};

                var x = new UnknownMetaEvent(ticks, eventNumber, data);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.metaId == y.metaId);
                Assert.That(x.data.SequenceEqual(y.data));
            }

            [Test]
            public void SysExEvent()
            {
                var ticks = 25463u;
                var data = new byte[] {255, 32, 7, 65, 35, 42};

                var x = new SysExEvent(ticks, data);
                var y = ReDeserialize(x);

                Assert.That(x.Ticks == y.Ticks);
                Assert.That(x.data.SequenceEqual(y.data));
            }
        }
    }
}