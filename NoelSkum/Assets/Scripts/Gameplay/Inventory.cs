using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
            }
            return instance;
        }
    }
    private List<InventoryObject> objects;

    private Container targetContainer;
    public Container TargetContainer
    {
        get
        {
            return this.targetContainer;
        }
        set
        {
            this.targetContainer = value;
        }
    }

    public void Start()
    {
        this.objects = new List<InventoryObject>();
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 2 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 2 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 2 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 2 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 0 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 1 }));
        this.objects.Add(new InventoryPanel(new byte[] { 0, 0, 0, 2 }));
        this.objects.Add(new InventoryItem(new byte[] { 1, 0, 0, 0 }));
        this.objects.Add(new InventoryItem(new byte[] { 1, 0, 0, 1 }));
        this.objects.Add(new InventoryItem(new byte[] { 1, 0, 0, 2 }));
    }

    public void Add(InventoryObject target)
    {
        this.objects.Add(target);
    }

    public void Remove(InventoryObject target)
    {
        if (this.objects.Contains(target))
        {
            this.objects.Remove(target);
        }
    }

    public InventoryPanel FindSamePanel(InventoryObject panel)
    {
        foreach (InventoryObject t in this.objects)
        {
            if (t.GetType() == typeof(InventoryPanel))
            {
                if (Object.ReferenceString(t.Reference) == Object.ReferenceString(panel.Reference))
                {
                    return (InventoryPanel)t;
                }
            }
        }
        return null;
    }

    public void OnGUI()
    {
        if ((Player.Instance.GMode == GameMode.Inventory) || (Player.Instance.GMode == GameMode.Container))
        {
            GUILayout.BeginArea(Rect.MinMaxRect(0.55f * Screen.width, 0.25f * Screen.height, 0.95f * Screen.width, 0.75f * Screen.height));
            for (int i = 0; i < this.objects.Count; i += 6)
            {
                GUILayout.BeginHorizontal();
                for (int j = i; (j < this.objects.Count) && (j < i + 6); j++)
                {
                    InventoryObject o = this.objects[j];
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

    public void OnInventoryClick(InventoryObject o)
    {
        if (Player.Instance.GMode == GameMode.Inventory)
        {
            o.EquipObject();
        }
        else if (Player.Instance.GMode == GameMode.Container)
        {
            this.Remove(o);
            this.TargetContainer.Add(o);
        }
    }
}
