using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuUI; // �޴� �г�

    // �޴� Ȱ��ȭ
    public void OpenMenu()
    {
        menuUI.SetActive(true);      // �޴� UI ���̱�
        Time.timeScale = 0f;         // ���� �Ͻ�����
    }

    // �޴� ��Ȱ��ȭ
    public void CloseMenu()
    {
        menuUI.SetActive(false);     // �޴� UI ����
        Time.timeScale = 1f;         // ���� �簳
    }

    public void Restart()
    {
        Time.timeScale = 1f;         // �� �̵� �� �ð� ����ȭ
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("main");
    }

    public void Close()
    {
        Application.Quit();
    }
}
