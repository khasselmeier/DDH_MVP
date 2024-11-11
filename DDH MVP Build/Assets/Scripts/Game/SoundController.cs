using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource miningGemSFX;
    public AudioSource lavaSFX;

    [Header("Lava Settings")]
    public Transform lava;
    public float lavaPlayDistance = 10f;
    public float maxLavaVolume = 1.0f;

    [Header("Mining Settings")]
    public float miningSFXDuration = 2f; //sound will play for -- seconds

    public static SoundController instance;

    private Transform player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                //Debug.Log("Player found by SoundController");
            }
        }

        if (player != null)
        {
            AdjustLavaSoundVolume();
        }
    }

    public void PlayMiningSound()
    {
        if (miningGemSFX != null)
        {
            miningGemSFX.Play();
            Invoke(nameof(StopMiningGemSound), miningSFXDuration);
        }
        else
        {
            Debug.LogWarning("MiningGemSFX is not asssigned");
        }
    }

    public void StopMiningGemSound()
    {
        if (miningGemSFX != null && miningGemSFX.isPlaying)
        {
            miningGemSFX.Stop();
        }
    }

    /*private void CheckLavaDistance()
    {
        if (lavaSFX != null && player != null && lava != null)
        {
            float distance = Vector3.Distance(player.position, lava.position);

            if (distance <= lavaPlayDistance && !lavaSFX.isPlaying)
            {
                lavaSFX.Play();
            }
            else if (distance > lavaPlayDistance && lavaSFX.isPlaying)
            {
                lavaSFX.Stop();
            }
        }
    }*/

    private void AdjustLavaSoundVolume()
    {
        if (lavaSFX != null && lava != null)
        {
            float distance = Vector3.Distance(player.position, lava.position);

            if (distance <= lavaPlayDistance)
            {
                float volume = Mathf.Lerp(maxLavaVolume, 0, distance / lavaPlayDistance);
                lavaSFX.volume = volume;

                if (!lavaSFX.isPlaying)
                {
                    lavaSFX.Play();
                }
            }
            else
            {
                if (lavaSFX.isPlaying)
                {
                    lavaSFX.Stop();
                }
            }
        }
    }
}
