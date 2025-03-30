using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int jumpcount = 2;
    private Animator anim;
    private bool isRollingReady;
    private bool isGround;
    private bool isDead;
    private Rigidbody2D playerrigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isRollingReady = false;
        isGround = true;
        isDead = false;
        playerrigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Move();
            Down();
            Jump();
        }
    }

    public void Move()
    {
        isRollingReady = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float speed = 10;
        float xinput = Input.GetAxis("Horizontal");
        spriteRenderer.flipX = xinput < 0 ? true : false;
        if ((xinput > 0 || xinput < 0)) 
        {
            anim.SetBool("Walk", true);
            transform.Translate(Vector2.right * xinput * speed * Time.deltaTime);
        }
        else { anim.SetBool("Walk", false); }
        
        


    }

    public void Down()
    {

        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("Down", true);
            isRollingReady = true;

        }
        else
        {
            anim.SetBool("Down", false);
            isRollingReady = false;

        }
        

    }

    public void Jump()
    {
        float jumpForce = 400;
        float yinput = Input.GetAxis("Vertical");
       
     if(Input.GetKeyDown(KeyCode.Space) &&jumpcount>0)
        {
            anim.SetBool("Jump",true);
            isGround = false;
            playerrigidbody.linearVelocity = Vector2.zero;
            playerrigidbody.AddForce(new Vector2(0,jumpForce));
            jumpcount--;
        
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            
            isGround = true;
            jumpcount = 2;
            anim.SetBool("Jump", false);
            

        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        // transform.Translate(gameObject.transform.position+=(new Vector2(0, 3)));
        playerrigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dead"))
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Die();
            StartCoroutine(Dead());
           
        }
    }

    IEnumerator Dead()
    {
        
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
