
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject[] stone;
    public GameObject[] stonespawner;
    public GameObject fire;
    public GameObject firespawn;
    private Rigidbody2D rb;
    private SpriteRenderer spriterenderer;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //Jump();
        FireFire();
        
    }



    public void onfire()
    {
        Instantiate(fire, firespawn.transform.position, Quaternion.identity);


    }

    private void FireFire()
    {
        anim.SetTrigger("Fire");
    }

    private void Jump()
    {
      
        anim.SetTrigger("Jump");

    }

    public void fallen()
    {
        for (int i = 0; i < stone.Length; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                stone[i].SetActive(true);
               


            }
            else
            {
                return;
               

            }
        }


    }

}
