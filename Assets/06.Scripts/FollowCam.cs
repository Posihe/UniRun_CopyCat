using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 rightOffset;
    public Vector3 leftOffset;
    PlayerController player;
    public float smoothSpeed = 0.125f; // �ε巯�� ��ȯ �ӵ�
    private Vector3 currentOffset;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        // �ʱ� �� ����
        currentOffset = rightOffset;
    }

    void Update()
    {
        // ���� �ٶ󺸴� ���⿡ ���� ��ǥ ������ ����
        Vector3 targetOffset = player.watchRight ? rightOffset : leftOffset;

        // ī�޶��� ��ġ�� �ε巴�� ��ȯ
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, smoothSpeed);

        // ī�޶� ��ġ ������Ʈ
        gameObject.transform.position = target.position + currentOffset;
    }
}