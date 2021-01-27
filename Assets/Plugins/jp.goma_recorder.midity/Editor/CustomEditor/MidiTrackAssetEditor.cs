using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Midity.Playable.Editor
{
    // Custom inspector for MIDI assets
    // There is no editable property; It just shows some infomation.
    [CustomEditor(typeof(MidiTrackAsset))]
    internal class MidiTrackAssetEditor : UnityEditor.Editor
    {
        private string _ccText;
        private string _durationText;
        private string _noteText;

        private void OnEnable()
        {
            var asset = (MidiTrackAsset) target;

            var track = asset.MidiTrack;

            var note = new HashSet<(byte number, NoteOctave octave, NoteName name)>();
            var cc = new HashSet<int>();

            foreach (var e in track.Events)
                switch (e)
                {
                    case OnNoteEvent onNoteEvent:
                        note.Add((onNoteEvent.NoteNumber, onNoteEvent.NoteOctave, onNoteEvent.NoteName));
                        break;
                    case ControlChangeEvent controlChangeEvent:
                        cc.Add((int) controlChangeEvent.controller);
                        break;
                }

            if (note.Count == 0)
            {
                _noteText = "-";
            }
            else
            {
                var sorted = note
                    .OrderBy(x => x.number)
                    .Select(x => $"{x.octave} {x.name}");
                _noteText = string.Join(",", sorted);
            }

            _ccText = cc.Count == 0 ? "-" : string.Join(", ", cc.OrderBy(x => x));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Duration", _durationText);
            EditorGUILayout.LabelField("Contained Events");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Note", _noteText);
            EditorGUILayout.LabelField("CC", _ccText);
            EditorGUI.indentLevel--;
        }

        private Texture2D _texture2D;

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            if (_texture2D is null)
            {
                var midiTrack = ((MidiTrackAsset) target).MidiTrack;
                const int topMargin = 2;
                const int bottomMargin = 1;
                _texture2D = midiTrack.WriteNoteBarTexture2D((int) midiTrack.DeltaTime / 8, topMargin,
                    bottomMargin, true);
            }

            return _texture2D;
        }
    }
}