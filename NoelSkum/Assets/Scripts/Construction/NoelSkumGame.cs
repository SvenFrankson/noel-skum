using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum Side
{
    Right = 0,
    Up = 1,
    Forward = 2,
    Left = 3,
    Down = 4,
    Back = 5
};

public class NoelSkumGame : MonoBehaviour {

    static private NoelSkumGame instance;
    static public NoelSkumGame Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NoelSkumGame>();
            }
            return instance;
        }
    }

    private int gridCellSize = 127;
    private Dictionary<string,GridCell[][][]> gridcells;
    private List<Item> items;

    public void Start()
    {
        this.gridcells = new Dictionary<string, GridCell[][][]>();
        this.items = new List<Item>();

        this.Load();
    }

    static string Coordinates(int I, int J, int K)
    {
        return I + "_" + J + "_" + K;
    }

    public void AddGridCells(int I, int J, int K)
    {
        string coordinates = Coordinates(I, J, K);
        GridCell[][][] gridCell = new GridCell[this.gridCellSize][][];
        for (int i = 0; i < this.gridCellSize; i++)
        {
            gridCell[i] = new GridCell[this.gridCellSize][];
            for (int j = 0; j < this.gridCellSize; j++)
            {
                gridCell[i][j] = new GridCell[this.gridCellSize];
            }
        }
        this.gridcells.Add(coordinates, gridCell);
    }

    public GridCell[][][] GetGridCells(int iPos, int jPos, int kPos)
    {
        int I = iPos / this.gridCellSize;
        int J = jPos / this.gridCellSize;
        int K = kPos / this.gridCellSize;
        string c = Coordinates(I, J, K);

        if (!this.gridcells.ContainsKey(c))
        {
            this.AddGridCells(I, J, K);
        }

        return this.gridcells[c];
    }

    public void AddPanel(int iPos, int jPos, int kPos, byte reference) 
    {
        GridCell[][][] gridCell = GetGridCells(iPos, jPos, kPos);
        iPos = iPos % this.gridCellSize;
        jPos = jPos % this.gridCellSize;
        kPos = kPos % this.gridCellSize;

        if (Panel.IsPanelPos(iPos, jPos, kPos))
        {
            if (gridCell[iPos][jPos][kPos] == null)
            {
                gridCell[iPos][jPos][kPos] = Panel.PanelConstructor(iPos, jPos, kPos, reference);
            }
        }
    }

    public void AddItem(int iPos, int jPos, int kPos, int rot, byte[] reference)
    {
        Item item = Item.ItemConstructor(iPos, jPos, kPos, rot, reference);
        this.items.Add(item);
    }

    public void DestroyObject(Object target)
    {
        if (target.GetType() == typeof(GridCell))
        {
            GridCell[][][] gridCell = GetGridCells(target.iPos, target.jPos, target.kPos);
            int iPos = target.iPos % this.gridCellSize;
            int jPos = target.jPos % this.gridCellSize;
            int kPos = target.kPos % this.gridCellSize;
            gridCell[iPos][jPos][kPos] = null;
        }
        else if (target.GetType() == typeof(Item))
        {
            this.items.Remove((Item)target);
        }
        Destroy(target.gameObject);
    }

    public byte[] GetSave()
    {
        List<byte> save = new List<byte>();
        foreach (GridCell[][][] gridCell in this.gridcells.Values)
        {
            for (int i = 0; i < this.gridCellSize; i++)
            {
                for (int j = 0; j < this.gridCellSize; j++)
                {
                    for (int k = 0; k < this.gridCellSize; k++)
                    {
                        if (gridCell[i][j][k] != null)
                        {
                            save.AddRange(gridCell[i][j][k].GetSave());
                        }
                    }
                }
            }
        }
        foreach (Item item in this.items)
        {
            save.AddRange(item.GetSave());
        }
        return save.ToArray();
    }

    public int Save()
    {
        Debug.Log("Start Save");
        string directoryPath = Application.dataPath + "/../Save/";
        Directory.CreateDirectory(directoryPath);
        string saveFilePath = directoryPath + "grid.data";

        byte[] saveData = this.GetSave();
        Debug.Log(saveData.Length);

        try
        {
            FileStream saveFile = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
            BinaryWriter dataStream = new BinaryWriter(saveFile);

            dataStream.Write(saveData);

            dataStream.Close();
            saveFile.Close();
        }
        catch (Exception e) {
            Debug.Log(e);
        }

        Debug.Log("Save Done");
        return 1;
    }

    public int Load()
    {
        string directoryPath = Application.dataPath + "/../Save/";
        Directory.CreateDirectory(directoryPath);
        string saveFilePath = directoryPath + "grid.data";

        try
        {
            FileStream saveFile = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader dataStream = new BinaryReader(saveFile);

            int b = dataStream.ReadByte();
            while (b != -1)
            {
                if (b == 2)
                {
                    int iPos = dataStream.ReadByte();
                    int jPos = dataStream.ReadByte();
                    int kPos = dataStream.ReadByte();
                    int reference = dataStream.ReadByte();
                    this.AddPanel(iPos, jPos, kPos, (byte)reference);
                }
                else if (b == 3)
                {
                    int iPos = dataStream.ReadByte();
                    int jPos = dataStream.ReadByte();
                    int kPos = dataStream.ReadByte();
                    int rot = dataStream.ReadByte();
                    byte[] reference = dataStream.ReadBytes(4);
                    this.AddItem(iPos, jPos, kPos, rot, reference);
                }
                try
                {
                    b = dataStream.ReadByte();
                }
                catch (EndOfStreamException)
                {
                    b = -1;
                }
            }

            dataStream.Close();
            saveFile.Close();
        }
        catch (Exception)
        {

        }

        return 1;
    }
}
