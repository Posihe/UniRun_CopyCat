using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public Vector2 playerPosition; // 특정 씬 이동 시 적용할 위치
    public int score = 0;

    private bool shouldMovePlayer = false; //  플레이어 위치 변경이 필요한 경우만 true

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 변경 이벤트 등록
        }
        else
        {
            Destroy(gameObject);
        }
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
        score = 0;


    }

    public void LoadMainScene()
    {
        shouldMovePlayer = true; //  포털을 통해 이동 시 위치 변경 활성화
        SceneManager.LoadScene("Main"); // 씬 변경
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main" && shouldMovePlayer) //  포털 이동 시에만 실행
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }
            shouldMovePlayer = false; //  다음 씬 변경 시 초기화
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 변경 이벤트 해제 (메모리 누수 방지)
    }
}
