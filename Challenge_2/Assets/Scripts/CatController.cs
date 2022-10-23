using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatController : MonoBehaviour
{
    Rigidbody2D rd2d;
    float hozMovement;
    float verMovement;

    [SerializeField] public float speed = 3;
    public float jump;
    bool isjumping;
    public Animator anim;
    float runSpeedModifier = 2f;
    bool isRunning = false;
    private bool facingRight = true;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;

    private int count;
    private int livesValue;

    public GameObject WinTextObject;
    public GameObject LoseTextObject;

    private bool isOnGround = false;
    [SerializeField] public Transform groundCheckCollider;
    public float checkRadius = .2f;
    public LayerMask allGround;



    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        count = 0;

        rd2d = GetComponent<Rigidbody2D>();
        livesValue = 3;

        SetCountText();
        WinTextObject.SetActive(false);
       
        SetCountText();
        LoseTextObject.SetActive(false);
        
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
   void Update()
   {
    float hozMovement = Input.GetAxis("Horizontal");
    float verMovement = Input.GetAxis("Vertical");

    if(Input.GetKeyDown(KeyCode.LeftShift))
    {
        isRunning = true;
    }
    if(Input.GetKeyUp(KeyCode.LeftShift))
    {
        isRunning = false;
    }

    if(Input.GetKeyDown(KeyCode.Space))
    {
        isjumping = true;
    }
    else if (Input.GetKeyUp(KeyCode.Space))
    {
        isjumping = false;
    }
   }

   void Move(float dir, bool jumpFlag)
   {
    if (isOnGround && jumpFlag)
    {
        isOnGround = false;
        jumpFlag = false;

        rd2d.AddForce(new Vector2(0f, jump));
    }
    float xVal = dir * speed * 100 * Time.fixedDeltaTime;
    if (isRunning)
    {
        xVal *=runSpeedModifier;
    }
    Vector2 targetVelocity = new Vector2(xVal,rd2d.velocity.y);
    rd2d.velocity = targetVelocity;
   
    if (facingRight && dir < 0)
        {
            Flip();
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            Flip();
            facingRight = true;
        }
        anim.SetFloat("xVelocity", Mathf.Abs(rd2d.velocity.x));
   }
   void GroundCheck()
   {
     isOnGround = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, checkRadius, allGround);
              if(colliders.Length > 0)
                   isOnGround = true;
   }


    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        Move(hozMovement, isjumping);

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            WinTextObject.SetActive(true);
            Destroy(gameObject);

            MusicControleScript.PlaySound("Win");
        }

        countText.text = "Count: " + count.ToString();
        if (count == 4)
        {
            livesValue = 3;
            transform.position = new Vector2(100f, 0.5f);
        }

        livesText.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0)
        {
            LoseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            count = count +1;
            SetCountText();
        }

        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue = livesValue - 1;

            SetCountText();
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

}
