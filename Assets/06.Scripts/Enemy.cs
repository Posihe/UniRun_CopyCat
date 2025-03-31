using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    public Transform[] patrol;
    private Animator anim;
    public bool isLeft;
    public bool isRight;

    // Update is called once per frame
    private void Start()
    {
        anim=GetComponent<Animator>();
    }
    void Update()
    {
        Check();
    }


    void Check()
    {
        if (isLeft==true)
        {
            isRight = false;
            transform.position += (Vector3)Vector2.left * speed * Time.deltaTime;
            anim.SetBool("LeftWalk", true);
            anim.SetBool("RightWalk", false);
        }
         if(isRight==true)
        {
            isLeft = false;
            transform.position += (Vector3)Vector2.right * speed * Time.deltaTime;
            anim.SetBool("RightWalk", true);
            anim.SetBool("LeftWalk", false);


        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Left"))
        {
            isLeft = true;
            isRight = false;


        }
        if(collision.gameObject.CompareTag("Right"))
        {
           
            isRight = true;
            isLeft = false;
        }
    }
}
