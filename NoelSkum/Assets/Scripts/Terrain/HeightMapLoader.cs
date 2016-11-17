using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HeightMapLoader {

    public Dictionary<string, byte[]> heightMaps = new Dictionary<string,byte[]>();

    public int[][] LoadHeightMap(int iPos, int jPos)
    {
        int[][] heightMap = new int[Chunck.ChunckSize + 4][];
        for (int i = 0; i < Chunck.ChunckSize + 4; i++)
        {
            heightMap[i] = new int[Chunck.ChunckSize + 4];
            for (int j = 0; j < Chunck.ChunckSize + 4; j++)
            {
                heightMap[i][j] = GetHeight(iPos * Chunck.ChunckSize + i - 2, jPos * Chunck.ChunckSize + j - 2);
            }
        }

        return heightMap;
    }

    public int GetHeight(int i, int j)
    {
        if ((i < 0) || (j < 0)) {
            return 0;
        }

        int iIndex = i / 1024;
        int jIndex = j / 1024;
        string heightMapName = "map_" + iIndex + "_" + jIndex + ".png";

        byte[] map = this.GetMap(heightMapName);

        return map[(i % 1024) + (j % 1024) * 1024];
    }

    public byte[] GetMap(string heightMapName)
    {
        if (!this.heightMaps.ContainsKey(heightMapName)) {
            this.heightMaps.Add(heightMapName, this.Load(heightMapName));
        }

        return this.heightMaps[heightMapName];
    }

    public byte[] Load(string heightMapName)
    {
        string directoryPath = Application.dataPath + "/../Terrain/";
        string saveFilePath = directoryPath + heightMapName;
        byte[] heightMap = File.ReadAllBytes(saveFilePath);
        Texture2D heightMapTexture = new Texture2D(1024, 1024);
        heightMapTexture.LoadImage(heightMap);

        Color[] heightMapTextureColors = heightMapTexture.GetPixels();
        heightMap = new byte[1024 * 1024];
        for (int i = 0; i < 1024 * 1024; i++)
        {
            heightMap[i] = (byte)Mathf.FloorToInt(heightMapTextureColors[i].r * 256);
        }

        return heightMap;
    }
}
