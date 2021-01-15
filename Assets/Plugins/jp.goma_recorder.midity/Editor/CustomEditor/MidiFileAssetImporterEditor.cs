using System.Text;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace Midity.Playable.Editor
{
    [CustomEditor(typeof(MidiFileAssetImporter))]
    internal sealed class MidiFileAssetImporterEditor : ScriptedImporterEditor
    {
        private string _lastCodeName;
        private SerializedProperty _codeName;

        public override bool showImportedObject => false;

        public override void OnEnable()
        {
            base.OnEnable();
            _codeName = serializedObject.FindProperty("_characterCodeName");
            _lastCodeName = _codeName.stringValue;
        }

        public override void OnInspectorGUI()
        {
            var assetImporter = (MidiFileAssetImporter) target;
            EditorGUILayout.LabelField("Format", assetImporter._midiFileAsset.MidiFile?.format.ToString());
            EditorGUILayout.LabelField("Delta Time", assetImporter._midiFileAsset.MidiFile?.DeltaTime.ToString());

            serializedObject.Update();
            EditorGUILayout.PropertyField(_codeName);
            try
            {
                Encoding.GetEncoding(_codeName.stringValue);
                _lastCodeName = _codeName.stringValue;
            }
            catch
            {
                _codeName.stringValue = _lastCodeName;
            }

            serializedObject.ApplyModifiedProperties();

            ApplyRevertGUI();
        }
    }
}