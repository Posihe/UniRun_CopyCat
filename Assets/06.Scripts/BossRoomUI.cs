using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossRoomUI : MonoBehaviour
{
    public Text TimeText;
    private float survieTime;
    private bool isGameover = false;
    public GameObject pannel;
 

    void Start()
    {
        survieTime = 60; // �ʱ� ���� �ð� ����
     
    }

    void Update()
    {
        if (!isGameover) // ���� ���� ���°� �ƴϸ� �ð� ����
        {
            survieTime -= Time.deltaTime;
            TimeText.text = "Time:" + Mathf.Max(0, (int)survieTime); // ���� ����

            if (survieTime <= 0) // �ð��� 0�� �Ǹ� ó��
            {
                survieTime = 0; // �ð� ����
                isGameover = true; // ���� ���� ���·� ����

              
                TimeText.text = "Time: 0"; // UI ������Ʈ
                StartCoroutine(End());
            }
        }
    }


    IEnumerator End()
    {
        pannel.SetActive(true);
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}