using UnityEngine;
using System.Collections;

public abstract class InventoryObject {

    protected byte[] reference;
    private string displayName;
    public string DisplayName
    {
        get
        {
            return this.displayName;
        }
    }
    public Texture2D DisplayPicture;

    public InventoryObject(byte r)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/panel_" + r.ToString());
        Panel p = prefab.GetComponent<Panel>();

        DisplayPicture = Resources.Load<Texture2D>("Textures/Inventory/panel_" + r.ToString() + "_inventory");

        reference = new byte[] { r };
        displayName = p.displayName;
    }

    public InventoryObject(byte[] r)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/item_" + Item.ReferenceString(r));
        Item item = prefab.GetComponent<Item>();

        DisplayPicture = Resources.Load<Texture2D>("Textures/Inventory/item_" + Item.ReferenceString(r) + "_inventory");

        reference = r;
        displayName = item.displayName;
    }

    public abstract void OnSelectedItem();
}

public class InventoryPanel : InventoryObject
{
    public byte PanelReference
    {
        get
        {
            return reference[0];
        }
    }

    public InventoryPanel(byte reference)
        : base(reference)
    {

    }

    public override void OnSelectedItem()
    {
        Player.Instance.SwitchToSetPanel(this.PanelReference);
    }
}

public class InventoryItem : InventoryObject
{
    public byte[] Reference
    {
        get
        {
            return reference;
        }
    }

    public InventoryItem(byte[] reference)
        : base(reference)
    {

    }

    public override void OnSelectedItem()
    {
        Player.Instance.SwitchToSetItem(this.Reference);
    }
}
