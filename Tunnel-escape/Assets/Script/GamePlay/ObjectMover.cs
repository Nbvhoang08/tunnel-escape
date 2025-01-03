using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private float moveSpeed;

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    void Update()
    {
        // Di chuyển vật thể về phía người chơi (theo trục X)
        transform.Translate(new Vector3(-1,0,0) * moveSpeed * Time.deltaTime);

        // Nếu vật thể đi ra khỏi phạm vi của người chơi, hãy hủy nó
        if (transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
