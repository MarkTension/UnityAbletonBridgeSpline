using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Bezier : MonoBehaviour
{
    public LineRenderer LineRenderer;
//    public Transform point0, point1, point2;
    public GameObject prefabVertex;
    public GameObject prefabAngle;

    private int numPoint = 5;
    public int numVertices = 7;
    public float radius = 3.0f;
    public List<float> vertexX;
    public List<float> vertexY;
    public List<float> midsX;
    public List<float> midsY;

    public List<GameObject> positions;
    public List<GameObject> positionsMid;
    
    public Vector2[] vertArray;

    public Tuple<List<float>, List<float>> ResetScript()
    {
        vertArray = new Vector2[numPoint*numVertices];
        
        LineRenderer.startWidth = 2f;
        LineRenderer.endWidth = 2f;
        
        midsX = new List<float>(numVertices);
        midsY = new List<float>(numVertices);

        for (int i = 0; i < numVertices; i++)
        {
            midsX.Add(new float());
            midsY.Add(new float());
        }

        if (positions.Count != 0)
        {
            // destroy old gameobjects
            for (int i = 0; i < numVertices; i++)
            {
                Destroy(positions[i]);
                Destroy(positionsMid[i]);
            }
        }
        
        positions = new List<GameObject>(numVertices);
        positionsMid = new List<GameObject>(numVertices);

        CreatePolygon(); // polygon will be the vcontrol points that the user can change
        CreateMids(); // between polygon vertices



        for (int i = 0; i < numVertices; i++)
        {
            var point = Instantiate(prefabVertex, new Vector3(vertexX[i], vertexY[i], 0),
                new UnityEngine.Quaternion(0, 0, 0, 0));
            
            var angle = Instantiate(prefabAngle, new Vector3(midsX[i], midsY[i], 0),
                new UnityEngine.Quaternion(0, 0, 0, 0));
            positions.Add(point);
            positionsMid.Add(angle);
        }
        
        return Tuple.Create(vertexX, vertexY);
    }
    

    private void CreatePolygon()
    {
        vertexX = new List<float>(numVertices);
        vertexY = new List<float>(numVertices);
        var rand = UnityEngine.Random.Range(0.1f, 1.0f);
        for (int i = 0; i < numVertices; i++) {
            vertexX.Add(radius * Mathf.Cos(2 * Mathf.PI * i / numVertices) * rand);
            vertexY.Add(radius * Mathf.Sin(2 * Mathf.PI * i / numVertices) * rand);
        }
    }
    
    private void CreateMids()
    {
        for (int i = 0; i < numVertices; i++)
        {
            float xNew;
            float yNew;
            Vector2 midpoint;
            if (i != numVertices - 1)
            {
                midpoint = new Vector2(vertexX[i], vertexY[i]) * 0.5f + new Vector2(vertexX[i+1], vertexY[i+1]) * (0.5f );

                xNew = midpoint.x;
                yNew = midpoint.y;
            }
            else
            {
                midpoint = new Vector2(vertexX[i], vertexY[i]) * 0.5f + new Vector2(vertexX[0], vertexY[0]) * (0.5f );

                xNew = midpoint.x;
                yNew = midpoint.y;
            }

            midsX[i] = xNew;
            midsY[i] = yNew;
        }
    }


    public Tuple<List<float>, List<float>, bool, Vector2[]> ModifySpline(int vertex, int vertexAction, float amount)
    {
        //change position of gameObject
        //change position of gameObject
        var movementAmount = 0.4f * amount *0.2f;
        if (vertexAction == 1)
        {
            movementAmount *= -1f;
        }

        var norm = positions[vertex].transform.position / positions[vertex].transform.position.magnitude * movementAmount; //* direction;

        bool error = false;
        if (float.IsNaN(norm.x))
        {
            error = true;
        }
        else
        {
            positions[vertex].transform.Translate(norm);
            vertexX[vertex] = positions[vertex].transform.position.x;
            vertexY[vertex] = positions[vertex].transform.position.y;
    
            // recalculate midpoints
            CreateMids();
            // change midpoints
            for (int i = 0; i < numVertices; i++)
            {
                positionsMid[i].transform.position = new Vector3(midsX[i], midsY[i], 0f);
            }

            // plot midpoints
            CreateSpline();
        }
        
        return Tuple.Create(vertexX, vertexY, error, vertArray);
    }
    

    private void CreateSpline()
    {
        LineRenderer.positionCount = numPoint*numVertices;
        Vector3 p0, p1 ,p2;

        for(int j = 0; j < positions.Count; j++)
        {
            if (j < positions.Count - 1)
            {
                // determine control points of segment
                p0 = positionsMid[j].transform.position;
                p1 = positions[j + 1].transform.position;
                p2 = positionsMid[j + 1].transform.position;
            }

            else
            {
                // determine control points of segment
                p0 = positionsMid[j].transform.position;
                p1 = positions[0].transform.position;
                p2 = positionsMid[0].transform.position;
            }
            // set points of quadratic Bezier curve
            Vector3 position;
            float t;
            float pointStep = 1.0f / numPoint;

            for(int i = 0; i < numPoint; i++) 
            {
                t = i * pointStep;
                position = (1.0f - t) * (1.0f - t) * p0 
                           + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
                LineRenderer.SetPosition(i + j * numPoint, position);
                vertArray[i + j * numPoint] = new Vector2(position[0], position[1]);
            }
        }
    }
}
