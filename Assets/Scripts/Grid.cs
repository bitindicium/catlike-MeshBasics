using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
   public int xSize, ySize;

   private Vector3[] _vertices;
   private Mesh _mesh;

   private void Awake()
   {
      StartCoroutine(Generate());
   }

   private IEnumerator Generate()
   {
      var wait = new WaitForSeconds(0.05f);

      GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
      _mesh.name = "Procedural Grid";

      _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
      for (int i = 0, y = 0; y <= ySize; y++) {
         for (int x = 0; x <= xSize; x++, i++) {
            _vertices[i] = new Vector3(x, y);
         }
      }

      _mesh.vertices = _vertices;

      var triangles = new int[xSize * ySize * 6];
      for (int ti = 0, vi = 0, y = 0; y < ySize; ++y, ++vi)
      {
         for (int x = 0; x < xSize; x++, ti += 6, ++vi)
         {
            triangles[ti] = vi;
            triangles[ti + 3] = triangles[ti + 2] = vi + 1;
            triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
            triangles[ti + 5] = vi + xSize + 2;

            _mesh.triangles = triangles;
            yield return wait;
         }
      }

      _mesh.triangles = triangles;
   }

   private void OnDrawGizmos()
   {
      if (_vertices == null)
         return;

      Gizmos.color = Color.black;
      foreach (var vertex in _vertices)
      {
         Gizmos.DrawSphere(vertex, 0.1f);
      }
   }
}
