using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private List<Collider2D> collisions;
    SquareBehaviour parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<SquareBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("Something");
        parent.AttackHitboxEnter(collider);
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        print("Something else");
        parent.AttackHitboxExit(collider);
    }
}
