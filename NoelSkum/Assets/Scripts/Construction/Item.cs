using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Item : Object {

    private byte[] reference;
    public byte[] Reference
    {
        get
        {
            return this.reference;
        }
    }
    public int rot;

    public static Item ItemConstructor(int iPos, int jPos, int kPos, int rot, byte[] reference)
    {
        Debug.Log(ReferenceString(reference));
        GameObject prefab = Resources.Load<GameObject>("Prefabs/item_" + ReferenceString(reference));
        GameObject instance = Instantiate(prefab);
        Item item = instance.GetComponent<Item>();

        item.iPos = iPos;
        item.jPos = jPos;
        item.kPos = kPos;
        item.reference = reference;
        item.rot = rot;

        item.transform.position = item.Position;
        item.transform.rotation = Quaternion.AngleAxis(rot * 90f, Vector3.up);

        return item;
    }

    public override int UpdatePos(int iPos, int jPos, int kPos, int rot)
    {
        if ((this.iPos != iPos) || (this.jPos != jPos) || (this.kPos != kPos) || (this.rot != rot))
        {
            this.iPos = iPos;
            this.jPos = jPos;
            this.kPos = kPos;
            this.rot = rot;

            this.transform.position = this.Position;
            this.transform.rotation = Quaternion.AngleAxis(this.rot * 90f, Vector3.up);

            return 1;
        }
        return 0;
    }

    public int Move(int iOffset, int jOffset, int kOffset, int rotOffset)
    {
        return this.UpdatePos(this.iPos + iOffset, this.jPos + jOffset, this.kPos + kOffset, (this.rot + rotOffset) % 4);
    }

    public void MoveRight()
    {
        this.Move(1, 0, 0, 0);
    }

    public void MoveUp()
    {
        this.Move(0, 1, 0, 0);
    }

    public void MoveForward()
    {
        this.Move(0, 0, 1, 0);
    }

    public void MoveLeft()
    {
        this.Move(-1, 0, 0, 0);
    }

    public void MoveDown()
    {
        this.Move(0, -1, 0, 0);
    }

    public void MoveBack()
    {
        this.Move(0, 0, -1, 0);
    }

    public void RotatePlus()
    {
        this.Move(0, 0, 0, 1);
    }

    public void RotateMinus()
    {
        this.Move(0, 0, 0, -1);
    }

    public static string ReferenceString(byte[] reference)
    {
        string referenceString = "";
        foreach (byte b in reference)
        {
            referenceString += b.ToString();
        }
        return referenceString;
    }

    public override string ReferenceString()
    {
        return ReferenceString(this.Reference);
    }

    public override byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add(3);
        save.Add((byte)this.iPos);
        save.Add((byte)this.jPos);
        save.Add((byte)this.kPos);
        save.Add((byte)this.rot);
        save.AddRange(this.reference);

        return save.ToArray();
    }

    public static void WorldPosToItemPos(out int iPos, out int jPos, out int kPos, Vector3 worldPos)
    {
        iPos = Mathf.RoundToInt(2 * worldPos.x);
        jPos = Mathf.RoundToInt(2 * worldPos.y);
        kPos = Mathf.RoundToInt(2 * worldPos.z);
    }
}
