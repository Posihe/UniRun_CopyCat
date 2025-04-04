
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{

    int hp;
    public int jumpcount = 2;
    public float pressTime;
    public Transform start;
    public Transform target;
    public float speed = 5;
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
    private SpriteRenderer SpriteRenderer;
    private float rollingTime = 1.0f; // 1초 동안 기다림
    private int tapCount = 0; // 엔터 키 입력 횟수

    public float doubleTapTime = 0.3f;

    private float horizontalInput;

    private float lastTapTime = -1f;
    private int lastDirection = 0;
    private bool boostReady = false;
    private bool isBoosted = false;

    public GameObject coinpreFab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isRollingReady = false;
      
        isDead = false;
        playerrigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        isRoling = false;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        hp = GameManager.instance.score;
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
        // 수평 입력 값 가져오기 (왼쪽: -1, 오른쪽: 1, 입력 없음: 0)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 플레이어의 방향 전환 (왼쪽으로 가면 flipX를 true로 설정)
        SpriteRenderer.flipX = horizontalInput < 0 ? true : false;

        // 입력이 있을 경우 걷기 애니메이션 활성화
        if (horizontalInput != 0)
        {
            anim.SetBool("Walk", true);
        }

        // "Horizontal" 버튼이 눌렸을 때(좌/우 키 입력 시작 시) 실행
        if (Input.GetButtonDown("Horizontal"))
        {
            int currentDirection = (int)Mathf.Sign(horizontalInput); // 현재 입력 방향 (왼쪽 -1, 오른쪽 1)

            // 현재 입력 방향이 이전 입력 방향과 동일하고, 마지막 입력 후 일정 시간 내라면 부스트 준비 상태로 설정
            if (currentDirection == lastDirection && Time.time - lastTapTime <= doubleTapTime)
            {
                boostReady = true; // 부스트 가능
            }
            else
            {
                boostReady = false; // 부스트 불가능
            }

            // 현재 시간과 방향을 저장 (다음 입력과 비교하기 위해)
            lastTapTime = Time.time;
            lastDirection = currentDirection;
        }

        // 부스트 실행 조건:
        // 1. boostReady가 true여야 함 (더블 탭 감지됨)
        // 2. 입력 방향이 0이 아님
        // 3. 현재 입력 방향이 마지막 입력 방향과 동일해야 함
        if (boostReady && horizontalInput != 0 && (int)Mathf.Sign(horizontalInput) == lastDirection)
        {
            isBoosted = true; // 부스트 활성화
        }
        else if (horizontalInput == 0) // 입력이 없을 경우
        {
            isBoosted = false; // 부스트 비활성화
            boostReady = false; // 부스트 준비 상태 해제
            anim.SetBool("Run", false); // 달리기 애니메이션 비활성화
            anim.SetBool("Walk", false); // 걷기 애니메이션 비활성화
        }
        else if ((int)Mathf.Sign(horizontalInput) != lastDirection) // 입력 방향이 바뀌었을 경우
        {
            isBoosted = false; // 부스트 비활성화
            boostReady = false; // 부스트 준비 상태 해제
        }

        // 이동 속도 설정 (부스트 시 2배 속도)
        float moveSpeed = isBoosted ? speed * 2 : speed;

        // 부스트 상태면 달리기 애니메이션 활성화
        anim.SetBool("Run", isBoosted);

        // 플레이어 이동 처리 (오른쪽 벡터 * 방향 입력 * 속도 * 프레임 시간)
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BossRoom"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene("Boss");
            }
        }
    }
    private void Rolling()
    {
       
        if (isRoling) return; // 이미 구르기 중이면 무시

        if (Input.GetKeyDown(KeyCode.Return))
        {
            tapCount++; // 엔터 키 입력 횟수 증가
            lastTapTime = Time.time; // 마지막 입력 시간 업데이트

            // Rolling 애니메이션 실행
            anim.SetBool("Rolling", true);
        }

        // 마지막 입력 후 1초가 지나면 이동 시작
        if (!isRoling && tapCount > 0 && Time.time - lastTapTime >= doubleTapTime)
        {
            StartCoroutine(StartRolling());
        }
    }
 // 코루틴: 1초 후 입력된 횟수에 비례하여 이동
    private IEnumerator StartRolling()
    {

        isRoling = true; // 구르기 시작

        // 이동 거리 = 기본 속도 * 입력 횟수
        float moveDistance = speed * tapCount;

        // 오른쪽 방향으로 순간적인 힘 적용 (Impulse 사용)
        playerrigidbody.AddForce(Vector2.right * moveDistance, ForceMode2D.Impulse);

        // 입력 횟수 초기화
        tapCount = 0;

        // Rolling 애니메이션을 0.5초 후에 종료
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Rolling", false);
        isRoling = false;
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
    public void Hit()
    {
        float randomvalue = Random.Range(0, 3);
       
        
            for (int i = 1; i < hp; i++)
            {
                for(int j=1; j<360; j++)
                Instantiate(coinpreFab, transform.position, Quaternion.Euler(0,0,j));



            }
         GameManager.instance.lossCoin();
       
    }


}
