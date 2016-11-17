using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System;

public class PlanetDataGenerator : EditorWindow {

    private int iPos = 0;
    private int jPos = 0;
    private RandomSeed randomizer;
    private RandomSeed Randomizer
    {
        get
        {
            if (randomizer == null)
            {
                randomizer = new RandomSeed(42);
            }
            return randomizer;
        }
    }

    private int MINMAPDEGREE = 8;
    private int MAPSIZEDEGREE = 10;
    private int mapSize = -1;
    private int MAPSIZE
    {
        get
        {
            if (this.mapSize == -1)
            {
                this.mapSize = Mathf.FloorToInt(Mathf.Pow(2f, this.MAPSIZEDEGREE));
            }
            return this.mapSize;
        }
    }
    private int MAXMAPDEGREE = 15;
    private int MAPHEIGHT = 127;

    [MenuItem("Window/PlanetDataGenerator")]
	static void Open () {
        PlanetDataGenerator instance = EditorWindow.GetWindow<PlanetDataGenerator>();
	}

	public void OnGUI () {
        this.iPos = EditorGUILayout.IntField("IPos", this.iPos);
        this.jPos = EditorGUILayout.IntField("JPos", this.jPos);
		if (GUILayout.Button ("Create Terrain Data")) {
            byte[][] terrainData = getMap(this.iPos, this.jPos);
            this.SaveTerrainData(terrainData, this.iPos, this.jPos);
        }
	}

