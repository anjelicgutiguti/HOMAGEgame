using System.Collections.Generic;
using UnityEngine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public int width = 16;
    public int depth = 16;
    public int maxHeight = 16;

    public float noiseScale = 0.08f; // Scale of the Perlin noise
    public int seed = 100; // Seed for the Perlin noise generation

    private bool[,,] blocks; // 3D array to store the blocks in the terrain

    private List<Vector3> vertices = new List<Vector3>(); // List to store the vertices of the mesh
    private List<int> triangles = new List<int>(); // List to store the triangle indices of the mesh

    private static readonly Vector3[] cubeVertices = new Vector3[]
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 1, 0),
        new Vector3(0, 1, 0),

        new Vector3(0, 0, 1),
        new Vector3(1, 0, 1),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 1)
    };

    private static readonly int[,] faces =
    {
        { 0, 1, 2, 3 }, // Back
        { 5, 4, 7, 6 }, // Front
        { 4, 0, 3, 7 }, // Left
        { 1, 5, 6, 2 }, // Right
        { 4, 5, 1, 0 }, // Bottom
        { 3, 2, 6, 7 }  // Top
    };

    private static readonly Vector3Int[] directions =
    {
        new Vector3Int(0, 0, -1), // Back
        new Vector3Int(0, 0, 1),  // Front
        new Vector3Int(-1, 0, 0), // Left
        new Vector3Int(1, 0, 0),  // Right
        new Vector3Int(0, -1, 0), // Bottom
        new Vector3Int(0, 1, 0)   // Top
    };

    void Start()
    {
        GenerateBlocks();
        GenerateMesh();
    }

    void GenerateBlocks()
    {
        blocks = new bool[width, maxHeight, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float worldX = transform.position.x + x;
                float worldZ = transform.position.z + z;

                float noise = Mathf.PerlinNoise((worldX + seed) * noiseScale, (worldZ + seed) * noiseScale);

               int minimumHeight = 4; // Minimum height of the terrain
                
               int terrainVariatin = maxHeight - minimumHeight - 1; // Calculate the variation in height based on maxHeight and minimumHeight
               int height = minimumHeight + Mathf.FloorToInt(noise * terrainVariatin); // Calculate the height of the terrain based on the noise value
               
               for (int y = 0; y <= height; y++)
                {
                    blocks[x, y, z] = true; // Set the block at the given position to true (solid)
                }
            }
        }

    }

    void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (blocks[x, y, z])
                    {
                        AddBlock(new Vector3Int(x, y, z));
                    }
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Use 32-bit indices for large meshes
        mesh.vertices = vertices.ToArray(); // Assign the vertices and triangles to the mesh
        mesh.triangles = triangles.ToArray(); // Assign the vertices and triangles to the mesh

        mesh.RecalculateBounds(); // Recalculate the bounds of the mesh
        mesh.RecalculateNormals();// Recalculate the normals of the mesh

        GetComponent<MeshFilter>().mesh = mesh; // Assign the mesh to the MeshFilter component
        GetComponent<MeshCollider>().sharedMesh = mesh; // Assign the mesh to the MeshCollider component
    }

    void AddBlock(Vector3Int position) // Add a block at the given position and generate its mesh
    {
        for (int face = 0; face < 6; face++) // Loop through each face of the cube
        {
            Vector3Int neighbor = position + directions[face]; // Check if the neighbor block is out of bounds or empty

            if (IsBlock(neighbor))
                continue; // If the neighbor block is not empty, skip this face
            int vertetxIndex = vertices.Count; // Store the current vertex index

            for (int i = 3; i >= 0; i--) // Loop through each vertex of the face
            {
                int cubeVertex = faces[face, i];

                vertices.Add(position + cubeVertices[cubeVertex]); // Add the vertex to the list of vertices
            }
            triangles.Add(vertetxIndex);
            triangles.Add(vertetxIndex + 1);
            triangles.Add(vertetxIndex + 2);
            triangles.Add(vertetxIndex);
            triangles.Add(vertetxIndex + 2);
            triangles.Add(vertetxIndex + 3);
        }
    }

    bool IsBlock(Vector3Int position) // Check if the block at the given position is out of bounds or empty
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= maxHeight || position.z < 0 || position.z >= depth)
            return false; // If the position is out of bounds, return false
        return blocks[position.x, position.y, position.z]; // Return true if the block is not empty, false otherwise
    }   
}