using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class InventoryIconMaker : EditorWindow {

    public Object Target;
    public Texture2D DisplayPicture;

	[MenuItem("Window/InventoryIconMaker")]
    static void Open()
    {
        EditorWindow.GetWindow<InventoryIconMaker>();
    }

    public void OnGUI()
    {
        EditorGUILayout.TextArea("Pick a prefab. Hit \"Refresh\" to refresh this prefab's inventory icon. Hit \"Save\" to save the texture as png.");
        this.Target = EditorGUILayout.ObjectField("Target", this.Target, typeof(Object), false) as Object;
        if (GUILayout.Button("Refresh"))
        {
            this.RefreshTarget();
        }
    }

    public void RefreshTarget()
    {
        if (this.Target == null)
        {
            return;
        }

        byte[] DisplayPictureData = AssetPreview.GetAssetPreview(this.Target.gameObject).EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Resources/Textures/Inventory/" + this.Target.name + "_inventory.png", DisplayPictureData);
    }
}
