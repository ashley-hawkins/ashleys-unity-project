using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    GameObject character;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Player/Character");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var threshold = 1.0f;
        var positionDiff = character.transform.position.x - transform.position.x;

        if (positionDiff > threshold)
        {
            // go right
            rb.AddForce(new Vector2(1, 0) * 12000 * Time.deltaTime);
            transform.localScale = new Vector2(1, 1);
        }
        else if (positionDiff < -threshold)
        {
            // go left
            rb.AddForce(new Vector2(-1, 0) * 12000 * Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
