using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Side
{
    Right = 0,
    Up = 1,
    Forward = 2,
    Left = 3,
    Down = 4,
    Back = 5
};

public class Grid : MonoBehaviour {

    static private Grid instance;
    static public Grid Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Grid>();
            }
            return instance;
        }
    }

    private GridItem[][][] gridItems;

    public void Start()
    {
        this.gridItems = new GridItem[255][][];
        for (int i = 0; i < 255; i++)
        {
            this.gridItems[i] = new GridItem[31][];
            for (int j = 0; j < 31; j++)
            {
                this.gridItems[i][j] = new GridItem[255];
            }
        }

        Panel p = new Panel(6, 1, 6, 0);
        p.Instantiate();
        this.gridItems[6][1][6] = p;

        p = new Panel(8, 1, 6, 0);
        p.Instantiate();
        this.gridItems[8][1][6] = p;

        p = new Panel(8, 1, 8, 0);
        p.Instantiate();
        this.gridItems[8][1][8] = p;

        p = new Panel(6, 1, 8, 0);
        p.Instantiate();
        this.gridItems[6][1][8] = p;
        
    }

    public void AddPanel(int iPos, int jPos, int kPos, byte reference) 
    {
        if (Panel.IsPanelPos(iPos, jPos, kPos))
        {
            if (this.gridItems[iPos][jPos][kPos] == null)
            {
                Panel p = new Panel(iPos, jPos, kPos, reference);
                p.Instantiate();
                this.gridItems[iPos][jPos][kPos] = p;
            }
        }
    }

    public byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        for (int i = 0; i < 255; i++)
        {
            for (int j = 0; j < 31; j++)
            {
                for (int k = 0; k < 255; k++)
                {
                    if (this.gridItems[i][j][k] != null)
                    {
                        save.AddRange(this.gridItems[i][j][k].GetSave());
                    }
                }
            }
        }
        return save.ToArray();
    }
}
