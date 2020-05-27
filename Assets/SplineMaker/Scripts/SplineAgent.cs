using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TreeEditor;
using Random = System.Random;

public class SplineAgent : MonoBehaviour
{
    public GameObject prefabBezier;
    public GameObject prefabMeshCreator;
    public GameObject node0;
    public GameObject node1;
    public GameObject node2;
    public GameObject node3;
    public GameObject node4;
    public GameObject node5;
    public GameObject node6;
    public GameObject nodeCol;


    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;

    private int splineNumber;
    private int vertexNumber;
    private int vertexAction;
    private int vertexPosition;
    private int NumSplines = 1;


    // counters
    private int cycleCount;
    private int updateCount;
    private int splineCount;
    

    private List<float> vertexX;
    private List<float> vertexY;
    private List<GameObject> bezierPrefabs;
    private List<GameObject> meshPrefabs;
    private List<Bezier> bezierScripts;
    private List<Material> materials;
    private List<MeshCreator> meshScripts;

    public void Start()
    {
        cycleCount = 0;
        
        bezierPrefabs = new List<GameObject>();
        bezierScripts = new List<Bezier>();

        meshPrefabs = new List<GameObject>();
        meshScripts = new List<MeshCreator>();
        splineCount = 0;

        // make new bezier prefab
        materials = new List<Material>();
        materials.Add(material1);
        materials.Add(material2);
        materials.Add(material3);
        materials.Add(material4);
        materials.Add(material5);
        
        node0.gameObject.transform.hasChanged = false;
        node1.gameObject.transform.hasChanged = false;
        node2.gameObject.transform.hasChanged = false;
        node3.gameObject.transform.hasChanged = false;
        node4.gameObject.transform.hasChanged = false;
        node5.gameObject.transform.hasChanged = false;
        node6.gameObject.transform.hasChanged = false;
        nodeCol.gameObject.transform.hasChanged = false;
        Reset();
        
    }
    

    public void Update()
    {
        if (node0.gameObject.transform.hasChanged)
        {
            ProcessMidi(0);

            node0.gameObject.transform.hasChanged = false;
        }if (Input.GetKeyDown("space"))
        {
            ProcessMidi(0);

            node0.gameObject.transform.hasChanged = false;
        }
        if (node1.gameObject.transform.hasChanged)
        {
            ProcessMidi(1);

            node1.gameObject.transform.hasChanged = false;
        }
        if (node2.gameObject.transform.hasChanged)
        {
            
            ProcessMidi(2);
            node2.gameObject.transform.hasChanged = false;
        }
        if (node3.gameObject.transform.hasChanged)
        {
            ProcessMidi(3);
            node3.gameObject.transform.hasChanged = false;
        }
        if (node4.gameObject.transform.hasChanged)
        {
            ProcessMidi(4);
            
            node4.gameObject.transform.hasChanged = false;
        }
        if (node5.gameObject.transform.hasChanged)
        {
            
            ProcessMidi(5);
            
            node5.gameObject.transform.hasChanged = false;
        }if (node6.gameObject.transform.hasChanged)
        {
            
            ProcessMidi(6);
            
            node6.gameObject.transform.hasChanged = false;
        }
        if (nodeCol.gameObject.transform.hasChanged)
        {
            splineCount++;

            var col = splineCount % 5;

            bezierPrefabs.Add(GameObject.Instantiate(prefabBezier));
            meshPrefabs.Add(GameObject.Instantiate(prefabMeshCreator, new Vector3(0,0,splineCount*-0.1f), new Quaternion()));
            meshPrefabs[splineCount].GetComponent<Renderer>().material = materials[col];


            bezierScripts.Add(bezierPrefabs[splineCount].GetComponent<Bezier>());
            meshScripts.Add(meshPrefabs[splineCount].GetComponent<MeshCreator>());
            bezierScripts[splineCount].ResetScript();
            bezierScripts[splineCount].ModifySpline(vertexNumber, vertexAction, 1f);

            meshPrefabs[splineCount].transform.Translate(new Vector3(UnityEngine.Random.Range(-14f,14f),UnityEngine.Random.Range(-8f,8f),0f));
            nodeCol.gameObject.transform.hasChanged = false;
        }

    }

    public void ProcessMidi(int vertexNumber)
    {
        updateCount++;

        var amount = 15f; //10f - splineCount*0.2f; 
        var vertices = bezierScripts[splineCount].ModifySpline(vertexNumber, 0, amount);
        
        vertexX = vertices.Item1;
        vertexY = vertices.Item2;
        Vector2[] vertArray1 = vertices.Item4;

        meshScripts[splineCount].MakeMesh(vertArray1);
    }
    

    public void Reset()
    {
        if (bezierPrefabs.Count > 0)
        {
            for (int i = 0; i < bezierPrefabs.Count; i++)
            {
                Destroy(bezierPrefabs[i]);
                Destroy(meshPrefabs[i]);
                Destroy(bezierScripts[i]);
                Destroy(meshScripts[i]);
            }
            bezierPrefabs.Clear();
            meshPrefabs.Clear();
            bezierScripts.Clear();
            meshScripts.Clear();
        }

        for (int n = 0; n < NumSplines; n++)
        {
            bezierPrefabs.Add(GameObject.Instantiate(prefabBezier));
            meshPrefabs.Add(GameObject.Instantiate(prefabMeshCreator, new Vector3(0,n*5,n), new Quaternion()));
            bezierScripts.Add(bezierPrefabs[n].GetComponent<Bezier>());

            meshScripts.Add(meshPrefabs[n].GetComponent<MeshCreator>());
            meshPrefabs[n].GetComponent<Renderer>().material = materials[n];
            bezierScripts[n].ResetScript();
        }
        
        cycleCount++;
    }
}


