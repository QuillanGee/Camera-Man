using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;

public class ObjectProjection : MonoBehaviour
{
    //for current Mesh
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;
    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

    //for spawning new mesh
    private GameObject projectedMeshObject;
    private Mesh projectedMesh;
    private PolygonCollider2D polygonCollider;
    public Material projectedMaterial;

    private float fixedZ = 3.25f;

    void Awake()
    {
        // Get the Mesh Filter attached to this GameObject
        meshFilter = GetComponent<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void GetMeshData()
    {
        if (meshFilter != null)
        {
            mesh = meshFilter.mesh;
            vertices = mesh.vertices;   
            // Convert local verticies to world
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transform.TransformPoint(vertices[i]);
                // Debug.Log("Vertex " + i + " world position: " + vertices[i]);
            }
        }
        else
        {
            Debug.LogError("No MeshFilter found on the GameObject!");
        }
    }
    
    public void UpdatePerception()
    {
        GetMeshData();
        Vector3[] projectedVerticies = ProjectVerticesTo2D(vertices);
        
        // Check if currMesh is null, if not destory currMesh (gameobject)
        DestoryProjectedMesh();
        projectedMeshObject = new GameObject("ProjectedMesh");
        
        projectedMesh = Create2DMesh(projectedVerticies, mesh.triangles);
        polygonCollider = projectedMeshObject.AddComponent<PolygonCollider2D>();
        AddPolygonColliderFromProjectedVertices(projectedVerticies,polygonCollider);
        
        projectedMeshObject.AddComponent<MeshFilter>().mesh = projectedMesh;
        projectedMeshObject.AddComponent<MeshRenderer>().material = projectedMaterial;
    }

    public void DestoryProjectedMesh()
    {
        if (projectedMeshObject != null)
        {
            Destroy(projectedMeshObject);
        }
    }
    
    Vector3[] ProjectVerticesTo2D(Vector3[] currVertices)
    {
        Vector3[] projectedVertices = new Vector3[currVertices.Length];

        // Loop through each vertex
        for (int i = 0; i < currVertices.Length; i++)
        {
            Vector3 vertex = currVertices[i];
            
            float distanceToPlane = fixedZ - vertex.z;

            // Calculate the perspective factor (scaling by distance to the camera)
            float scaleFactor = 1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane)); // Avoid division by zero

            // Apply the perspective projection
            float projectedX = vertex.x * scaleFactor;
            float projectedY = vertex.y * scaleFactor;

            // The Z is now fixed to the target Z-plane
            projectedVertices[i] = new Vector3(projectedX, projectedY, 0);
            print(projectedVertices[i]);
        }

        return projectedVertices;
    }
    
    Vector3[] ProjectVerticesTo2DAlgorithm2(Vector3[] currVertices)
    {
        Vector3[] projectedVerticies = new Vector3[currVertices.Length];

        // Matrix for orthographic projection onto the XY-plane
        Matrix4x4 projectionMatrix = Matrix4x4.identity;
        projectionMatrix.m22 = 0f; // Set Z value to zero (flatten the Z axis)

        // Apply the matrix transformation to each vertex
        for (int i = 0; i < currVertices.Length; i++)
        {
            projectedVerticies[i] = projectionMatrix.MultiplyPoint3x4(currVertices[i]);
        }

        return projectedVerticies;
    }
    
    Mesh Create2DMesh(Vector3[] vertices, int[] triangles)
    {
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = triangles;

        // Recalculate normals and bounds for the new mesh
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        return newMesh;
    }
    
    void AddPolygonColliderFromProjectedVertices(Vector3[] projectedVertices,PolygonCollider2D polyCollider)
    {
        // Convert the 3D vertices (projected onto a 2D plane) to 2D vertices
        Vector2[] points2D = new Vector2[projectedVertices.Length];
        for (int i = 0; i < projectedVertices.Length; i++)
        {
            // Use X and Y coordinates, ignoring Z (since it's projected)
            points2D[i] = new Vector2(projectedVertices[i].x, projectedVertices[i].y);
        }

        // Set the vertices of the PolygonCollider2D
        polyCollider.SetPath(0, points2D); // Set the points as the path of the collider
    }
}
