using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRendererBehaviour : MonoBehaviour
{
    public static int Bounces = 0;

    // Start is called before the first frame update
    void Start()
    {
        Bounces = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {

        GUI.Label(new Rect(10, 10, 180, 20), "Corner Bounces: " + Bounces);
        GUI.Label(new Rect(10, 35, 100, 20), "FPS: " + 1 / Time.deltaTime);
    }
}
