using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

    private GridCell[][][] gridcells;
    private List<Item> items;

    public void Start()
    {
        this.gridcells = new GridCell[255][][];
        for (int i = 0; i < 255; i++)
        {
            this.gridcells[i] = new GridCell[255][];
            for (int j = 0; j < 255; j++)
            {
                this.gridcells[i][j] = new GridCell[255];
            }
        }

        this.items = new List<Item>();

        /*
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
        */

        this.Load();
    }

    public void AddPanel(int iPos, int jPos, int kPos, byte reference) 
    {
        if (Panel.IsPanelPos(iPos, jPos, kPos))
        {
            if (this.gridcells[iPos][jPos][kPos] == null)
            {
                this.gridcells[iPos][jPos][kPos] = Panel.PanelConstructor(iPos, jPos, kPos, reference);
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
            this.gridcells[target.iPos][target.jPos][target.kPos] = null;
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
        for (int i = 0; i < 255; i++)
        {
            for (int j = 0; j < 255; j++)
            {
                for (int k = 0; k < 255; k++)
                {
                    if (this.gridcells[i][j][k] != null)
                    {
                        save.AddRange(this.gridcells[i][j][k].GetSave());
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

        FileStream saveFile = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
        BinaryWriter dataStream = new BinaryWriter(saveFile);

        dataStream.Write(this.GetSave());

        dataStream.Close();
        saveFile.Close();

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
            if (b == 2)
            {
                int iPos = dataStream.ReadByte();
                int jPos = dataStream.ReadByte();
                int kPos = dataStream.ReadByte();
                int reference = dataStream.ReadByte();
                this.AddPanel(iPos, jPos, kPos, (byte)reference);
            }
            else if (b == 3) {
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

        return 1;
    }
}
