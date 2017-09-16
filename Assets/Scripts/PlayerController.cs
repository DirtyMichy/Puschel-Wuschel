using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool facingRight = true;
	public bool jump = true;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public float h;
	public Camera cam;
    public int powerUpCount = 0;

    public bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

    public GameObject Balloons;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		if (Input.GetButton ("Jump") && grounded)
        {
			jump = true;
            GetComponent<AudioSource>().Play();
		}

		float x = transform.position.x;
		if (x > 0) {		
			cam.transform.position = new Vector3(x,0f,-10f);
		}
	}

	void FixedUpdate(){

		h = Input.GetAxis ("Horizontal");

//		anim.SetFloat("Speed", Mathf.Abs(h));
		
		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);

		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
		
		if (jump)
		{
//			anim.SetTrigger("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}

		if (h != 0) {
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