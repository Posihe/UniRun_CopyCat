using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
   

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.position + offset;
    }
}
