using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Midity.Editor
{
    [CustomPropertyDrawer(typeof(NoteEventFilter))]
    public class NoteFilterDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var serializedObject = property.serializedObject;
            var root = Resources.Load<VisualTreeAsset>("Filters/NoteFilter").Instantiate();

            #region Note name filter

            var noteNameFilterValueTable = property.FindPropertyRelative("_noteNameFilterValueTable");
            var r = noteNameFilterValueTable.GetArrayElementAtIndex(0);

            #endregion

            #region Note number renge filter

            var rangeMinParent = root.Q("min");
            var minIntField = rangeMinParent.Q<IntegerField>();
            var minNoteNameField = rangeMinParent.Q<EnumField>(null, "note-name-field");
            var minNoteOctaveField = rangeMinParent.Q<EnumField>(null, "note-octave-field");
            var minNoteProperty = property.FindPropertyRelative("_minNoteNumber");

            var rangeMaxParent = root.Q("max");
            var maxIntField = rangeMaxParent.Q<IntegerField>();
            var maxNoteNameField = rangeMaxParent.Q<EnumField>(null, "note-name-field");
            var maxNoteOctaveField = rangeMaxParent.Q<EnumField>(null, "note-octave-field");
            var maxNoteProperty = property.FindPropertyRelative("_maxNoteNumber");

            var noteRangeSlider = root.Q<MinMaxSlider>();

            var settingMin = false;
            minIntField.RegisterValueChangedCallback(x => SetNoteRangeMin((byte) x.newValue));
            minNoteNameField.RegisterValueChangedCallback(x =>
            {
                var noteNumber = ((NoteName) x.newValue, (NoteOctave) minNoteOctaveField.value).ToNoteNumber();
                SetNoteRangeMin(noteNumber);
            });
            minNoteOctaveField.RegisterValueChangedCallback(x =>
            {
                var noteNumber = ((NoteName) minNoteNameField.value, (NoteOctave) x.newValue).ToNoteNumber();
                SetNoteRangeMin(noteNumber);
            });

            var settingMax = false;
            maxIntField.RegisterValueChangedCallback(x => SetNoteRangeMax((byte) x.newValue));
            maxNoteNameField.RegisterValueChangedCallback(x =>
            {
                var noteNumber = ((NoteName) x.newValue, (NoteOctave) maxNoteOctaveField.value).ToNoteNumber();
                SetNoteRangeMax(noteNumber);
            });
            maxNoteOctaveField.RegisterValueChangedCallback(x =>
            {
                var noteNumber = ((NoteName) maxNoteNameField.value, (NoteOctave) x.newValue).ToNoteNumber();
                SetNoteRangeMax(noteNumber);
            });

            noteRangeSlider.RegisterValueChangedCallback(changeEvent =>
            {
                if (changeEvent.previousValue.x != changeEvent.newValue.x)
                    SetNoteRangeMin((byte) changeEvent.newValue.x);
                else if (changeEvent.previousValue.y != changeEvent.newValue.y)
                    SetNoteRangeMax((byte) changeEvent.newValue.y);
            });

            SetNoteRangeMin((byte) minNoteProperty.intValue);
            SetNoteRangeMax((byte) maxNoteProperty.intValue);

            void SetNoteRangeMin(byte value)
            {
                if (settingMin) return;
                settingMin = true;
                value = Math.Min(Math.Max((byte) 0, value), (byte) maxNoteProperty.intValue);
                minIntField.value = value;
                minNoteNameField.value = NoteEnumUtil.ToNoteName(value);
                minNoteOctaveField.value = NoteEnumUtil.ToNoteOctave(value);
                noteRangeSlider.minValue = value;
                minNoteProperty.intValue = value;
                serializedObject.ApplyModifiedProperties();
                settingMin = false;
            }

            void SetNoteRangeMax(byte value)
            {
                if (settingMax) return;
                settingMax = true;
                value = Math.Min(Math.Max((byte) minNoteProperty.intValue, value), (byte) 131);
                maxIntField.value = value;
                maxNoteNameField.value = NoteEnumUtil.ToNoteName(value);
                maxNoteOctaveField.value = NoteEnumUtil.ToNoteOctave(value);
                noteRangeSlider.maxValue = value;
                maxNoteProperty.intValue = value;
                serializedObject.ApplyModifiedProperties();
                settingMax = false;
            }

            #endregion

            return root;
        }
    }
}