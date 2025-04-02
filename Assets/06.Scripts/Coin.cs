using UnityEngine;


public class Coin : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector2(0,90)*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            GameManager.instance.AddCoin();
            gameObject.SetActive(false);
        }
    }
    
}
