using System.Linq;
using UnityEngine;

namespace Midity
{
    public static class MidiTrackBackgroundExtension
    {
        public static Texture2D WriteNoteBarTexture2D(this MidiTrack midiTrack, int noteWidthRate,
            int topMargin, int bottomMargin, bool ignoreOctave = false)
        {
            var noteTable = midiTrack.NoteEventPairs;
            if (noteTable.Count == 0)
                return new Texture2D(1, 1);

            var minNoteNumber = (byte) 0;
            var noteNumberRange = 12;
            if (!ignoreOctave)
            {
                minNoteNumber = noteTable.Min(x => x.NoteNumber);
                noteNumberRange = noteTable.Max(x => x.NoteNumber) - minNoteNumber;
            }

            var texture = new Texture2D((int) midiTrack.AllTicks / noteWidthRate,
                bottomMargin + noteNumberRange + topMargin, TextureFormat.RGBA32, false, true)
            {
                filterMode = FilterMode.Point
            };

            var data = texture.GetRawTextureData<Color32>();
            foreach (var pair in noteTable)
                for (var x = (int) (pair.OnTick / noteWidthRate);
                    x < pair.OffTick / noteWidthRate;
                    x++)
                {
                    var y = (ignoreOctave ? (byte) pair.NoteName : pair.NoteNumber - minNoteNumber) + bottomMargin;
                    data[x + y * texture.width] = Color.HSVToRGB(pair.NoteNumber % 12 / 12f, 0.85f, 0.87f);
                }

            texture.Apply();
            return texture;
        }
    }
}