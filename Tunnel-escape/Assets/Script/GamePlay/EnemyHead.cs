using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public float damage = 10;
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg")&& !enemy.isKnee)
        {
            // Kiểm tra phần Enemy bị trúng đòn dựa trên Tag của Collider
            enemy.HitOnHead(); // Trúng đầu
            enemy.hp -= damage;
            Debug.Log("head");
        }
    }
}
