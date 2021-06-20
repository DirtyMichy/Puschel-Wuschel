using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Vector3 lastPosition;                           // A position marking where to check if the player is grounded.

    public float k_GroundedRadius = .2f;    // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;                 // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;      // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;

    public float runSpeed = 40f;
    Vector2 directionCurrentNew;
    float horizontalMove = 0f;

    //######################################################

    public bool facingRight = true;
    public bool jump = true;
    public bool alive = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public float iceForce = 1f;
    public Transform[] groundCheck;

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
    public GameObject Name;
    public GameObject spawnParticles;

    bool directionRight = false;
    bool directionLeft = false;

    public void Awake()
    {
        //Check if we want our CowBoyHat
        Transform hat = transform.Find("CowBoyHat");
        if (hat != null)
        {
            if (Camera.main.GetComponent<LevelName>())
                if (Camera.main.GetComponent<LevelName>().levelType == "Western")
                    hat.GetComponent<SpriteRenderer>().enabled = true;
        }

        //cam = Camera.main;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        gamePadIndex = new GamepadInput.GamePad.Index[4];
        gamePadIndex[0] = GamePad.Index.One;
        gamePadIndex[1] = GamePad.Index.Two;
        gamePadIndex[2] = GamePad.Index.Three;
        gamePadIndex[3] = GamePad.Index.Four;

        //######################################################

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetPlayerID(int i)
    {
        playerID = i;
        //if (i == 0)
        //    cam = Camera.main;
    }

    void Update()
    {
        directionCurrentNew = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);
        if (Input.GetKey(KeyCode.S))
            directionCurrentNew.x = -1f;
        if (Input.GetKey(KeyCode.D))
            directionCurrentNew.x = 1f;
        //Jumping
        if ((((Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.UpArrow)))
            || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])))
        {
            jump = true;
            Debug.Log("Jump");
        }

        /*
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer("Ground")))
                grounded = true;
        }

        //Powerup
        if ((Input.GetKeyDown(KeyCode.X) && playerID == 0) || GamePad.GetButton(GamePad.Button.X, gamePadIndex[playerID]))
        {
            if (powerUpCount >= 1 && !powerUpActivated && !Balloons.activeSelf)
                StartCoroutine(powerUp());
        }

        //Jumping
        if ((((Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.UpArrow)) && playerID == 0)
            || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])) && alive && grounded)
        {
            jump = true;
        }

        grounded = false; //after jumping we check again inside the for loop if we are grounded
        */
        if (powerUpActivated)
        {
            rb2d.angularVelocity = 640f * -transform.localScale.x;
        }
    }

    IEnumerator powerUp()
    {
        powerUpActivated = true;
        rb2d.freezeRotation = false;
        rb2d.mass = 100f;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = false;

        while (powerUpCount > 0)
        {
            transform.Find("PowerUpText").GetComponent<TextMesh>().text = "";
            yield return new WaitForSeconds(1f);
            powerUpCount--;
        }

        powerUpActivated = false;
        rb2d.freezeRotation = true;
        rb2d.mass = 1f;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Body.transform.localScale = new Vector3(1f, 1f, 1f);

        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void GoRight()
    {
        directionLeft = false;
        directionRight = true;
    }

    public void GoLeft()
    {
        directionRight = false;
        directionLeft = true;
    }

    void FixedUpdate()
    {



        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        Move(directionCurrentNew.x * runSpeed * Time.fixedDeltaTime, jump);
        jump = false;

        //######################################################
        /*
        if (alive)
        {
            Vector2 directionCurrent = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);

            if ((Input.GetKey(KeyCode.S) || directionLeft || Input.GetKey(KeyCode.LeftArrow)) && playerID == 0)
                directionCurrent.x = -1f;
            if ((Input.GetKey(KeyCode.D) || directionRight || Input.GetKey(KeyCode.RightArrow)) && playerID == 0)
                directionCurrent.x = 1f;
			
            if (directionCurrent.x * rb2d.velocity.x < maxSpeed)
                rb2d.AddForce(Vector2.right * directionCurrent.x * moveForce * iceForce);

            //rb2d.AddForce(Vector2.up * moveForce/4f *  -1f);

            if (directionCurrent.x > 0 && !facingRight)
                Flip();
            else if (directionCurrent.x < 0 && facingRight)
                Flip();

            if (jump)
            {
                GetComponent<AudioSource>().Play();
                grounded = false;

                rb2d.AddForce(new Vector2(0f, jumpForce));
                //rb2d.velocity = new Vector2(0f, 0f);                            //resetting the velocity so old value wont be used (no flickery jumping)
                jump = false;
            }

            if (directionCurrent.x != 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            directionRight = false;
        }
        */
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (alive && ((c.tag == "Enemy" && !powerUpActivated) || c.tag == "KillZone"))
        {
            if (c.gameObject.GetComponent<Enemy>())
                if (c.gameObject.GetComponent<Enemy>().resetPositionAfterKill)
                {
                    c.gameObject.GetComponent<Enemy>().SpawnParticle();
                    c.gameObject.transform.position += new Vector3(16f, 0f, 0f);
                }
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        alive = false;
        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[1].Play();
        SpriteRenderer[] childComps = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 255; i > 0; i -= 4)
        {
            for (int j = 0; j < childComps.Length; j++)
            {
                childComps[j].GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, i / 255f);
            }
            yield return new WaitForSeconds(.0001f);
        }
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    public void Move(float move, bool jump)
    {
        Debug.Log(move + " + " + jump);
        //only control the player if grounded or airControl is turned on
        if ((m_Grounded || (m_AirControl && lastPosition.x != transform.position.x)))
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            if (move != 0)
                anim.SetBool("isWalking", true);
            else
                anim.SetBool("isWalking", false);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            GetComponent<AudioSource>().Play();
        }

        lastPosition = transform.position;
    }
}