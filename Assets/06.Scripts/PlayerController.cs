
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int jumpcount = 2;
    private Animator anim;
    private bool isRollingReady;
    private bool isGround;
    private bool isDead;
    bool isInTunnel = false;
    private Rigidbody2D playerrigidbody;
    private AudioSource playerAudio;
    public AudioClip jumpClip;
    public AudioClip deadClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isRollingReady = false;
        isGround = true;
        isDead = false;
        playerrigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();

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
        if (isInTunnel && Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Bonus());
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
            playerAudio.clip = jumpClip;
            playerAudio.Play();
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
        if(collision.gameObject.CompareTag("Tunnel")&&Input.GetKeyDown(KeyCode.DownArrow))
        {
          
            isInTunnel = true; 

        }

       
    }
   

    public void Die()
    {
        playerAudio.clip = deadClip;
        playerAudio.Play();
        isDead = true;
        anim.SetTrigger("Dead");
       
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

    IEnumerator Bonus()
    {
        anim.SetBool("Bonus", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("Bonus", false);

    }



}
