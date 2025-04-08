using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;



public class PlayerController : MonoBehaviour
{

    //int hp;
    public int jumpcount = 2;
    public int mjumpcount = 2;
    public float jumpForce = 400;
    public float pressTime;
    public int hp;
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
    private float rollingTime = 1.0f; // 1�� ���� ��ٸ�
    private int tapCount = 0; // ���� Ű �Է� Ƚ��

    public float doubleTapTime = 0.3f;

    private float horizontalInput;

    private float lastTapTime = -1f;
    private int lastDirection = 0;
    private bool boostReady = false;
    private bool isBoosted = false;
    public bool isNothit = true;

    public GameObject coinpreFab;
    //����Ͽ�
    private bool isRightPressed = false;
    private bool isLeftPressed = false;
    private bool isJumpPressed = false;
    private bool isMrollingready = false;
    public bool isMrolling = false;
    private bool isReturn = false;
    private bool mRun = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        isRollingReady = false;

        isDead = false;
        playerrigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        isRoling = false;
        isGround = true;

        SpriteRenderer = GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {

            isReturn = true;

        }


        Move();
        Down();
        if (!isRollingReady)
        {
            Jump();
        }
        Rolling();





        // �������� ��ư �Է� ���� ����
        if (isRightPressed)
        {
            MoveRight();
            if (isReturn)
            {
                anim.SetBool("Run", true);
                transform.Translate(Vector2.right * (speed * 2) * Time.deltaTime);

            }
        }
        else if (isLeftPressed)
        {
            MoveLeft();
            if (isReturn)
            {
                anim.SetBool("Run", true);
                transform.Translate(Vector2.left * (speed * 2) * Time.deltaTime);

            }
        }
        if (isJumpPressed)
        {
            Mjump();
            isJumpPressed = false;

        }

        if (isMrollingready && isReturn)
        {
            Mrolling();


        }


        if (isInTunnel == true && Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(InTunnel());

        }

