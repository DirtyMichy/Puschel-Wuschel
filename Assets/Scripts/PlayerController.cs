using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerController : MonoBehaviour
{
	public bool facingRight = true;
	public bool jump = true;
    private bool alive = true;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform[] groundCheck;
	//public float h;
	public Camera cam;
    public int powerUpCount = 0;
    public int playerID = 0;
    public bool powerUpActivated = false;

    public bool isLeader = false;
    public bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

    GamepadInput.GamePad.Index[] gamePadIndex;

    public GameObject Balloons;
    public GameObject Body;

    public void SetPlayerID(int i)
    {
        playerID = i;
        if (i == 0)
            cam = Camera.main;
    }

    // Use this for initialization
	void Start ()
    {
        cam = Camera.main;

        anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
        
        gamePadIndex = new GamepadInput.GamePad.Index[4];
        gamePadIndex[0] = GamePad.Index.One;
        gamePadIndex[1] = GamePad.Index.Two;
        gamePadIndex[2] = GamePad.Index.Three;
        gamePadIndex[3] = GamePad.Index.Four;
    }
	
	// Update is called once per frame
	void Update ()
    {

        for(int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer("Ground")))
                grounded = true;
        }

        //Powerup
        if ((Input.GetKeyDown(KeyCode.X) &&  playerID == 0) || GamePad.GetButton(GamePad.Button.X, gamePadIndex[playerID]))
        {
            if(powerUpCount >= 1 && !powerUpActivated)
            StartCoroutine(powerUp());
        }

		//if (Input.GetButton ("Jump") && grounded)
        if ((((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) &&  playerID == 0) || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])) && alive && grounded)
        {
			jump = true;
            GetComponent<AudioSource>().Play();
		}

        grounded = false;
        /*
		if (isLeader)
        {
            float x = transform.position.x;
            cam.transform.position = new Vector3(x,0f,-10f);
		}
        */
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        float rightestPos = 0;
        int leader = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.position.x > rightestPos && players[i].transform.position.x > 0)
            {
                rightestPos = players[i].transform.position.x;
                leader = i;
            }
        }
        /*
        if ((players[leader].transform.position.x - transform.position.x) >= 14)
        {
            transform.position = players[leader].transform.position;
        }
*/
        if(powerUpActivated)
        {
            GetComponent<Rigidbody2D>().angularVelocity = 640f*-transform.localScale.x ;
        }
    }

    IEnumerator powerUp()
    {
        powerUpActivated = true;
        GetComponent<Rigidbody2D>().freezeRotation=false;
        GetComponent<Rigidbody2D>().mass = 100f ;
        while(powerUpCount > 0)
        {        
            yield return new WaitForSeconds(1f);
            powerUpCount--;
        }
        powerUpActivated = false;
        GetComponent<Rigidbody2D>().freezeRotation=true;
        GetComponent<Rigidbody2D>().mass = 1f ;
        transform.rotation=new Quaternion(0f,0f,0f,0f);
        Body.transform.localScale=new Vector3(1f,1f,1f);
    }

    void FixedUpdate()
    {
        if (alive)
        {
		//h = Input.GetAxis ("Horizontal");

        Vector2 directionCurrent = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && playerID == 0)
            directionCurrent.x = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && playerID == 0)
            directionCurrent.x = 1f;

        //		anim.SetFloat("Speed", Mathf.Abs(h));
        //Debug.Log(directionCurrent.x);
        if (directionCurrent.x * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * directionCurrent.x * moveForce);

		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		
		if (directionCurrent.x > 0 && !facingRight)
			Flip ();
		else if (directionCurrent.x < 0 && facingRight)
			Flip ();
		
		if (jump)
		{
//			anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce*GetComponent<Rigidbody2D>().mass));
			jump = false;
		}

		if (directionCurrent.x != 0) {
			anim.SetBool ("isWalking", true);
		}else{
			anim.SetBool ("isWalking", false);
		}

        }
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (alive && c.tag == "Enemy")
        {
            alive = false;
            StartCoroutine(Die());
        }        
    }

    public IEnumerator Die()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[1].Play();
        SpriteRenderer[] childComps = GetComponentsInChildren<SpriteRenderer>();        
        for(int i = 255; i > 0; i-=4)
        {
            for(int j = 0; j < childComps.Length; j++)
            {
                childComps[j].GetComponent<SpriteRenderer>().color = new Color(1f,0f,0f,i/255f); 
            }
            yield return new WaitForSeconds(.0001f);
        }
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    /*
    public IEnumerator BalloonFly()
    {
        GetComponent<Rigidbody2D>().gravityScale = - 0.5f;
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody2D>().gravityScale = 8f;
    }
    */
}