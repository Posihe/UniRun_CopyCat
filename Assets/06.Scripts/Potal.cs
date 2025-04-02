using UnityEngine;

public class Potal : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.LoadMainScene(); //  포털을 통해 이동 시 특정 위치로 이동
        }
    }
}
