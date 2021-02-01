using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Midity.Editor
{
    public static class UIElementsEditorHelper
    {
        public static void FillDefaultInspector(VisualElement container, SerializedObject serializedObject,
            bool hideScript)
        {
            var property = serializedObject.GetIterator();
            if (property.NextVisible(true)) // Expand first child.
                do
                {
                    if (property.propertyPath == "m_Script" && hideScript) continue;

                    var field = new PropertyField(property);
                    field.name = "PropertyField:" + property.propertyPath;


                    if (property.propertyPath == "m_Script" && serializedObject.targetObject != null)
                        field.SetEnabled(false);

                    container.Add(field);
                } while (property.NextVisible(false));
        }
    }
}