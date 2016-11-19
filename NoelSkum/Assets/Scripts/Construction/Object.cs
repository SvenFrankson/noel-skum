using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Object : MonoBehaviour
{
    public string displayName;
    public Coordinates cGlobal;

    public List<ObjectMenuOptionType> MenuOptions;

    public void HighLight(Color color, float outlineWidth)
    {
        Material[] materials = this.GetComponent<Renderer>().materials;
        foreach (Material material in materials)
        {
            material.SetColor("_OutlineColor", color);
            material.SetFloat("_Outline", outlineWidth);
        }
    }

    public Vector3 Position
    {
        get
        {
            return this.cGlobal.Position / 2f;
        }
    }

    public void PickUp()
    {
        NoelSkumGame.Instance.DestroyObject(this);
        Player.Instance.GMode = GameMode.Normal;
    }

    public abstract string ReferenceString();
    public abstract int UpdatePos(Coordinates cGlobal, int rot = 0);
    public abstract byte[] GetSave();
}
