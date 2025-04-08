using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 rightOffset;
    public Vector3 leftOffset;
    PlayerController player;
    public float smoothSpeed = 0.125f; // 부드러운 전환 속도
    private Vector3 currentOffset;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        // 초기 값 설정
        currentOffset = rightOffset;
    }

    void Update()
    {
        // 현재 바라보는 방향에 따른 목표 오프셋 결정
        Vector3 targetOffset = player.watchRight ? rightOffset : leftOffset;

        // 카메라의 위치를 부드럽게 전환
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed);

        // 카메라 위치 업데이트
        gameObject.transform.position = target.position + currentOffset;
    }
}