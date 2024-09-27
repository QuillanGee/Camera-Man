using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class ObjectProjection : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;
    private Vector2[] uv;

    private Mesh projectedMesh;

    public Material projectedMaterial;

    void Start()
    {
        // Get the Mesh Filter attached to this GameObject
        meshFilter = GetComponent<MeshFilter>();

        // Ensure the GameObject has a MeshFilter
        if (meshFilter != null)
        {
            // Access the Mesh from the Mesh Filter
            mesh = meshFilter.mesh;

            // Now you can access the mesh data
            vertices = mesh.vertices;        // Get the vertices
            triangles = mesh.triangles;          // Get the triangle indices
            normals = mesh.normals;          // Get the normals
            uv = mesh.uv;                    // Get the UV coordinates

        }
        else
        {
            Debug.LogError("No MeshFilter found on the GameObject!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Forward movement
        {
            UpdatePerception();
        }
    }


    // Example: Modify the mesh (move vertices, recalculate bounds)

    void UpdatePerception()
    {
        Vector3[] transformedVertices = ProjectVerticesTo2D(vertices);
        projectedMesh = Create2DMesh(transformedVertices, mesh.triangles);

        // Create a GameObject to display the new mesh
        GameObject projectedMeshObject = new GameObject("ProjectedMesh");
        projectedMeshObject.AddComponent<MeshFilter>().mesh = projectedMesh;
        projectedMeshObject.AddComponent<MeshRenderer>().material = projectedMaterial;

        // meshFilter.mesh.vertices = vertices;
        // // After modifying, assign the updated vertices back to the mesh            
        // // Recalculate bounds and normals if you modify the mesh
        // meshFilter.mesh.RecalculateBounds();
        // meshFilter.mesh.RecalculateNormals();
    }
    
    Vector3[] ProjectVerticesTo2D(Vector3[] vertices)
    {
        Vector3[] transformedVertices = new Vector3[vertices.Length];

        // Matrix for orthographic projection onto the XY-plane
        Matrix4x4 projectionMatrix = Matrix4x4.identity;
        projectionMatrix.m22 = 0f; // Set Z value to zero (flatten the Z axis)

        // Apply the matrix transformation to each vertex
        for (int i = 0; i < vertices.Length; i++)
        {
            transformedVertices[i] = projectionMatrix.MultiplyPoint3x4(vertices[i]);
        }

        return transformedVertices;
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
}
