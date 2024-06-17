using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController enemy;

    void Start()
    {
        enemy = gameObject.transform.parent.GetComponent<EnemyController>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("metal_hit");
            PlayerController.Instance.TakeDamage(enemy.damage);

        }
    }
}