        hp = GameManager.instance.score;


    }

    public void Move()
    {
        // ���� �Է� �� �������� (����: -1, ������: 1, �Է� ����: 0)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // �÷��̾��� ���� ��ȯ (�������� ���� flipX�� true�� ����)
        SpriteRenderer.flipX = horizontalInput < 0 ? true : false;

        // �Է��� ���� ��� �ȱ� �ִϸ��̼� Ȱ��ȭ
        if (horizontalInput != 0)
        {
            anim.SetBool("Walk", true);
        }

        // "Horizontal" ��ư�� ������ ��(��/�� Ű �Է� ���� ��) ����
        if (Input.GetButtonDown("Horizontal"))
        {
            int currentDirection = (int)Mathf.Sign(horizontalInput); // ���� �Է� ���� (���� -1, ������ 1)

            // ���� �Է� ������ ���� �Է� ����� �����ϰ�, ������ �Է� �� ���� �ð� ����� �ν�Ʈ �غ� ���·� ����
            if (currentDirection == lastDirection && Time.time - lastTapTime <= doubleTapTime)
            {
                boostReady = true; // �ν�Ʈ ����
            }
            else
            {
                boostReady = false; // �ν�Ʈ �Ұ���
            }

            // ���� �ð��� ������ ���� (���� �Է°� ���ϱ� ����)
            lastTapTime = Time.time;
            lastDirection = currentDirection;
        }

        // �ν�Ʈ ���� ����:
        // 1. boostReady�� true���� �� (���� �� ������)
        // 2. �Է� ������ 0�� �ƴ�
        // 3. ���� �Է� ������ ������ �Է� ����� �����ؾ� ��
        if (boostReady && horizontalInput != 0 && (int)Mathf.Sign(horizontalInput) == lastDirection)
        {
            isBoosted = true; // �ν�Ʈ Ȱ��ȭ
        }
        else if (horizontalInput == 0) // �Է��� ���� ���
        {
            isBoosted = false; // �ν�Ʈ ��Ȱ��ȭ
            boostReady = false; // �ν�Ʈ �غ� ���� ����
            anim.SetBool("Run", false); // �޸��� �ִϸ��̼� ��Ȱ��ȭ
            anim.SetBool("Walk", false); // �ȱ� �ִϸ��̼� ��Ȱ��ȭ
        }
        else if ((int)Mathf.Sign(horizontalInput) != lastDirection) // �Է� ������ �ٲ���� ���
        {
            isBoosted = false; // �ν�Ʈ ��Ȱ��ȭ
            boostReady = false; // �ν�Ʈ �غ� ���� ����
        }

        // �̵� �ӵ� ���� (�ν�Ʈ �� 2�� �ӵ�)
        float moveSpeed = isBoosted ? speed * 2 : speed;

        // �ν�Ʈ ���¸� �޸��� �ִϸ��̼� Ȱ��ȭ
        anim.SetBool("Run", isBoosted);

        // �÷��̾� �̵� ó�� (������ ���� * ���� �Է� * �ӵ� * ������ �ð�)
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
    }




    public void Down()
    {

        if (Input.GetKey(KeyCode.DownArrow) && isInTunnel == false && isGround == true)
        {
            anim.SetBool("Down", true);
            isRollingReady = true;

        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {

            anim.SetBool("Down", false);
            isRollingReady = false;

        }


    }

    public void Jump()
    {

        float yinput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && jumpcount > 0)
        {
            playerAudio.clip = jumpClip;
            playerAudio.Play();
            anim.SetBool("Jump", true);
            isGround = false;
            playerrigidbody.linearVelocity = Vector2.zero;
            playerrigidbody.AddForce(new Vector2(0, jumpForce));
            jumpcount--;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            isGround = true;
            jumpcount = 2;
            mjumpcount = 2;
            anim.SetBool("Jump", false);



        }
        if (collision.gameObject.CompareTag("Tunnel"))
        {

            isGround = true;
            jumpcount = 2;
            mjumpcount = 2;
            anim.SetBool("Jump", false);
        }


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;


        }

        if (collision.gameObject.CompareTag("Tunnel"))
        {

            isInTunnel = true;


        }
    }


    public void Die()
    {
        isDead = true;
        playerAudio.clip = deadClip;
        playerAudio.Play();
        isDead = true;
        anim.SetTrigger("Dead");
        StartCoroutine(Dead());

        playerrigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Die();
            StartCoroutine(Dead());

        }

        if (collision.gameObject.CompareTag("Potal"))
        {

            SceneManager.LoadScene(2);

        }




    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BossRoom"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene("Boss");
            }
        }
    }
    private void Rolling()
    {
       
        if (isRoling) return; // �̹� ������ ���̸� ����

        if (isRollingReady &&Input.GetKeyDown(KeyCode.Return))
        {
            isNothit = false;
            tapCount++; // ���� Ű �Է� Ƚ�� ����
            lastTapTime = Time.time; // ������ �Է� �ð� ������Ʈ

            // Rolling �ִϸ��̼� ����
            anim.SetBool("Rolling", true);
        }

        // ������ �Է� �� 1�ʰ� ������ �̵� ����
        if (!isRoling && tapCount > 0 && Time.time - lastTapTime >= doubleTapTime)
        {
            StartCoroutine(StartRolling());
        }
    }
    // �ڷ�ƾ: 1�� �� �Էµ� Ƚ���� ����Ͽ� �̵�
    private IEnumerator StartRolling()
    {

        isRoling = true; // ������ ����

        // �̵� �Ÿ� = �⺻ �ӵ� * �Է� Ƚ��
        float moveDistance = speed * tapCount;

        // ������ �������� �������� �� ���� (Impulse ���)
        playerrigidbody.AddForce(Vector2.right * moveDistance, ForceMode2D.Impulse);

        // �Է� Ƚ�� �ʱ�ȭ
        tapCount = 0;

        // Rolling �ִϸ��̼��� 0.5�� �Ŀ� ����
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Rolling", false);
        isRoling = false;
        isNothit = true;
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
            isGround = false; // �÷��̾ ������ �������� ��� isGround = false
        }
    }


    public void Hit()
    {
        int currentHp = GameManager.instance.score;
        float randomvalue = Random.Range(0, 360);

        if (currentHp > 0 && isNothit)
        {
            for (int i = 0; i < hp; i++)
            {
                Debug.Log("�ǰ�");

                // �÷��̾� �ֺ� ���� ��ġ�� ����
                Vector2 spawnOffset = Random.insideUnitCircle.normalized * 1.5f;
                Vector2 spawnPos = (Vector2)transform.position + spawnOffset;

                GameObject coin = Instantiate(coinpreFab, spawnPos, Quaternion.Euler(0, 0, randomvalue));

                // ���ο� ���� ���� ���� ȿ�� (��: �ݶ��̴� ��Ȱ��ȭ �� ��Ȱ��)
                StartCoroutine(EnableCoinCollider(coin));
                StartCoroutine(DestroyCoin(coin));
            }

            GameManager.instance.lossCoin();
            StartCoroutine(Hurt());
        }
        else if (hp == 0)
        {
            Die();
        }
    }
    IEnumerator Hurt()
    {

        isNothit = false;
        StartCoroutine(HitEffect());
        yield return new WaitForSeconds(1);
        isNothit = true;
    }
    IEnumerator HitEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            SpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            SpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

    }


    // ���� �ݶ��̴��� ���� �ð� �Ŀ� Ȱ��ȭ
    IEnumerator EnableCoinCollider(GameObject coin)
    {
        Collider2D col = coin.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            yield return new WaitForSeconds(0.5f); // 0.5�� �� �ٽ� �浹 ����
            col.enabled = true;
        }
    }
    IEnumerator DestroyCoin(GameObject coin)
    {

        yield return new WaitForSeconds(2);
        Destroy(coin);


    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  ����� ��ư �ڵ�

    public void ReturnButtonDown()
    {
        isReturn = true;

    }

    public void ReturnButtonUp()
    {

        isReturn = false;


    }
    public void OnJumpButtonDown()
    {
        isJumpPressed = true;



    }

    public void onJumpButtonUp()
    {

        isJumpPressed = false;


    }

    public void OnRightButtonDown()
    {
        isRightPressed = true;
    }

    public void OnRightButtonUp()
    {
        isRightPressed = false;
        anim.SetBool("Walk", false); // �ȱ� �ִϸ��̼� ����
    }

    public void OnLeftButtonDown()
    {
        isLeftPressed = true;

    }

    public void OnLeftButtonUp()
    {
        isLeftPressed = false;
        anim.SetBool("Walk", false); // �ȱ� �ִϸ��̼� ����
    }

    public void onDownButtonDown()
    {
        isMrollingready = true;
        anim.SetBool("Down", true);

    }

    public void onDownButtonUp()
    {
        isMrollingready = false;
        anim.SetBool("Down", false);


    }
    private void MoveRight()
    {
        SpriteRenderer.flipX = false;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        anim.SetBool("Walk", true);
    }

    private void MoveLeft()
    {
        SpriteRenderer.flipX = true;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        anim.SetBool("Walk", true);
    }

    private void Mjump()
    {

        if (mjumpcount > 0 && !isMrollingready)
        {
            playerAudio.clip = jumpClip;
            playerAudio.Play();
            anim.SetBool("Jump", true);
            isGround = false;
            playerrigidbody.linearVelocity = Vector2.zero;
            playerrigidbody.AddForce(new Vector2(0, jumpForce));
            mjumpcount--;

        }


    }

    private void Mrolling()
    {
        if (isMrolling) return;

        if (isMrollingready && isReturn)
        {
            tapCount++;
            lastTapTime = Time.time;
            anim.SetBool("Rolling", true);


            isMrollingready = false;
            isReturn = false;
        }

        if (!isMrolling && tapCount > 0 && Time.time - lastTapTime >= doubleTapTime)
        {
            StartCoroutine(StartmRolling()); // ����Ͽ� �Ѹ� �ڷ�ƾ ȣ��
        }

    }
    private IEnumerator StartmRolling()
    {

        isMrolling = true; // ������ ����

        // �̵� �Ÿ� = �⺻ �ӵ� * �Է� Ƚ��
        float moveDistance = speed * tapCount;

        // ������ �������� �������� �� ���� (Impulse ���)
        playerrigidbody.AddForce(Vector2.right * moveDistance, ForceMode2D.Impulse);

        // �Է� Ƚ�� �ʱ�ȭ
        tapCount = 0;

        // Rolling �ִϸ��̼��� 0.5�� �Ŀ� ����
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Rolling", false);
        isMrolling = false;
    }


}