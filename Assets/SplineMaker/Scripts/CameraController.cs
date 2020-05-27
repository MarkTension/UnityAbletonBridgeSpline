using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera targetCamera;
    public Camera stateCamera;
    public Shader greyscaleShader;
    
    // Start is called before the first frame update
    void Start()
    {
        targetCamera.SetReplacementShader(greyscaleShader, null);
        stateCamera.SetReplacementShader(greyscaleShader, null);
    }


}
