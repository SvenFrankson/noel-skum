using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour
{
    private List<InventoryObject> objects;
    private List<InventoryObject> Objects
    {
        get
        {
            if (this.objects == null)
            {
                this.objects = new List<InventoryObject>();
            }
            return this.objects;
        }
    }

    public void Add(InventoryObject target)
    {
        this.Objects.Add(target);
    }

    public void Remove(InventoryObject target)
    {
        if (this.Objects.Contains(target))
        {
            this.Objects.Remove(target);
        }
    }

    public void OnGUI()
    {
        if (Player.Instance.GMode == GameMode.Container)
        {
            if (Inventory.Instance.TargetContainer == this)
            {
                GUILayout.BeginArea(Rect.MinMaxRect(0.05f * Screen.width, 0.25f * Screen.height, 0.45f * Screen.width, 0.75f * Screen.height));
                for (int i = 0; i < this.Objects.Count; i += 6)
                {
                    GUILayout.BeginHorizontal();
                    for (int j = i; (j < this.Objects.Count) && (j < i + 6); j++)
                    {
                        InventoryObject o = this.Objects[j];
                        if (GUILayout.Button(o.DisplayPicture, GUILayout.Width(64), GUILayout.Height(64)))
                        {
                            OnInventoryClick(o);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndArea();
            }
        }
    }

    public void OnInventoryClick(InventoryObject o)
    {
        this.Remove(o);
        Inventory.Instance.Add(o);
    }

    public byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add((byte)this.Objects.Count);
        foreach (InventoryObject o in this.Objects)
        {
            Debug.Log("Saving content into Container");
            save.AddRange(o.Reference);
        }
        return save.ToArray();
    }
}
