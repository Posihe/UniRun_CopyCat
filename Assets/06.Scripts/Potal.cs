using UnityEngine;

public class Potal : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.LoadMainScene(); //  ������ ���� �̵� �� Ư�� ��ġ�� �̵�
        }
    }
}
