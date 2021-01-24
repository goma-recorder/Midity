using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Label = UnityEngine.UIElements.Label;

namespace Midity.Editor
{
    public class MTrkEventListWindoww : EditorWindow
    {
        [MenuItem("Window/Midity/MTrkEventListWindow")]
        public static void ShowExample()
        {
            var wnd = GetWindow<MTrkEventListWindoww>();
            wnd.titleContent = new GUIContent("MTrkEventList");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;

            // Import UXML
            var listWindow = Resources.Load<VisualTreeAsset>("MTrkEventList").Instantiate();
            root.Add(listWindow);

            var eventList = listWindow.Q<ScrollView>("EventList");
            var midiFileField = listWindow.Q<ObjectField>("TargetMidiFile");

            PopupField<string> trackNamePopUp = null;
            midiFileField.RegisterValueChangedCallback(changeEvent =>
            {
                if (changeEvent.newValue == changeEvent.previousValue) return;
                eventList.contentContainer.Clear();
                if (changeEvent.newValue is null) return;
                if (!(trackNamePopUp is null)) root.Remove(trackNamePopUp);

                var midiFile = ((MidiFileAsset) changeEvent.newValue).MidiFile;

                var trackNameList = new List<string>();
                for (var i = 0; i < midiFile.Tracks.Count; i++)
                    trackNameList.Add($"{i}:{midiFile.Tracks[i].Name ?? "No Name"}");

                trackNamePopUp = new PopupField<string>(
                    "Track Name",
                    trackNameList,
                    0,
                    trackName =>
                    {
                        eventList.contentContainer.Clear();
                        var index = trackNameList.IndexOf(trackName);
                        foreach (var mTrkEvent in midiFile.Tracks[index].Events)
                            eventList.contentContainer.Add(MTrkEventLabel.Instantiate(mTrkEvent));
                        return trackName;
                    });
                listWindow.Insert(2, trackNamePopUp);
            });

            // var objectField = labelFromUXML.GetFirstOfType<MidiFIleAssetField>();
            // // Debug.Log(objectField.label);
            // objectField.RegisterValueChangedCallback(c =>
            // {
            //     if (c.newValue is null) return;
            //     var midiFile = ((MidiFileAsset) c.newValue).MidiFile;
            //     var scrollView = new ScrollView();
            //     root.Add(scrollView);
            //     foreach (var mTrkEvent in midiFile.Tracks[1].Events)
            //     {
            //        var mTrkEventLabel = MTrkEventLabel.Instantiate(mTrkEvent);
            //        scrollView.Add(mTrkEventLabel);
            //     }
            // });
            // root.Add(objectField);
        }

        // void Intialize(VisualElement m_MyScriptableObjectDisplayContainer)
        // {
        //     var m_MyScriptableObjectField = new ObjectField("MyScriptableObject Object");
        //     m_MyScriptableObjectField.objectType = typeof(MidiFileAsset);
        //     m_MyScriptableObjectField.RegisterValueChangedCallback(c =>
        //     {
        //         m_MyScriptableObjectDisplayContainer.Clear();
        //         m_MyScriptableObjectDisplayContainer.Add(MyScriptableObjectDisplay((MidiFileAsset) c.newValue));
        //     });
        //
        //     m_MyScriptableObjectDisplayContainer.Add(m_MyScriptableObjectField);
        //     // m_MyScriptableObjectDisplayContainer.Add(m_MyScriptableObjectDisplayContainer);
        // }
        //
        //
        // VisualElement MyScriptableObjectDisplay(MidiFileAsset MyScriptableObject)
        // {
        //     var container = new VisualElement();
        //
        //     container.Add(new Label("MyScriptableObject Object settings"));
        //     if (MyScriptableObject == null)
        //     {
        //         container.Add(new Label("please set a MyScriptableObject object"));
        //     }
        //     else
        //     {
        //         container.Add(CreateUIElementInspector(MyScriptableObject, null));
        //     }
        //
        //     return container;
        // }
        //
        // public static VisualElement CreateUIElementInspector(UnityEngine.Object target,
        //     List<string> propertiesToExclude)
        // {
        //     var container = new VisualElement();
        //
        //     var serializedObject = new SerializedObject(target);
        //
        //     var fields = GetVisibleSerializedFields(target.GetType());
        //
        //     for (int i = 0; i < fields.Length; ++i)
        //     {
        //         var field = fields[i];
        //         // Do not draw HideInInspector fields or excluded properties.
        //         if (propertiesToExclude != null && propertiesToExclude.Contains(field.Name))
        //         {
        //             continue;
        //         }
        //
        //         var serializedProperty = serializedObject.FindProperty(field.Name);
        //
        //         var propertyField = new PropertyField(serializedProperty);
        //
        //         container.Add(propertyField);
        //     }
        //
        //     container.Bind(serializedObject);
        //
        //
        //     return container;
        // }
        //
        // public static FieldInfo[] GetVisibleSerializedFields(Type T)
        // {
        //     List<FieldInfo> infoFields = new List<FieldInfo>();
        //
        //     var publicFields = T.GetFields(BindingFlags.Instance | BindingFlags.Public);
        //     for (int i = 0; i < publicFields.Length; i++)
        //     {
        //         if (publicFields[i].GetCustomAttribute<HideInInspector>() == null)
        //         {
        //             infoFields.Add(publicFields[i]);
        //         }
        //     }
        //
        //     var privateFields = T.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        //     for (int i = 0; i < privateFields.Length; i++)
        //     {
        //         if (privateFields[i].GetCustomAttribute<SerializeField>() != null)
        //         {
        //             infoFields.Add(privateFields[i]);
        //         }
        //     }
        //
        //     return infoFields.ToArray();
        // }
    }
}