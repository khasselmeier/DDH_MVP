using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public int damageAmount = 50; // amt of damage dealt
    public float damageInterval = 1f; // how often the player takes damage per second

    private bool isPlayerInLava = false;
    private float damageTimer = 0f;

    private PlayerBehavior player; // ref to player script

    void Update()
    {
        // apply damage over time if the player is in the lava
        if (isPlayerInLava && player != null)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                player.TakeDamage(damageAmount); // calls the TakeDamage method in PlayerBehavior
                damageTimer = 0f; // resets damage timer
            }
        }
    }

    // player enters the lava
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInLava = true;
            player = other.GetComponent<PlayerBehavior>();

            if (player != null)
            {
                Debug.Log("Player has entered the lava");
            }
        }
    }

    // player exits the lava
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInLava = false;
            damageTimer = 0f; // reset damage timer
            Debug.Log("Player has left the lava");
        }
    }
}
