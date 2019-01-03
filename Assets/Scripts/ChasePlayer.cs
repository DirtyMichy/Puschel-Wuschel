using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    
    public GameObject currentTarget = null;
    public float rotationSpeed = 10f;
    public float range = 32f;
    public float speed = 1f;
  
    // Update is called once per frame
    void FixedUpdate()
    {        
        float minimalEnemyDistance = float.MaxValue;
        GameObject[] playerAlive = null;
        
        playerAlive = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in playerAlive)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            if (distance < minimalEnemyDistance && distance < range)
            {
                currentTarget = player;
                minimalEnemyDistance = distance;
            }
        }

        if (currentTarget != null)
        {
            Vector3 theScale = transform.localScale;
            if ((currentTarget.transform.position.x > transform.position.x && theScale.y <= 0) || (currentTarget.transform.position.x < transform.position.x && theScale.y > 0))
            {
                theScale.y *= -1;
                transform.localScale = theScale;
            }

            Vector2 point2Target = (Vector2)transform.position - (Vector2)currentTarget.transform.position;
            
            float value = Vector3.Cross(point2Target, transform.right).z;
            
            GetComponent<Rigidbody2D>().angularVelocity = 200f * value;
            
            GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }

    }
}
