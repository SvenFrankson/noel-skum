using UnityEngine;
using System.Collections;

/// <summary>
/// Usage : 
/// Name a Coordinates instance 'cGlobal' when refering to an absolute position.
/// Name a Coordinates instance 'cBlock' when refering to a GridCell[][][] position.
/// Name a Coordinates instance 'cLocal' when refering to a position relatively to a Block.
/// Note :
/// cGlobal = cBlock * GRIDSIZE + cLocal;
/// </summary>
public struct Coordinates {
    static public int GRIDSIZE = 127;
    public int i;
    public int j;
    public int k;

    public Coordinates(int i, int j, int k)
    {
        this.i = i;
        this.j = j;
        this.k = k;
    }

    public Coordinates(byte[] b)
    {
        this.i = b[0] * GRIDSIZE + b[1];
        this.j = b[2] * GRIDSIZE + b[3];
        this.k = b[4] * GRIDSIZE + b[5];
    }

    public Coordinates ToBlock 
    {
        get {
            return this / GRIDSIZE;
        }
    }

    public Coordinates ToLocal
    {
        get
        {
            return this % GRIDSIZE;
        }
    }

    public Vector3 Position
    {
        get
        {
            return new Vector3(this.i, this.j, this.k);
        }
    }

    public byte[] ToByte()
    {
        byte[] b = new byte[6];
        b[0] = (byte)this.ToBlock.i;
        b[1] = (byte)this.ToLocal.i;
        b[2] = (byte)this.ToBlock.j;
        b[3] = (byte)this.ToLocal.j;
        b[4] = (byte)this.ToBlock.k;
        b[5] = (byte)this.ToLocal.k;
        return b;
    }

    static public Coordinates Zero {
        get {
            return new Coordinates(0, 0, 0);
        }
    }

    static public Coordinates operator +(Coordinates c1, Coordinates c2)
    {
        return (new Coordinates(c1.i + c2.i, c1.j + c2.j, c1.k + c2.k));
    }

    static public Coordinates operator /(Coordinates c, int n)
    {
        return (new Coordinates(c.i / n, c.j / n, c.k / n));
    }

    static public Coordinates operator %(Coordinates c, int n)
    {
        return (new Coordinates(c.i % n, c.j % n, c.k % n));
    }

    static public bool operator ==(Coordinates c1, Coordinates c2)
    {
        return (((c1.i == c2.i) && (c1.j == c2.j)) && (c1.k == c2.k));
    }

    static public bool operator !=(Coordinates c1, Coordinates c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object obj)
    {
        return this == (Coordinates)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
