using UnityEngine;
using System.Collections;

public abstract class InventoryObject {

    protected string reference;
    public string Reference
    {
        get
        {
            return this.reference;
        }
    }
    public byte[] ReferenceByte
    {
        get
        {
            return Object.ReferenceStringToByteArray(this.Reference);
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

    public InventoryObject(string reference)
    {
        GameObject prefab = Loader.Get(reference);
        if (Object.ReferenceStringToFirstByte(reference) == 0)
        {
            Panel p = prefab.GetComponent<Panel>();
            this.displayName = p.DisplayName;
        }
        else if (Object.ReferenceStringToFirstByte(reference) == 1)
        {
            Item item = prefab.GetComponent<Item>();
            this.displayName = item.DisplayName;
        }
        this.DisplayPicture = Resources.Load<Texture2D>("Textures/Inventory/" + reference + "_inventory");
        this.reference = reference;
    }

    public static InventoryObject CreateFromRef(string reference)
    {
        if (Object.ReferenceStringToFirstByte(reference) == 0)
        {
            return new InventoryPanel(reference);
        }
        else if (Object.ReferenceStringToFirstByte(reference) == 1)
        {
            return new InventoryItem(reference);
        }
        return null;
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
    public InventoryPanel(string reference)
        : base(reference)
    {

    }
}

public class InventoryItem : InventoryObject
{
    public InventoryItem(string reference)
        : base(reference)
    {

    }
}
