using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public Vector2 playerPosition; // Ư�� �� �̵� �� ������ ��ġ
    public int score;

    private bool shouldMovePlayer = false; //  �÷��̾� ��ġ ������ �ʿ��� ��츸 true

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded; // �� ���� �̺�Ʈ ���
        }
        else
        {
            Destroy(gameObject);
        }
        score = 0;
    }

    public void AddCoin()
    {
        if (!isGameOver)
        {
            score++;
        }
    }

    public void lossCoin()
    {
        if (score > 0)
        {
            score = 0;
        }
        else 
        {
            score = -1;
        }


    }
    public void ResetGame()
    {
        score = 0;
    }
    public void LoadMainScene()
    {
        shouldMovePlayer = true; //  ������ ���� �̵� �� ��ġ ���� Ȱ��ȭ
        SceneManager.LoadScene("Main"); // �� ����
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main" && shouldMovePlayer) //  ���� �̵� �ÿ��� ����
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }
            shouldMovePlayer = false; //  ���� �� ���� �� �ʱ�ȭ
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �� ���� �̺�Ʈ ���� (�޸� ���� ����)
    }
}
