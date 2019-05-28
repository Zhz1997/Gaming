using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshGen:MonoBehaviour
{
    public Mesh mesh;
    public int size;
    public Vector3[] vertices;
    public int[] triangles;
    public float maxHeight;
    public float minHeight;
    public MeshFilter meshFilter;

    void Main()
    {
        generateMesh();
        meshFilter.sharedMesh = mesh;
    }


    public void generateMesh()
    {
        mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        size = 250;
        vertices = new Vector3[(size + 1) * (size + 1)];
        triangles = new int[size * size * 2 * 3];
        maxHeight = -1000;
        minHeight = 1000;


        for (int i = 0, x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++, i++)
            {
                float y = Mathf.PerlinNoise(x * .1f, z * .1f) * 10f;
                if (y < minHeight)
                {
                    minHeight = y;
                }
                if (y > maxHeight)
                {
                    maxHeight = y;
                }
                vertices[i] = new Vector3(x, y, z);
                //Debug.Log(vertices[i]);
            }
        }

        Debug.Log("Min is " + minHeight.ToString());
        Debug.Log("Max is " + maxHeight.ToString());

        int ii = 0, ti = 0;
        for (int x = 0; x < size; x++)
        {
            int zz = 0;
            while (zz < size)
            {
                //1st triangle
                triangles[ti] = ii;
                ti = ti + 1;
                triangles[ti] = ii + 1;
                ti = ti + 1;
                triangles[ti] = ii + size + 1;
                ti = ti + 1;
                //2nd triangle 
                triangles[ti] = ii + 1;
                ti = ti + 1;
                triangles[ti] = ii + 1 + size + 1;
                ti = ti + 1;
                triangles[ti] = ii + size + 1;
                ti = ti + 1;
                zz = zz + 1;
                ii = ii + 1;
            }
            ii = ii + 1;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }



}
