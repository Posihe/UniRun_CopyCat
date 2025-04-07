using UnityEngine;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour
{

    private float speed;
    private int jumpcount;
    private Animator anim;
    private SpriteRenderer sprite;


    private bool isRightPressed = false;
    private bool isLeftPressed = false;
    private bool isJumpPressed = false;

    private void Start()
    {
        speed = GetComponent<PlayerController>().speed;
        jumpcount = GetComponent<PlayerController>().jumpcount;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 지속적인 버튼 입력 상태 감지
        if (isRightPressed)
        {
            MoveRight();
        }
        else if (isLeftPressed)
        {
            MoveLeft();
        }
        if (isJumpPressed)
        {
            //Jumpcount();

        }
    }

    public void OnJumpButtonDown()
    {
        isJumpPressed = true;

    }

    public void OnRightButtonDown()
    {
        isRightPressed = true;
    }

    public void OnRightButtonUp()
    {
        isRightPressed = false;
        anim.SetBool("Walk", false); // 걷기 애니메이션 정지
    }

    public void OnLeftButtonDown()
    {
        isLeftPressed = true;

    }

    public void OnLeftButtonUp()
    {
        isLeftPressed = false;
        anim.SetBool("Walk", false); // 걷기 애니메이션 정지
    }

    private void MoveRight()
    {
        sprite.flipX = false;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        anim.SetBool("Walk", true);
    }

    private void MoveLeft()
    {
        sprite.flipX = true;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        anim.SetBool("Walk", true);
    }


   

}