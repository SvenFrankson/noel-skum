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

    private Dictionary<Coordinates, GridCell[][][]> gridcells;
    private List<Item> items;

    public void Start()
    {
        this.gridcells = new Dictionary<Coordinates, GridCell[][][]>();
        this.items = new List<Item>();

        this.Load();
    }

    public void AddGridCells(Coordinates c)
    {
        GridCell[][][] gridCell = new GridCell[Coordinates.GRIDSIZE][][];
        for (int i = 0; i < Coordinates.GRIDSIZE; i++)
        {
            gridCell[i] = new GridCell[Coordinates.GRIDSIZE][];
            for (int j = 0; j < Coordinates.GRIDSIZE; j++)
            {
                gridCell[i][j] = new GridCell[Coordinates.GRIDSIZE];
            }
        }
        this.gridcells.Add(c, gridCell);
    }

    public GridCell[][][] GetGridCells(Coordinates cGlobal)
    {
        Coordinates c = cGlobal.ToBlock;

        if (!this.gridcells.ContainsKey(c))
        {
            this.AddGridCells(c);
        }

        return this.gridcells[c];
    }

    public void AddPanel(Coordinates cGlobal, byte[] reference) 
    {
        Coordinates cLocal = cGlobal.ToLocal;
        GridCell[][][] gridCell = GetGridCells(cGlobal);

        if (Panel.IsPanelPos(cGlobal))
        {
            if (gridCell[cLocal.i][cLocal.j][cLocal.k] == null)
            {
                gridCell[cLocal.i][cLocal.j][cLocal.k] = Panel.PanelConstructor(cGlobal, reference);
            }
        }
    }

    public Item AddItem(Coordinates cGlobal, int rot, byte[] reference)
    {
        Item item = Item.ItemConstructor(cGlobal, rot, reference);
        this.items.Add(item);
        return item;
    }

    public void DestroyObject(Object target)
    {
        if (target.GetType() == typeof(GridCell))
        {
            GridCell[][][] gridCell = GetGridCells(target.cGlobal);
            Coordinates cLocal = target.cGlobal.ToLocal;
            gridCell[cLocal.i][cLocal.j][cLocal.k] = null;
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
            for (int i = 0; i < Coordinates.GRIDSIZE; i++)
            {
                for (int j = 0; j < Coordinates.GRIDSIZE; j++)
                {
                    for (int k = 0; k < Coordinates.GRIDSIZE; k++)
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

        FileStream saveFile = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader dataStream = new BinaryReader(saveFile);

        int b = dataStream.ReadByte();
        while (b != -1)
        {
            byte[] cGlobalByte = dataStream.ReadBytes(6);
            Coordinates cGlobal = new Coordinates(cGlobalByte);
            if (b == 2)
            {
                byte[] reference = dataStream.ReadBytes(4);
                this.AddPanel(cGlobal, reference);
            }
            else if (b == 3)
            {
                int rot = dataStream.ReadByte();
                byte[] reference = dataStream.ReadBytes(4);
                this.AddItem(cGlobal, rot, reference);
            }
            else if (b == 4)
            {
                int rot = dataStream.ReadByte();
                byte[] reference = dataStream.ReadBytes(4);
                Item item = this.AddItem(cGlobal, rot, reference);
                int contentCount = dataStream.ReadByte();
                for (int i = 0; i < contentCount; i++)
                {
                    Debug.Log("Loading content into Container");
                    reference = dataStream.ReadBytes(4);
                    item.Content.Add(InventoryObject.CreateFromRef(reference));
                }
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

        return 1;
    }
}
