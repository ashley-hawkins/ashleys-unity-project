using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoBounceBehaviour : MonoBehaviour
{
    // Speed metres per second in either direction
    public float Speed = 1.0f;
    // Main camera
    private Camera cam;
    // Current velocity
    private Vector2 vel;
    private BoxCollider2D bCol;

    // true if the DVD logo was in the corner on the last frame, always false on the first frame.
    private bool wasInCorner;

    // Start is called before the first frame update
    void Start()
    {
        wasInCorner = false;
        cam = Camera.main;
        bCol = GetComponent<BoxCollider2D>();

        // I think this is probably more understandable than 'vel = new(1, -1)'
        vel = Vector2.right + Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        int hits = 0;
        Vector2 rightTop = cam.WorldToViewportPoint(bCol.bounds.max);
        Vector2 leftBottom = cam.WorldToViewportPoint(bCol.bounds.min);

        if (leftBottom.x <= 0.0f)
        {
            vel.x = 1;
            ++hits;
        }
        else if (rightTop.x >= 1.0f)
        {
            vel.x = -1;
            ++hits;
        }
        if (leftBottom.y <= 0.0f)
        {
            vel.y = 1;
            ++hits;
        }
        else if (rightTop.y >= 1.0f)
        {
            vel.y = -1;
            ++hits;
        }

        {
            bool nowInCorner = hits > 1;

            if (nowInCorner && !wasInCorner)
            {
                ScoreRendererBehaviour.Bounces += 1;
            }
            wasInCorner = nowInCorner;
        }

        transform.position += (Vector3)(Speed * vel * Time.deltaTime);
    }
}
