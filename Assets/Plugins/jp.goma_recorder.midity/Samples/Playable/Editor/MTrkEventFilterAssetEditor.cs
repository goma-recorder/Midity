using Midity;
using UnityEditor;
using Midity.Editor;
using UnityEngine.UIElements;

[CustomEditor(typeof(MTrkEventFilterAsset))]
public class MTrkEventFilterAssetEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var container = new VisualElement();
        UIElementsEditorHelper.FillDefaultInspector(container, serializedObject, false);
        return container;
    }
}