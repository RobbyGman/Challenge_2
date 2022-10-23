using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatController : MonoBehaviour
{
    private Rigidbody2D rd2d;
    float hozMovement;
    float verMovement;
    public float speed;
    public float jump;
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

    private bool isOnGround;
    public Transform goundcheck;
    public float checkRadius;
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
   }

   void Move(float dir)
   {
    float xVal = dir * speed * 100 * Time.deltaTime;
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


    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        Move(hozMovement);

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(goundcheck.position, checkRadius, allGround);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            WinTextObject.SetActive(true);

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
        else if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue = livesValue - 1;
            SetCountText();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            }
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
