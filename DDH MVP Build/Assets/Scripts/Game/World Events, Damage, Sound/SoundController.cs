using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource miningGemSFX;
    public AudioSource lavaSFX;
    public AudioSource damageSFX;
    public AudioSource goldPickupSFX;
    public AudioSource rockPickupSFX;
    public AudioSource healthPickupSFX;
    public AudioSource footstepsSFX;

    [Header("Lava Settings")]
    public Transform lava;
    public float lavaPlayDistance = 10f;
    public float maxLavaVolume = 1.0f;

    [Header("SFX Durations")]
    public float miningSFXDuration = 2f; //sound will play for -- seconds
    public float damageSFXDuration = 0.5f;
    public float goldSFXDuration = 1f;
    public float rockSFXDuration = 1f;
    public float healthSFXDuration = 1f;

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
            }
        }

        if (player != null)
        {
            AdjustLavaSoundVolume();
        }
    }

    public void PlayFootstepsSound()
    {
        if (footstepsSFX != null && !footstepsSFX.isPlaying)
        {
            footstepsSFX.loop = true; // Loop footsteps for continuous walking
            footstepsSFX.Play();
        }
    }

    public void StopFootstepsSound()
    {
        if (footstepsSFX != null && footstepsSFX.isPlaying)
        {
            footstepsSFX.loop = false;
            footstepsSFX.Stop();
        }
    }

    public void PlayGoldPickupSound()
    {
        PlaySoundWithDuration(goldPickupSFX, goldSFXDuration);
    }

    public void PlayRockPickupSound()
    {
        PlaySoundWithDuration(rockPickupSFX, rockSFXDuration);
    }

    public void PlayHealthPickupSound()
    {
        PlaySoundWithDuration(healthPickupSFX, healthSFXDuration);
    }

    public void PlayMiningSound()
    {
        PlaySoundWithDuration(miningGemSFX, miningSFXDuration);
    }

    public void PlayDamageSound()
    {
        PlaySoundWithDuration(damageSFX, damageSFXDuration);
    }

    private void PlaySoundWithDuration(AudioSource audioSource, float duration)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Invoke(nameof(StopCurrentSound), duration);
        }
        else
        {
            Debug.LogWarning($"{audioSource?.name} SFX is not assigned");
        }
    }

    private void StopCurrentSound()
    {
        foreach (AudioSource source in new[] { goldPickupSFX, rockPickupSFX, healthPickupSFX, miningGemSFX, damageSFX })
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }
    }

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