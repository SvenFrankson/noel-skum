using UnityEngine;
using System.Collections;

public abstract class InventoryObject {

    protected byte[] reference;
    public byte[] Reference
    {
        get
        {
            return this.reference;
        }
    }
    private string displayName;
    public string DisplayName
    {
        get
        {
            return this.displayName;
        }
    }
    public Texture2D DisplayPicture;

    public InventoryObject(byte[] r)
    {
        GameObject prefab;
        if (r[0] == 0)
        {
            prefab = Resources.Load<GameObject>("Prefabs/panel_" + Panel.ReferenceString(r));
            Panel p = prefab.GetComponent<Panel>();
            this.displayName = p.displayName;
            this.DisplayPicture = Resources.Load<Texture2D>("Textures/Inventory/panel_" + Panel.ReferenceString(r) + "_inventory");
        }
        else if (r[0] == 1)
        {
            prefab = Resources.Load<GameObject>("Prefabs/item_" + Item.ReferenceString(r));
            Item item = prefab.GetComponent<Item>();
            this.displayName = item.displayName;
            this.DisplayPicture = Resources.Load<Texture2D>("Textures/Inventory/item_" + Item.ReferenceString(r) + "_inventory");
        }
        this.reference = r;
    }

    public static InventoryObject CreateFromObject(Object target)
    {
        if (target.GetType() == typeof(Panel))
        {
            return new InventoryPanel(target.Reference);
        }
        else if (target.GetType() == typeof(Item))
        {
            return new InventoryItem(target.Reference);
        }
        return null;
    }

    public void EquipObject()
    {
        Inventory.Instance.Remove(this);
        Player.Instance.Equip(this);
    }
}

public class InventoryPanel : InventoryObject
{
    public InventoryPanel(byte[] reference)
        : base(reference)
    {

    }
}

public class InventoryItem : InventoryObject
{
    public InventoryItem(byte[] reference)
        : base(reference)
    {

    }
}
