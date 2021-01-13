using System.IO;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace Midity.Playable.Editor
{
    // Custom importer for .mid files
    [ScriptedImporter(1, "mid")]
    internal sealed class MidiFileAssetImporter : ScriptedImporter
    {
        [SerializeField] private string _characterCodeName = "utf-8";
        [SerializeField] internal MidiFileAsset _midiFileAsset;

        public override void OnImportAsset(AssetImportContext context)
        {
            var assetName = Path.GetFileNameWithoutExtension(assetPath);

            MidiFile midiFile;
            byte[] trackBytes;
            // Main MIDI file asset
            using (var stream = new FileStream(context.assetPath, FileMode.Open, FileAccess.Read))
            {
                var deserializer = new MidiDeserializer(stream, _characterCodeName);
                (midiFile, trackBytes) = deserializer.LoadTrackBytes();
            }

            var fileAsset = MidiFileAsset.Instantiate(midiFile, assetName, trackBytes);
            fileAsset.name = assetName;
            context.AddObjectToAsset("MidiFileAsset", fileAsset);
            context.SetMainObject(fileAsset);

            var trackCount = midiFile.Tracks.Count;
            var trackAssets = new MidiTrackAsset[trackCount];
            for (var i = 0; i < trackCount; i++)
            {
                trackAssets[i] = MidiTrackAsset.Instantiate(fileAsset, i);
                trackAssets[i].name = $"{i}:{midiFile.Tracks[i].Name}";
            }

            // Contained tracks
            foreach (var track in trackAssets)
                context.AddObjectToAsset(track.name, track);
            _midiFileAsset = fileAsset;
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}