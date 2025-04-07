using UnityEngine;
using UnityEngine.UI;

public class BossRoomUI : MonoBehaviour
{
    public Text TimeText;

    private float  survieTime;
    private bool isGameover = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        survieTime = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameover)
        {
            survieTime -= Time.deltaTime;
            TimeText.text = "Time:" + (int)survieTime;

        }

        if(survieTime<=0)
        {
            isGameover = true;

            


        }
    }
}
