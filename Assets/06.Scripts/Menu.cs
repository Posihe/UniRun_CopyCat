using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuUI; // 메뉴 패널

    // 메뉴 활성화
    public void OpenMenu()
    {
        menuUI.SetActive(true);      // 메뉴 UI 보이기
        Time.timeScale = 0f;         // 게임 일시정지
    }

    // 메뉴 비활성화
    public void CloseMenu()
    {
        menuUI.SetActive(false);     // 메뉴 UI 끄기
        Time.timeScale = 1f;         // 게임 재개
    }

    public void Restart()
    {
        Time.timeScale = 1f;         // 씬 이동 전 시간 정상화
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("main");
    }

    public void Close()
    {
        Application.Quit();
    }
}
