using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rig;
    //public int npcHitDamage = 50;

    public void Initialize()
    {
        Destroy(gameObject, 3.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //EnemyChaseAttack enemy = collision.gameObject.GetComponent<EnemyChaseAttack>();
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // each bullet does 1 damage
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("NPC"))
        {
            // ref to the player
            PlayerBehavior player = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();

            if (player != null)
            {
                player.OnNpcHit(); // calls the player's OnNpcHit function to deal damage to player
            }

            Destroy(gameObject);
        }
    }
}