using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : GridCell
{
    private byte reference;
    public byte Reference {
        get {
            return this.reference;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            if (this.cGlobal.i % 2 == 1)
            {
                return Quaternion.Euler(0f, 0f, 90f);
            }
            if (this.cGlobal.j % 2 == 1)
            {
                return Quaternion.identity;
            }
            if (this.cGlobal.k % 2 == 1)
            {
                return Quaternion.Euler(90f, 0f, 0f);
            }
            else
            {
                return Quaternion.Euler(45f, 45f, 45f);
            }
        }

    }

    public static Panel PanelConstructor(Coordinates cGlobal, byte reference)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/panel_" + reference.ToString());
        GameObject instance = Instantiate<GameObject>(prefab);
        Panel p = instance.GetComponent<Panel>();

        p.reference = reference;
        p.cGlobal = cGlobal;

        p.transform.position = p.Position;
        p.transform.rotation = p.Rotation;

        return p;
    }

    public override string ReferenceString()
    {
        return this.Reference.ToString();
    }

    public override int UpdatePos(Coordinates cGlobal, int rot = 0)
    {
        if (this.cGlobal != cGlobal)
        {
            this.cGlobal = cGlobal;

            this.transform.position = this.Position;
            this.transform.rotation = this.Rotation;

            return 1;
        }
        return 0;
    }

    public override byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add(2);
        save.AddRange(this.GetPosSave());
        save.Add(this.reference);

        return save.ToArray();
    }

    static public bool IsPanelPos(Coordinates cGlobal)
    {
        int evenCount = 0;
        if (cGlobal.i % 2 == 1)
        {
            evenCount++;
        }
        if (cGlobal.j % 2 == 1)
        {
            evenCount++;
        }
        if (cGlobal.k % 2 == 1)
        {
            evenCount++;
        }

        return evenCount == 1;
    }

    static public Coordinates WorldPosToPanelPos(Vector3 worldPos) 
    {
        Coordinates cGlobal = Coordinates.Zero;
        int[] iPoses = new int[2];
        int[] jPoses = new int[2];
        int[] kPoses = new int[2];
        iPoses[0] = Mathf.FloorToInt(2 * worldPos.x);
        iPoses[1] = Mathf.CeilToInt(2 * worldPos.x);
        jPoses[0] = Mathf.FloorToInt(2 * worldPos.y);
        jPoses[1] = Mathf.CeilToInt(2 * worldPos.y);
        kPoses[0] = Mathf.FloorToInt(2 * worldPos.z);
        kPoses[1] = Mathf.CeilToInt(2 * worldPos.z);

        float bestValue = float.MaxValue;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Coordinates cTemp = new Coordinates(iPoses[i], jPoses[j], kPoses[k]);
                    if (IsPanelPos(cTemp))
                    {
                        float value = Mathf.Pow(2 * worldPos.x - iPoses[i], 2f) + Mathf.Pow(2 * worldPos.y - jPoses[j], 2f) + Mathf.Pow(2 * worldPos.z - kPoses[k], 2f);
                        if (value < bestValue)
                        {
                            bestValue = value;
                            cGlobal = cTemp;
                        }
                    }
                }
            }
        }

        return cGlobal;
    }
}