    public int SaveTerrainData(byte[][] terrainData, int iPos, int jPos)
    {
        int TextureSize = terrainData.Length;
        string directoryPath = Application.dataPath + "/../Terrain/";
        Directory.CreateDirectory(directoryPath);
        string saveFilePath = directoryPath + "map_" + iPos + "_" + jPos + ".png";
        FileStream saveFile = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
        BinaryWriter dataStream = new BinaryWriter(saveFile);
        Color[] heightMap = new Color[TextureSize * TextureSize];

        for (int j = 0; j < TextureSize; j++)
        {
            for (int i = 0; i < TextureSize; i++)
            {
                float g = terrainData[i][j] / 256f;
                heightMap[i + j * TextureSize] = new Color(g, g, g);
            }
        }
        Texture2D heightMapTexture = new Texture2D(TextureSize, TextureSize);
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
        Byte[][] heightMap = new Byte[1024][];

        for (int i = 0; i < 1024; i++)
        {
            heightMap[i] = new Byte[1024];
            for (int j = 0; j < 1024; j++)
            {
                int h = Mathf.FloorToInt(EvaluateBiCubic(i + iPos * 1024, j + jPos * 1024, Randomizer) * 128) + 64;
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

    private float BiCubicInterpolation(float x, float y, float[][] values) 
    {
        float i0, i1, i2, i3;
        i0 = TERP(x, values[0][0], values[1][0], values[2][0], values[3][0]);
        i1 = TERP(x, values[0][1], values[1][1], values[2][1], values[3][1]);
        i2 = TERP(x, values[0][2], values[1][2], values[2][2], values[3][2]);
        i3 = TERP(x, values[0][3], values[1][3], values[2][3], values[3][3]);

        return TERP(y, i0, i1, i2, i3);
    }

    float[][] getMapLowDegree(int iPos, int jPos, int degree) {
        int step = Mathf.FloorToInt(Mathf.Pow(2f, degree));
        int xPos = iPos * this.MAPSIZE;
        int yPos = jPos * this.MAPSIZE;
        float val;
        float[][] values = new float[4][];
        float[][] map = new float[MAPSIZE][];
        int dx, dy, i, j, i0, j0, i1, j1;

        for (i1 = 0; i1 < 4; i1 += 1) {
            values[i1] = new float[4];
        }
        for (i = 0; i < this.MAPSIZE; i += 1) {
            map[i] = new float[MAPSIZE];
        }

        for (i = 0; i < this.MAPSIZE; i += step) {
            for (j = 0; j < this.MAPSIZE; j += step) {
                for (i0 = 0; i0 < step; i0 += 1) {
                    for (j0 = 0; j0 < step; j0 += 1) {
                        for (i1 = 0; i1 < 4; i1 += 1) {
                            for (j1 = 0; j1 < 4; j1 += 1) {
                                values[i1][j1] = this.Randomizer.Rand(xPos + i + (i1 - 1) * step, yPos + j + (j1 - 1) * step, degree);
                            }
                        }

                        dx = i0 / step;
                        dy = j0 / step;

                        val = BiCubicInterpolation(dx, dy, values);
                        val = Mathf.Min(1, val);
                        val = Mathf.Max(0, val);

                        map[i + i0][j + j0] = val;
                    }
                }
            }
        }
        return map;
    }

    float[][] getMapHighDegree(int iPos, int jPos, int degree) {
        

        int step = Mathf.FloorToInt(Mathf.Pow(2f, degree));
        int xPos = iPos * this.MAPSIZE;
        int yPos = jPos * this.MAPSIZE;
        int x0 = Mathf.Abs(xPos) - (Mathf.Abs(xPos) % step);
        int y0 = Mathf.Abs(yPos) - (Mathf.Abs(yPos) % step);
        float val;
        float[][] values = new float[4][];
        float[][] map = new float[MAPSIZE][];
        int dx, dy, i, j;
            
        if (xPos < 0) {
            x0 = -x0 - step;
        }
            
        if (yPos < 0) {
            y0 = -y0 - step;
        }

        for (i = 0; i < 4; i += 1) {
            values[i] = new float[4];
            for (j = 0; j < 4; j += 1) {
                values[i][j] = this.Randomizer.Rand(x0 + (i - 1) * step, y0 + (j - 1) * step, degree);
            }
        }

        for (i = 0; i < this.MAPSIZE; i += 1) {
            map[i] = new float[this.MAPSIZE];
            for (j = 0; j < this.MAPSIZE; j += 1) {
                dx = (xPos + i - x0) / step;
                dy = (yPos + j - y0) / step;

                val = BiCubicInterpolation(dx, dy, values);
                val = Mathf.Min(1, val);
                val = Mathf.Max(0, val);

                map[i][j] = val;
            }
        }
        return map;
    }

    byte[][] getMap(int iPos, int jPos) {
        Debug.Log(this.MAPSIZE);
        int i = 0;
        int j = 0;
        int d = 0;
        float[][] mapFloat = new float[this.MAPSIZE][];
        byte[][] map = new byte[this.MAPSIZE][];
        float[][][] maps = new float[this.MAXMAPDEGREE][][];

        for (i = 0; i < this.MAPSIZE; i += 1) {
            map[i] = new byte[this.MAPSIZE];
            mapFloat[i] = new float[this.MAPSIZE];
        }

        for (d = this.MINMAPDEGREE; d < this.MAPSIZEDEGREE; d += 1) {
            maps[d] = this.getMapLowDegree(iPos, jPos, d);
        }
        for (d = this.MAPSIZEDEGREE; d < this.MAXMAPDEGREE; d += 1) {
            maps[d] = this.getMapHighDegree(iPos, jPos, d);
        }

        for (i = 0; i < this.MAPSIZE; i += 1) {
            for (j = 0; j < this.MAPSIZE; j += 1) {
                map[i][j] = (byte)(this.MAPHEIGHT / 2);
                for (d = this.MINMAPDEGREE; d < this.MAXMAPDEGREE; d += 1) {
                    mapFloat[i][j] += (maps[d][i][j] - 0.5f) / Mathf.Pow(2f, this.MAXMAPDEGREE - d) * this.MAPHEIGHT;
                }
                map[i][j] = (byte)Mathf.FloorToInt(mapFloat[i][j]);
            }
        }
        return map;
    }
}