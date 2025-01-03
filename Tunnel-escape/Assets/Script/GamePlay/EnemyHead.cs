using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg"))
        {
            // Kiểm tra phần Enemy bị trúng đòn dựa trên Tag của Collider
            enemy.HitOnHead(); // Trúng đầu
            enemy.hp -= 50;
        }
    }
}
