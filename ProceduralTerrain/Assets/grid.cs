using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    public GameObject tree;
    // Start is called before the first frame update
    Mesh mesh;
    [Range(1,500)]
    public int NumberOfTrees = 50;
    int size = 250;
    int freq = 10;
    Vector3[] treePos;
    Vector3[] vertices;
    int[] triangles;
    float maxHeight;
    float minHeight;
    int onetime = 0;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[(size + 1) * (size + 1)];
        triangles = new int[size * size * 2 * 3];
        maxHeight = -1000;
        minHeight = 1000;
        float[,] height = Perlin2DGen(size, freq);
        treePos = new Vector3[NumberOfTrees];
        List<Vector3> treePosL = new List<Vector3>();


        for (int i = 0, x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++, i++)
            {
                float y = height[x, z];
                if (y < minHeight)
                {
                    minHeight = y;
                }
                if (y > maxHeight)
                {
                    maxHeight = y;
                }
                vertices[i] = new Vector3(x, y, z);
                if (y > 5&&y<15)
                {
                    treePosL.Add(vertices[i]);
                }
            }
        }
        int counter = 0;

        while (counter != NumberOfTrees)
        {
            int index = Random.Range(0,treePosL.Count);
            int check = 0;
            Vector3 cur = treePosL[index];
            treePosL.RemoveAt(index);
            for (int i = 0; i < counter; i++)
            {
                Vector3 temp = treePos[i];
                if (Mathf.Abs(cur.x - temp.x) <= 3 && Mathf.Abs(cur.z - temp.z) <= 3)
                {
                    check = 1;
                    break;
                }
            }
            if(check == 0)
            {
                treePos[counter] = cur;
                counter = counter + 1;
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        Debug.Log(height[0, 0]);
        Debug.Log(mesh.normals[0]);
        Vector3 pos = new Vector3(0, height[0, 0] * 1.5f, 0);
        Debug.Log(mesh.normals.Length);


    }

    void Update()
    {
        if (onetime==0)
        {
            for(int i = 0; i < NumberOfTrees; i++)
            {
                //Debug.Log(treePos[i]);
            }
            //Debug.Log(freq);
            for (int i = 0; i < NumberOfTrees; i++)
            {
                Vector3 tempPos = new Vector3(treePos[i].x, 30, treePos[i].z);
                RaycastHit hit;
                if (Physics.Raycast(tempPos, Vector3.down, out hit))
                {
                    //Debug.Log("better");
                    Vector3 hitPoint = hit.point;
                    Quaternion orientation;

                    orientation = Quaternion.FromToRotation(Vector3.left, hit.normal);
                    tree.transform.localScale = new Vector3(30, 30, 30);

                    Instantiate(tree, hitPoint, orientation);

                }
            }
            onetime = 1;
        }
    }



        //f(t) = 6t^5-15t^4+10t^3
        float interpolation_function(float t)
        {
            float t_cubic = t * t * t;
            float t_square = t * t;

            return 6 * (t_cubic * t_square) - 15 * (t_square * t_square) + 10 * t_cubic;
        }


        float[,] Perlin2DGen(int num_sample, int frequency)
        {
            float pxPerGrid = num_sample / frequency;
            Vector2[,] gradients = new Vector2[frequency + 2, frequency + 2];
            float[,] noise = new float[num_sample + 1, num_sample + 1];

            //Create random gradients at all cell corners
            for (int i = 0; i <= frequency + 1; i++)
            {
                for (int j = 0; j <= frequency + 1; j++)
                {
                    Vector2 rand_vector = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
                    gradients[i, j] = rand_vector.normalized;
                }
            }

            //good so far

            for (float x = 0; x <= num_sample; x++)
            {
                for (float z = 0; z <= num_sample; z++)
                {

                    int xi = Mathf.FloorToInt(x / pxPerGrid);
                    int xf = xi + 1;
                    int zi = Mathf.FloorToInt(z / pxPerGrid);
                    int zf = zi + 1;

                    float xCellMin = xi * pxPerGrid;
                    float xCellMax = xCellMin + pxPerGrid;
                    float zCellMin = zi * pxPerGrid;
                    float zCellMax = zCellMin + pxPerGrid;

                    Vector2 a = new Vector2(x - xCellMin, z - zCellMin);
                    Vector2 b = new Vector2(x - xCellMax, z - zCellMin);
                    Vector2 c = new Vector2(x - xCellMin, z - zCellMax);
                    Vector2 d = new Vector2(x - xCellMax, z - zCellMax);

                    float s = Vector2.Dot(a, gradients[xi, zi]);
                    float t = Vector2.Dot(b, gradients[xf, zi]);
                    float u = Vector2.Dot(c, gradients[xi, zf]);
                    float v = Vector2.Dot(d, gradients[xf, zf]);

                    float sx = interpolation_function((x - xCellMin) / pxPerGrid);
                    float sy = interpolation_function((z - zCellMin) / pxPerGrid);

                    float aa = mix(s, t, sx);
                    float bb = mix(u, v, sx);
                    float value = mix(aa, bb, sy);



                    noise[(int)x, (int)z] = value*1.5f;

                }
            }


            return noise;
        }

        float mix(float x, float y, float a)
        {
            return (1 - a) * x + a * y;
        }


    }
 