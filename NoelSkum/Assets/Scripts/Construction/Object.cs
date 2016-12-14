using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;

public abstract class Object : MonoBehaviour
{
    public string SetUpReference;
    public abstract string Reference
    {
        get;
    }
    public byte[] ReferenceByte
    {
        get
        {
            return ReferenceStringToByteArray(this.Reference);
        }
    }
    public string DisplayName;
    public Coordinates cGlobal;
    private List<ObjectMenuOptionType> menuOptions = new List<ObjectMenuOptionType>();
    public List<ObjectMenuOptionType> MenuOptions
    {
        get
        {
            return this.menuOptions;
        }
    }

    public Object()
    {
        this.MenuOptions.Add(ObjectMenuOptionType.PickUp);
    }

    public void HighLight(Color color, float outlineWidth)
    {
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();

        foreach (Renderer r in renderers)
        {
            materials.AddRange(r.GetComponent<Renderer>().materials);
        }

        foreach (Material material in materials)
        {
            material.SetColor("_OutlineColor", color);
            material.SetFloat("_Outline", outlineWidth);
        }
    }

    public void PickUp()
    {
        NoelSkumGame.Instance.DestroyObject(this);
        Inventory.Instance.Add(InventoryObject.CreateFromObject(this));
        Player.Instance.GMode = GameMode.Normal;
    }

    public abstract Vector3 Position();
    public abstract int UpdatePos(Coordinates cGlobal, int rot = 0);
    public abstract byte[] GetSave();

    static public byte ReferenceStringToFirstByte(string reference)
    {
        string refPart = reference.Substring(0, 2);
        return byte.Parse(refPart, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    static public byte[] ReferenceStringToByteArray(string reference)
    {
        byte[] referenceByte = new byte[reference.Length / 2];
        for (int i = 0; i < referenceByte.Length; i++)
        {
            string refPart = reference.Substring(2 * i, 2);
            referenceByte[i] = byte.Parse(refPart, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
        return referenceByte;
    }

    static public string ReferenceByteArrayToString(byte[] reference)
    {
        return BitConverter.ToString(reference).Replace("-", "");
    }
}
