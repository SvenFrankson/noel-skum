using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Item : Object {

    public int rot;

    public static Item ItemConstructor(Coordinates cGlobal, int rot, byte[] reference)
    {
        Debug.Log(ReferenceString(reference));
        GameObject prefab = Resources.Load<GameObject>("Prefabs/item_" + ReferenceString(reference));
        GameObject instance = Instantiate(prefab);
        Item item = instance.GetComponent<Item>();

        item.cGlobal = cGlobal;
        item.reference = reference;
        item.rot = rot;

        item.transform.position = item.Position();
        item.transform.rotation = Quaternion.AngleAxis(rot * 90f, Vector3.up);

        return item;
    }

    public override int UpdatePos(Coordinates cGlobal, int rot)
    {
        if (this.cGlobal != cGlobal || (this.rot != rot))
        {
            this.cGlobal = cGlobal;
            this.rot = rot;

            this.transform.position = this.Position();
            this.transform.rotation = Quaternion.AngleAxis(this.rot * 90f, Vector3.up);

            return 1;
        }
        return 0;
    }

    public int Move(int iOffset, int jOffset, int kOffset, int rotOffset)
    {
        Coordinates newCGlobal = this.cGlobal + new Coordinates(iOffset, jOffset, kOffset);
        return this.UpdatePos(newCGlobal, (this.rot + rotOffset) % 4);
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

    public override Vector3 Position()
    {
        return this.cGlobal.Position / 2f + 0.25f * Vector3.one;
    }

    public override string ReferenceString()
    {
        return ReferenceString(this.Reference);
    }

    public override byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add(3);
        save.AddRange(this.cGlobal.ToByte());
        save.Add((byte)this.rot);
        save.AddRange(this.reference);

        return save.ToArray();
    }

    public static Coordinates WorldPosToItemPos(Vector3 worldPos)
    {
        int i = Mathf.RoundToInt(2 * (worldPos.x - 0.5f));
        int j = Mathf.RoundToInt(2 * (worldPos.y - 0.5f));
        int k = Mathf.RoundToInt(2 * (worldPos.z - 0.5f));

        return new Coordinates(i, j, k);
    }
}
