using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeg : MonoBehaviour
{
    public Enemy enemy;
    public float damage = 10;
     private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra va chạm với Player hoặc vũ khí của Player
        if (other.gameObject.CompareTag("PlayerLeg"))
        {
            enemy.HitOnLeg(); // Trúng các phần còn lại
            enemy.hp -=damage;
        }
    }
    
}
