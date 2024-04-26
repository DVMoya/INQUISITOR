using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterMeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public GameObject target;
    public float planeHeight = 0;
    public int xSize = 50;
    public int zSize = 50;
    public float frameRate = .1f;
    public float scale = 25f;
    public float xOffset = 0;
    public float zOffset = 0;
    public Vector2 flow = new Vector2(.05f, .05f);

    public Gradient gradient;

    private float maxTerrainHeight;
    private float minTerrainHeight;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        StartCoroutine(UpdateWater(frameRate));
    }

    private void Update()
    {
        Follow();
    }

    IEnumerator UpdateWater(float fps)
    {
        do
        {
            CreateShape();
            UpdateMesh();

            yield return new WaitForSeconds(fps);
        } while (flow != Vector2.zero);
    }

    void CreateShape()
    {
        // vertex count = (xSize + 1) * (zSize + 1) --> the number of vertices needed
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = CalculateVertexHeight(x, z);
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight) maxTerrainHeight = y;
                if (y < minTerrainHeight) minTerrainHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6]; // 6 vertices for each square

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++; // get rid of the aberrant triangle formed when jumping to the next line
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float heigth = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(heigth);
                i++;
            }
        }

        xOffset += flow.x;
        zOffset += flow.y;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    float CalculateVertexHeight(int x, int z)
    {
        float xCoord = (float)x / xSize * scale + xOffset;
        float zCoord = (float)z / zSize * scale + zOffset;

        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    void Follow()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x,
                                        0,
                                        target.transform.position.z);
        Vector3 offsetPos = new Vector3(-(xSize / 2) * (transform.localScale.x),
                                        planeHeight,
                                        -(zSize / 2) * (transform.localScale.x));
        transform.position = targetPos + offsetPos;
    }
}