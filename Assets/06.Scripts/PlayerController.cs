
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public int jumpcount = 2;
    public float pressTime;
    public Transform start;
    public Transform target;
    public float speed = 10;
    private Animator anim;
    public bool isRollingReady;
    public bool isGround;
    private bool isDead;
    public bool isRoling;
   public bool isInTunnel = false;
    private Rigidbody2D playerrigidbody;
    private AudioSource playerAudio;
    public AudioClip jumpClip;
    public AudioClip deadClip;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isRollingReady = false;
      
        isDead = false;
        playerrigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        isRoling = false;
      

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Move();
            Down();
            Jump();
            Rolling();
        }
        

          if(isInTunnel==true &&Input.GetKeyDown(KeyCode.DownArrow ))
        {
            StartCoroutine(InTunnel());

        }
           

        
    }

    public void Move()
    {
        isRollingReady = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
       
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

        if (Input.GetKey(KeyCode.DownArrow)&&isInTunnel==false && isGround==true)
        {
            anim.SetBool("Down", true);
            isRollingReady = true;

        }
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {

            anim.SetBool("Down", false);

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
        

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;


        }

        if (collision.gameObject.CompareTag("Tunnel") )
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
        StartCoroutine(Dead());
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

        if (collision.gameObject.CompareTag("Potal"))
        {

            SceneManager.LoadScene(1);

        }

    }



    private void Rolling()
    {
        if (isRollingReady && Input.GetKey(KeyCode.Return))
        {
            pressTime += Time.deltaTime; // 키를 누르는 동안 pressTime 증가
            anim.SetBool("Rolling", true);
            isRoling = true;

            if (Input.GetKeyUp(KeyCode.Return))
            {
                transform.Translate(Vector2.right * pressTime * speed); // 최종 이동
                anim.SetBool("Rolling", false);
                pressTime = 0f; // pressTime 초기화
            }
        }
    }


    IEnumerator Dead()
    {
        
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

   IEnumerator InTunnel()
    {
        GameObject tunnelObject = GameObject.FindWithTag("Tunnel");
        tunnelObject.GetComponent<BoxCollider2D>().enabled = false;

       
        
        yield return new WaitForSeconds(1);
        tunnelObject.GetComponent<BoxCollider2D>().enabled = true;


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false; // 플레이어가 땅에서 떨어지면 즉시 isGround = false
        }
    }

   

}
