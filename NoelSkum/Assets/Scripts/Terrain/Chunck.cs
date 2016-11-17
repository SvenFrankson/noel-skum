using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunck : MonoBehaviour {

    public int iPos;
    public int jPos;
    static public int ChunckSize = 64;
    public int[][] heightMap;

	void Start () {
        HeightMapLoader loader = new HeightMapLoader();
        this.heightMap = loader.LoadHeightMap(iPos, jPos);

        this.GetComponent<MeshFilter>().sharedMesh = this.GetMesh();
        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().sharedMesh;
	}

    private Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[][] vertices = new Vector3[2 * ChunckSize + 7][];
        Vector3[][] newVertices = new Vector3[2 * ChunckSize + 7][];
        Vector3[][] normals = new Vector3[2 * ChunckSize + 7][];
        List<Vector3> verticesList = new List<Vector3>();
        List<Vector3> normalsList = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < 2 * ChunckSize + 7; i++)
        {
            vertices[i] = new Vector3[2 * ChunckSize + 7];
            newVertices[i] = new Vector3[2 * ChunckSize + 7];
            normals[i] = new Vector3[2 * ChunckSize + 7];
            // Zero-initialization is required for debug purpose only
            for (int j = 0; j < 2 * ChunckSize + 7; j++)
            {
                vertices[i][j] = Vector3.zero;
                newVertices[i][j] = Vector3.zero;
            }
        }

        // Set vertices know from heightmap
        for (int i = 0; i < ChunckSize + 4; i++)
        {
            for (int j = 0; j < ChunckSize + 4; j++)
            {
                vertices[2 * i][2 * j] = new Vector3(- 0.5f + i, this.heightMap[i][j] * 0.5f, - 0.5f + j);
            }
        }

        // Set face-vertices
        for (int i = 1; i < 2 * ChunckSize + 7; i += 2)
        {
            for (int j = 1; j < 2 * ChunckSize + 7; j += 2)
            {
                vertices[i][j] = (vertices[i - 1][j - 1] + vertices[i - 1][j + 1] + vertices[i + 1][j - 1] + vertices[i + 1][j + 1]) / 4f;
            }
        }

        // Set edge-vertices along x-oriented edges
        for (int i = 1; i < 2 * ChunckSize + 7; i += 2)
        {
            for (int j = 0; j < 2 * ChunckSize + 7; j += 2)
            {
                if ((j > 0) && (j < 2 * ChunckSize + 6))
                {
                    vertices[i][j] = (vertices[i - 1][j] + vertices[i + 1][j] + vertices[i][j - 1] + vertices[i][j + 1]) / 4f;
                }
                else
                {
                    vertices[i][j] = (vertices[i - 1][j] + vertices[i + 1][j]) / 2f;
                }
            }
        }

        // Set edge-vertices along z-oriented edges
        for (int i = 0; i < 2 * ChunckSize + 7; i += 2)
        {
            for (int j = 1; j < 2 * ChunckSize + 7; j += 2)
            {
                if ((i > 0) && (i < 2 * ChunckSize + 6))
                {
                    vertices[i][j] = (vertices[i][j - 1] + vertices[i][j + 1] + vertices[i - 1][j] + vertices[i + 1][j]) / 4f;
                }
                else
                {
                    vertices[i][j] = (vertices[i][j - 1] + vertices[i][j + 1]) / 2f;
                }
            }
        }

        // Smooth vertices known from heightmap
        for (int i = 4; i < 2 * ChunckSize + 3; i += 2)
        {
            for (int j = 4; j < 2 * ChunckSize + 3; j += 2)
            {
                Vector3 faceVerticesMedium = (vertices[i - 1][j - 1] + vertices[i + 1][j - 1] + vertices[i - 1][j + 1] + vertices[i + 1][j + 1]) / 4f;
                Vector3 edgeVerticesMedium = (vertices[i - 1][j] + vertices[i + 1][j] + vertices[i][j - 1] + vertices[i][j + 1]) / 4f;
                newVertices[i][j] = (faceVerticesMedium + 2 * edgeVerticesMedium + vertices[i][j]) / 4f;
            }
        }
        for (int i = 4; i < 2 * ChunckSize + 3; i += 2)
        {
            for (int j = 4; j < 2 * ChunckSize + 3; j += 2)
            {
                vertices[i][j] = newVertices[i][j];
            }
        }

        // Set normals for vertices known from heightmap
        for (int i = 3; i < 2 * ChunckSize + 4; i++)
        {
            for (int j = 3; j < 2 * ChunckSize + 4; j++)
            {
                Vector3 n1 = -Vector3.Cross(vertices[i + 1][j] - vertices[i][j], vertices[i][j + 1] - vertices[i][j]);
                Vector3 n2 = -Vector3.Cross(vertices[i - 1][j] - vertices[i][j], vertices[i][j - 1] - vertices[i][j]);
                normals[i][j] = (n1 + n2).normalized;
                if (normals[i][j].y < 0)
                {
                    Debug.Log("Erreur");
                }
            }
        }

        // Set triangles
        for (int j = 0; j < 2 * ChunckSize + 0; j++)
        {
            for (int i = 0; i < 2 * ChunckSize + 0; i++)
            {
                if (Vector3.SqrMagnitude(vertices[3 + i][3 + j] - vertices[3 + i + 1][3 + j + 1]) > Vector3.SqrMagnitude(vertices[3 + i + 1][3 + j] - vertices[3 + i][3 + j + 1]))
                {
                    triangles.Add(i + j * (2 * ChunckSize + 1));
                    triangles.Add(i + (j + 1) * (2 * ChunckSize + 1));
                    triangles.Add((i + 1) + j * (2 * ChunckSize + 1));

                    triangles.Add(i + (j + 1) * (2 * ChunckSize + 1));
                    triangles.Add((i + 1) + (j + 1) * (2 * ChunckSize + 1));
                    triangles.Add((i + 1) + j * (2 * ChunckSize + 1));
                }
                else
                {
                    triangles.Add(i + j * (2 * ChunckSize + 1));
                    triangles.Add(i + (j + 1) * (2 * ChunckSize + 1));
                    triangles.Add((i + 1) + (j + 1) * (2 * ChunckSize + 1));

                    triangles.Add((i + 1) + (j + 1) * (2 * ChunckSize + 1));
                    triangles.Add((i + 1) + j * (2 * ChunckSize + 1));
                    triangles.Add(i + j * (2 * ChunckSize + 1));
                }
            }
        }
        
        // Linearize vertices grid
        for (int j = 3; j < 2 * ChunckSize + 4; j++)
        {
            for (int i = 3; i < 2 * ChunckSize + 4; i++)
            {
                verticesList.Add(vertices[i][j]);
                normalsList.Add(normals[i][j]);
            }
        }

        mesh.vertices = verticesList.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normalsList.ToArray();

        return mesh;
    }
}
