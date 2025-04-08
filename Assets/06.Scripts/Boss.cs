
using System.Collections;
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
    public float actionInterval = 3f;
    private float timer = 0f;
    public GameObject pannel;
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
        
        timer += Time.deltaTime;
        if(timer>=actionInterval)
        {

            TriggerRandomAction();
            timer = 0f;
        }
        
    }

    void TriggerRandomAction()
    {

        int randomIndex = Random.Range(0, 2);
        switch(randomIndex)
        {

            case 0:
                Jump();
                break;
            case 1:
                FireFire();
                break;


        }



    }

    public void onfire()
    {
        Instantiate(fire, firespawn.transform.position, Quaternion.identity);


    }

    private void FireFire()
    {
        anim.SetTrigger("Fire");
        Debug.Log("발사");
    }

    private void Jump()
    {
      
        anim.SetTrigger("Jump");
        Debug.Log("점프");

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
