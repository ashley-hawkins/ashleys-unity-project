using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRendererBehaviour : MonoBehaviour
{
    public static int Bounces = 0;

    private Queue<float> fpsAvgList = new(new float[200]);
    float fpsAvg = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        fpsAvgList = new(new float[200]);
        Bounces = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float currentFps = 1 / Time.deltaTime;
        fpsAvg -= fpsAvgList.Dequeue() / 200.0f;
        fpsAvg += (currentFps / 200.0f);
        fpsAvgList.Enqueue(currentFps);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 180, 20), "Corner Bounces: " + Bounces);
        GUI.Label(new Rect(10, 35, 100, 20), "FPS: " + fpsAvg);
    }
}
