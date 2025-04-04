using UnityEngine;

public class Fire : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * 10*Time.deltaTime);
    }


    
}
