using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private List<InventoryObject> objects;

    public void Start()
    {
        this.objects = new List<InventoryObject>();
        this.objects.Add(new InventoryPanel(0));
        this.objects.Add(new InventoryPanel(1));
        this.objects.Add(new InventoryPanel(2));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(0));
        this.objects.Add(new InventoryPanel(1));
        this.objects.Add(new InventoryPanel(2));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(0));
        this.objects.Add(new InventoryPanel(1));
        this.objects.Add(new InventoryPanel(2));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryItem(new byte[] { 0, 0, 0, 1 }));
    }

    public void OnGUI()
    {
        if (Player.Instance.GMode == GameMode.Inventory)
        {
            GUILayout.BeginArea(Rect.MinMaxRect(0.25f * Screen.width, 0.25f * Screen.height, 0.75f * Screen.width, 0.75f * Screen.height));
            for (int i = 0; i < this.objects.Count; i += 6)
            {
                GUILayout.BeginHorizontal();
                for (int j = i; (j < this.objects.Count) && (j < i + 6); j++)
                {
                    InventoryObject o = this.objects[j];
                    if (GUILayout.Button(o.DisplayPicture, GUILayout.Width(64), GUILayout.Height(64)))
                    {
                        o.OnSelectedItem();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
