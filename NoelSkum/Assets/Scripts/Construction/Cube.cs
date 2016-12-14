using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : GridCell
{
    public override string Reference
    {
        get 
        { 
            throw new System.NotImplementedException(); 
        }
    }

    public override Vector3 Position()
    {
        throw new System.NotImplementedException();
    }

    public override int UpdatePos(Coordinates cGlobal, int rot = 0)
    {
        throw new System.NotImplementedException();
    }

    public override byte[] GetSave()
    {
        throw new System.NotImplementedException();
    }
}
