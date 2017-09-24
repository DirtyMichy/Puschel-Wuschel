using UnityEngine;
using System.Collections;
using GamepadInput;

public class PlayerController : MonoBehaviour
{
	public bool facingRight = true;
	public bool jump = true;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform[] groundCheck;
	//public float h;
	public Camera cam;
    public int powerUpCount = 0;
    public int playerID = 0;

    public bool isLeader = false;
    public bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

    GamepadInput.GamePad.Index[] gamePadIndex;

    public GameObject Balloons;

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

		//if (Input.GetButton ("Jump") && grounded)
        if ((((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W)) &&  playerID == 0) || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])) && grounded)
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
        if ((players[leader].transform.position.x - transform.position.x) >= 14)
        {
            transform.position = players[leader].transform.position;
        }

    }

    void FixedUpdate()
    {
		//h = Input.GetAxis ("Horizontal");

        Vector2 directionCurrent = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);

        if (Input.GetKey(KeyCode.A) && playerID == 0)
            directionCurrent.x = -1f;
        if (Input.GetKey(KeyCode.D) && playerID == 0)
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
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}

		if (directionCurrent.x != 0) {
			anim.SetBool ("isWalking", true);
		}else{
			anim.SetBool ("isWalking", false);
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
    /*
    public IEnumerator BalloonFly()
    {
        GetComponent<Rigidbody2D>().gravityScale = - 0.5f;
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody2D>().gravityScale = 8f;
    }
    */
}