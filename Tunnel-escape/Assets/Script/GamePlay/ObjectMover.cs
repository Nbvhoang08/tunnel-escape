using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour, IObserver
{
    private float moveSpeed;
    void Awake()
    {
        Subject.RegisterObserver(this); // Đăng ký observer
    }
    void OnDestroy()
    {
        Subject.UnregisterObserver(this); // Hủy đăng ký observer
    }
    public void OnNotify(string eventName, object eventData)
    {
        if(eventName == "End")
        {
            SetSpeed(0);
        }
    }
    void Start()
    {
        // Khởi tạo tốc độ di chuyển mặc định
        moveSpeed = 5f;
    }
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    void Update()
    {
        // Di chuyển vật thể về phía người chơi (theo trục X)
        transform.Translate(new Vector3(0,1,0) * moveSpeed * Time.deltaTime);

        // Nếu vật thể đi ra khỏi phạm vi của người chơi, hãy hủy nó
        if (transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
