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
        survieTime = 60; // 초기 생존 시간 설정
     
    }

    void Update()
    {
        if (!isGameover) // 게임 오버 상태가 아니면 시간 감소
        {
            survieTime -= Time.deltaTime;
            TimeText.text = "Time:" + Mathf.Max(0, (int)survieTime); // 음수 방지

            if (survieTime <= 0) // 시간이 0이 되면 처리
            {
                survieTime = 0; // 시간 고정
                isGameover = true; // 게임 오버 상태로 변경

              
                TimeText.text = "Time: 0"; // UI 업데이트
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