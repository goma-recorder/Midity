using System.IO;
using UnityEngine;

namespace Midity
{
    // ScriptableObject class used for storing a MIDI file asset
    public sealed class MidiFileAsset : ScriptableObject
    {
        [SerializeField] private Format format;
        [SerializeField] private uint deltaTime;
        [SerializeField] private int codePage;
        [SerializeField] private int trackCount;
        [SerializeField] private byte[] trackBytes;

        private MidiFile _midiFile;

        public MidiFile MidiFile
        {
            get
            {
                if (_midiFile != null) return _midiFile;

                _midiFile = new MidiFile(deltaTime, codePage, format);
                using (var stream = new MemoryStream(trackBytes))
                {
                    var deserializer = new MidiDeserializer(stream, codePage);
                    for (var i = 0; i < trackCount; i++)
                        deserializer.ReadTrack(i, _midiFile);
                }

                return _midiFile;
            }
        }

        public static MidiFileAsset Instantiate(MidiFile midiFile, string assetName, byte[] trackBytes)
        {
            var fileAsset = CreateInstance<MidiFileAsset>();
            fileAsset.name = assetName;
            fileAsset.format = midiFile.format;
            fileAsset.deltaTime = midiFile.DeltaTime;
            fileAsset.codePage = midiFile.encoding.CodePage;
            fileAsset.trackCount = midiFile.Tracks.Count;
            fileAsset.trackBytes = trackBytes;
            fileAsset._midiFile = midiFile;
            return fileAsset;
        }
    }
}