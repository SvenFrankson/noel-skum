﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Object : MonoBehaviour
{
    protected byte[] reference;
    public byte[] Reference
    {
        get
        {
            return this.reference;
        }
    }
    public string displayName;
    public Coordinates cGlobal;

    public List<ObjectMenuOptionType> MenuOptions;

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
    public abstract string ReferenceString();
    public abstract int UpdatePos(Coordinates cGlobal, int rot = 0);
    public abstract byte[] GetSave();

    public static string ReferenceString(byte[] reference)
    {
        string referenceString = "";
        foreach (byte b in reference)
        {
            referenceString += b.ToString();
        }
        return referenceString;
    }
}
