using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatController : MonoBehaviour
{
Animator anim;
private Rigidbody2D rd2d;

private int count;
public float speed;
public TextMeshProUGUI countText;
public GameObject WinTextObject;
public float jump;

private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        count = 0;
        SetCountText();
        WinTextObject.SetActive(false);
        
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 3);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 4);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            anim.SetInteger("State", 0);
        }
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
            
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 4)
        {
            WinTextObject.SetActive(true);
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

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
