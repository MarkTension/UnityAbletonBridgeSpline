using UnityEngine;
 
public class MeshCreator : MonoBehaviour
{

    public MeshFilter filter;
    public void MakeMesh (Vector2[] vertices2D) {
        
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }
 
        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();


        MeshRenderer poo = GetComponent<MeshRenderer>();
        
        // Set up game object with mesh;
        if (gameObject.GetComponent<MeshFilter>() == null)
        {
            filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        }
        else
        {
            filter = gameObject.GetComponent<MeshFilter>();
        }
        filter.mesh = msh;
    }
}