using Midity;
using NUnit.Framework;
using System.IO;

namespace Tests
{
    public class MidiTrackTest
    {
        private const string CodeName = "utf-8";

        private static MidiFile LoadMidiFile()
        {
            var path = Directory.GetCurrentDirectory() +
                       "/Assets/Plugins/jp.goma_recorder.midity/Tests/Runtime/Sample.mid";
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return new MidiDeserializer(fileStream, CodeName).Load();
            }
        }

        [Test]
        public void GetEventAtTick()
        {
            var midiFile = LoadMidiFile();
            var e = midiFile.Tracks[0].GetEventAtTick(0);
            Assert.That(midiFile.Tracks[0].Events[0] == e);
        }

        [Test]
        public void GetEventAfterTick()
        {
            var midiFile = LoadMidiFile();
            var e = midiFile.Tracks[0].GetEventAfterTick(1);
            Assert.That(e.Ticks >= 1);
        }

        [Test]
        public void GetEventBeforeTick()
        {
            var midiFile = LoadMidiFile();
            var e = midiFile.Tracks[0].GetEventBeforeTick(1);
            Assert.That(e.Ticks <= 1);
            Assert.That(e == midiFile.Tracks[0].Events[0]);
        }
    }
}