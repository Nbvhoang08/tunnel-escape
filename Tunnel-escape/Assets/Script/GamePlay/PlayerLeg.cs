using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeg : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("PlayerLeg"))
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Leg"))
            {
                // Lấy vị trí va chạm
                Vector3 contactPoint = other.ClosestPoint(transform.position);
                // Spawn hiệu ứng máu
                SpawnBloodEffect(contactPoint);

            }
        }
    }
     public GameObject bloodEffectPrefab;
    // Hàm spawn hiệu ứng máu
    private void SpawnBloodEffect(Vector3 position)
    {
        if (bloodEffectPrefab != null)
        {
            // Instantiate hiệu ứng máu tại vị trí va chạm
            Instantiate(bloodEffectPrefab, position, Quaternion.identity);
        }
    }
}

