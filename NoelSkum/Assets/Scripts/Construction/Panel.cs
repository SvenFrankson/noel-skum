using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : GridItem
{

    private byte reference;
    public byte Reference {
        get {
            return this.reference;
        }
    }
    private string name;
    private PanelInstance instance;
    public PanelInstance Instance
    {
        get
        {
            return this.instance;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            if (this.iPos % 2 == 1)
            {
                return Quaternion.Euler(0f, 0f, 90f);
            }
            if (this.jPos % 2 == 1)
            {
                return Quaternion.identity;
            }
            if (this.kPos % 2 == 1)
            {
                return Quaternion.Euler(90f, 0f, 0f);
            }
            else
            {
                return Quaternion.Euler(45f, 45f, 45f);
            }
        }

    }

    public Panel(int iPos, int jPos, int kPos, byte reference)
        : base(iPos, jPos, kPos)
    {
        this.reference = reference;
    }

    public int Update(int iPos, int jPos, int kPos, byte reference)
    {
        if ((this.iPos != iPos) || (this.jPos != jPos) || (this.kPos != kPos) || (this.reference != reference))
        {
            this.iPos = iPos;
            this.jPos = jPos;
            this.kPos = kPos;
            this.reference = reference;
            return 1;
        }
        return 0;
    }

    public PanelInstance Instantiate()
    {
        this.instance = PanelInstance.PanelConstructor(this);
        return this.instance;
    }

    public override byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add(2);
        save.AddRange(this.GetPosSave());
        save.Add(this.reference);

        return save.ToArray();
    }

    static public bool IsPanelPos(int i, int j, int k)
    {
        int evenCount = 0;
        if (i % 2 == 1)
        {
            evenCount++;
        }
        if (j % 2 == 1)
        {
            evenCount++;
        }
        if (k % 2 == 1)
        {
            evenCount++;
        }

        return evenCount == 1;
    }

    static public void WorldPosToPanelPos(out int iPos, out int jPos, out int kPos, Vector3 worldPos) 
    {
        iPos = 0;
        jPos = 0;
        kPos = 0;

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
                    if (IsPanelPos(iPoses[i], jPoses[j], kPoses[k]))
                    {
                        float value = Mathf.Pow(2 * worldPos.x - iPoses[i], 2f) + Mathf.Pow(2 * worldPos.y - jPoses[j], 2f) + Mathf.Pow(2 * worldPos.z - kPoses[k], 2f);
                        if (value < bestValue)
                        {
                            bestValue = value;
                            iPos = iPoses[i];
                            jPos = jPoses[j];
                            kPos = kPoses[k];
                        }
                    }
                }
            }
        }
    }
}
