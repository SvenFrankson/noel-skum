using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : GridItem {

    public Cube(int iPos, int jPos, int kPos) 
        : base(iPos, jPos, kPos)
    {
        
    }

    public override byte[] GetSave()
    {
        throw new System.NotImplementedException();
    }
}
