using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour {

    public GameObject currentTarget = null;
    public float rotationSpeed = 10f;
    public float range = 32f;
    public float speed = 1f;
    public bool canKill = true;
    
    // Update is called once per frame
    void Awake()
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
            if((currentTarget.transform.position.x > transform.position.x &&  theScale.y<=0) || (currentTarget.transform.position.x < transform.position.x && theScale.y>0))
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

    void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log("Collision");
        if(c.gameObject.tag == "Player" && canKill)
            Destroy(gameObject);
        else
            canKill = false;
    }

    /*
    void Update()
    {
        Debug.Log("AngularVelocity: " +  GetComponent<Rigidbody2D>().angularVelocity);
    }
    */
}
