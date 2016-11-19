using UnityEngine;
using System.Collections;

public abstract class GridCell : Object
{    
    protected byte[] GetPosSave()
    {
        return new byte[] { (byte)this.cGlobal.i, (byte)this.cGlobal.j, (byte)this.cGlobal.k };
    }
}
