using UnityEditor.Rendering;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Vector2 stoneposition;
    
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {


            transform.position = stoneposition;
            gameObject.SetActive(false);

        }
    }
}
