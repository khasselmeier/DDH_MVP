using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRiseAndFall : MonoBehaviour
{
    [Header("Lava Movement Settings")]
    public float riseHeight = 2f; //how high it will rise
    public float speed = 1f; //speed lava will rise
    public float baseHeight = 0f; //original Y position of the lava

    private float initialYPosition;

    private void Start()
    {
        initialYPosition = transform.position.y;
    }

    private void Update()
    {
        float newY = initialYPosition + Mathf.Sin(Time.time * speed) * riseHeight;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
