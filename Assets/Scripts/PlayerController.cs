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
	public Transform groundCheck;
	//public float h;
	public Camera cam;
    public int powerUpCount = 0;
    public int playerID = 0;

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
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		//if (Input.GetButton ("Jump") && grounded)
        if ((Input.GetKeyDown(KeyCode.Space) || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])) && grounded)
        {
			jump = true;
            GetComponent<AudioSource>().Play();
		}

		float x = transform.position.x;
		if (x > 0) {		
			cam.transform.position = new Vector3(x,0f,-10f);
		}
	}

	void FixedUpdate()
    {
		//h = Input.GetAxis ("Horizontal");

        Vector2 directionCurrent = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);

        if (Input.GetKey(KeyCode.A))
            directionCurrent.x = -1f;
        if (Input.GetKey(KeyCode.D))
            directionCurrent.x = 1f;
        
        //		anim.SetFloat("Speed", Mathf.Abs(h));
        Debug.Log(directionCurrent.x);
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
    /*
    public IEnumerator BalloonFly()
    {
        GetComponent<Rigidbody2D>().gravityScale = - 0.5f;
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody2D>().gravityScale = 8f;
    }
    */
}