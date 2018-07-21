using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowManShooting : MonoBehaviour
{
    public GameObject currentTarget = null;
    public float rotationSpeed = 10f;
    public float range = 8f;
    public float shotDelay = 1f;
    public bool shooting = false;
    public GameObject bullet;

    Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float minimalEnemyDistance = float.MaxValue;
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            if (distance < minimalEnemyDistance && distance < range)
            {
                currentTarget = player;
                minimalEnemyDistance = distance;
            }
        }
        Vector2 point2Target = transform.position;
        
        if (currentTarget != null)
            point2Target = (Vector2)transform.position - (Vector2)currentTarget.transform.position;

        if(currentTarget != null && !shooting)
            StartCoroutine("Shoot");
    }

    //Coroutine for AI shooting, enemy and friendly
    IEnumerator Shoot()
    {
        shooting = true;

//            Debug.Log("Shooting");

            //If there is an acompanying audio, play it
            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
            
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(1f);
            
            Instantiate(bullet, transform.position, transform.rotation);

            //Wait for it to be time to fire another shot
            yield return new WaitForSeconds(shotDelay);
        
        shooting = false;
    }
}