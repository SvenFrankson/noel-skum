using UnityEngine;
using System.Collections;

public abstract class GridCell : Object
{    
    protected byte[] GetPosSave()
    {
        return new byte[] { (byte)this.iPos, (byte)this.jPos, (byte)this.kPos };
    }
}
