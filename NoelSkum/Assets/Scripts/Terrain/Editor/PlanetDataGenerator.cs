using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System;

public class PlanetDataGenerator : EditorWindow {

    private int iPos = 0;
    private int jPos = 0;

    [MenuItem("Window/PlanetDataGenerator")]
	static void Open () {
        EditorWindow.GetWindow<PlanetDataGenerator>();
	}

	public void OnGUI () {
        this.iPos = EditorGUILayout.IntField("IPos", this.iPos);
        this.jPos = EditorGUILayout.IntField("JPos", this.jPos);
		if (GUILayout.Button ("Create Terrain Data")) {
            byte[][] terrainData = GetByteFor(this.iPos, this.jPos);
            this.SaveTerrainData(terrainData, this.iPos, this.jPos);
        }
	}

    public int SaveTerrainData(byte[][] terrainData, int iPos, int jPos)
    {
        string directoryPath = Application.dataPath + "/../Terrain/";
        Directory.CreateDirectory(directoryPath);
        string saveFilePath = directoryPath + "map_" + iPos + "_" + jPos + ".png";
        FileStream saveFile = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
        BinaryWriter dataStream = new BinaryWriter(saveFile);
        Color[] heightMap = new Color[1024 * 1024];

        for (int j = 0; j < 1024; j++)
        {
            for (int i = 0; i < 1024; i++)
            {
                float g = terrainData[i][j] / 256f;
                heightMap[i + j * 1024] = new Color(g, g, g);
            }
        }
        Texture2D heightMapTexture = new Texture2D(1024, 1024);
        heightMapTexture.SetPixels(heightMap);
        dataStream.Write(heightMapTexture.EncodeToPNG());

        dataStream.Close();
        saveFile.Close();

        return 1;
    }

    private void Generate()
    {
        Byte[][] terrainData = GetByteFor(0, 0);
        this.SaveTerrainData(terrainData, 0, 0);
    }

    private Byte[][] GetByteFor(int iPos, int jPos)
    {
        RandomSeed r = new RandomSeed(42);
        Byte[][] heightMap = new Byte[1024][];

        for (int i = 0; i < 1024; i++)
        {
            heightMap[i] = new Byte[1024];
            for (int j = 0; j < 1024; j++)
            {
                int h = Mathf.FloorToInt(EvaluateBiCubic(i + iPos * 1024, j + jPos * 1024, r) * 128) + 64;
                heightMap[i][j] = (byte)h;
            }
        }

        return heightMap;
    }

    static private float TERP(float t, float a, float b, float c, float d)
    {
        return 0.5f * (c - a + (2.0f * a - 5.0f * b + 4.0f * c - d + (3.0f * (b - c) + d - a) * t) * t) * t + b;
    }

    static public float EvaluateBiCubic(int x, int y, RandomSeed r)
    {
        int degree = 15;
        float value = 0f;

        for (int d = 8; d < degree; d++)
        {
            int range = Mathf.FloorToInt(Mathf.Pow(2f, degree - d));
            int x0 = (x / range) * range;
            int y0 = (y / range) * range;

            float xd = (float)(x % range) / (float)range;
            float yd = (float)(y % range) / (float)range;

            float[][] f = new float[4][];
            for (int i = 0; i < 4; i++)
            {
                f[i] = new float[4];
                for (int j = 0; j < 4; j++)
                {
                    f[i][j] = r.Rand(x0 + (i - 1) * range, y0 + (j - 1) * range, d);
                }
            }

            float[] fy = new float[4];
            for (int i = 0; i < 4; i++)
            {
                fy[i] = TERP(yd, f[i][0], f[i][1], f[i][2], f[i][3]);
            }

            float fx = TERP(xd, fy[0], fy[1], fy[2], fy[3]);

            value += fx / Mathf.FloorToInt(Mathf.Pow(2f, d - 8));
        }

        // Note : value belongs to  [-2f ; +2f]
        return (value + 2f) / 4f;
    }
}