using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour {

    public GameObject currentTarget = null;
    public float rotationSpeed = 10f;
    public float range = 32f;
    public float speed = 1f;
    public float lifeTime = 2f;

    // Update is called once per frame
    void Awake()
    {        
        iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(1f,1f,1f), "time", 1f));

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

        //Snowballs cant hit enemies
        if (c.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c.gameObject.GetComponent<Collider2D>());
        }
        else
        {
            if (c.gameObject.tag == "Player")
            {
                if(c.gameObject.GetComponent<PlayerController>().alive)
                {                
                    Invoke ("Die", 1f);
                }
            }
            else
            {
                //if it doesnt hit a character, it can disappear
                Invoke ("PrepareToDie", lifeTime);   
            }

            gameObject.tag = "Untagged";
        }
    }

    void PrepareToDie()
    {       
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0f,0f,0f), "time", 1f));
        Invoke ("Die", 1f);

    }

    void Die()
    {
        Destroy(gameObject);
    }

    /*
    void Update()
    {
        Debug.Log("AngularVelocity: " +  GetComponent<Rigidbody2D>().angularVelocity);
    }
    */
}
