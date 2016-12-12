using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : GridCell
{
    public bool Foundation;
    public bool Door;
    private bool openedDoor;
    public bool OpenedDoor
    {
        get
        {
            return this.openedDoor;
        }
        set
        {
            if (this.openedDoor != value)
            {
                if (value == true)
                {
                    this.Open();
                }
                else
                {
                    this.Close();
                }
            }
            this.openedDoor = value;
        }
    }

    private Animator c_animator;
    private Animator C_Animator
    {
        get
        {
            if (c_animator == null)
            {
                c_animator = this.GetComponent<Animator>();
            }
            return c_animator;
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
                return Quaternion.Euler(0f, 90f, 90f);
            }
            else
            {
                return Quaternion.Euler(45f, 45f, 45f);
            }
        }

    }

    public static Panel PanelConstructor(Coordinates cGlobal, byte[] reference)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/panel_" + ReferenceString(reference));
        GameObject instance = Instantiate<GameObject>(prefab);
        Panel p = instance.GetComponent<Panel>();

        p.reference = reference;
        p.cGlobal = cGlobal;

        p.transform.position = p.Position();
        p.transform.rotation = p.Rotation;

        return p;
    }

    public override string ReferenceString()
    {
        return ReferenceString(this.Reference);
    }

    public override int UpdatePos(Coordinates cGlobal, int rot = 0)
    {
        if (this.cGlobal != cGlobal)
        {
            this.cGlobal = cGlobal;

            this.transform.position = this.Position();
            this.transform.rotation = this.Rotation;

            return 1;
        }
        return 0;
    }

    public override Vector3 Position()
    {
        return this.cGlobal.Position / 2f;
    }

    public void Open()
    {
        this.C_Animator.SetTrigger("Open");
    }

    public void Close()
    {
        this.C_Animator.SetTrigger("Close");
    }

    public void SwitchOpenDoor()
    {
        this.OpenedDoor = !this.OpenedDoor;
    }

    public override byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        save.Add(2);
        save.AddRange(this.cGlobal.ToByte());
        save.AddRange(this.reference);

        return save.ToArray();
    }

    static public bool IsPanelPos(Coordinates cGlobal, bool foundation = false, bool door = false)
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

        if (foundation && cGlobal.j % 2 != 1)
        {
            return false;
        }

        if (door && cGlobal.j % 2 == 1)
        {
            return false;
        }

        return evenCount == 1;
    }

    static public Coordinates WorldPosToPanelPos(Vector3 worldPos, bool foundation = false, bool door = false) 
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
                    if (IsPanelPos(cTemp, foundation, door))
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
