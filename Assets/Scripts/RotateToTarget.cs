using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour {

    public GameObject currentTarget = null;
    public float rotationSpeed = 10f;
    public float range = 32f;
    public float speed = 1f;
	
	// Update is called once per frame
	void Update ()
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

        float value = Vector3.Cross(point2Target, transform.right).z;

        GetComponent<Rigidbody2D>().angularVelocity = 200f * value;

    }
}
