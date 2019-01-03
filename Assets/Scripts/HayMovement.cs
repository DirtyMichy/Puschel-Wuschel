using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMovement : MonoBehaviour
{

    [SerializeField]
    private float force = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WorldEnd")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f * force * Time.deltaTime, Random.Range(0.1f, 0.2f) * force * Time.deltaTime));
    }
}
