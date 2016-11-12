using UnityEngine;
using System.Collections;

public abstract class GridCell
{

    protected int iPos;
    protected int jPos;
    protected int kPos;

    public Vector3 Position
    {
        get
        {
            return this.iPos / 2f * Vector3.right + this.jPos / 2f * Vector3.up + this.kPos / 2f * Vector3.forward;
        }
    }

    protected GridCell(int iPos, int jPos, int kPos)
    {
        this.iPos = iPos;
        this.jPos = jPos;
        this.kPos = kPos;
    }

    abstract public byte[] GetSave();

    protected byte[] GetPosSave()
    {
        return new byte[] { (byte)this.iPos, (byte)this.jPos, (byte)this.kPos };
    }
}
