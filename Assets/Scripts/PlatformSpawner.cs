using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject platform;
    [SerializeField]
    Transform parent;
    [Tooltip("Maximum height diffrence between platforms.")]
    public float maxHeightDiff = 5.0f;
    [Tooltip("The percentage decrease in platform density per maxHeightDiff units (upward).")]
    [SerializeField]
    [Range(0, 1)]
    float platformDensityDecrease = 0.10f;
    [Tooltip("Spawn platforms this amount of units ahead of the player.")]
    public float spawnThreshhold = 15;
    [Tooltip("Spawn chance of a power-up on each platform.")]
    [Range(0, 1)]
    public float powerUpDropRate = 0.1f;
    [SerializeField]
    List<GameObject> PowerUps = new List<GameObject>();
    [SerializeField]
    float pwrUpHeightOffset = 0.5f;


    float oldPlatHeight;
    float leftBorder, rightBorder;
    Camera mainCam;
    BoxCollider2D coll;

    void Start()
    {
        mainCam = Camera.main;
        coll = platform.GetComponent<BoxCollider2D>();
        leftBorder = mainCam.ScreenToWorldPoint(Vector2.zero).x + coll.bounds.extents.x;
        rightBorder = mainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - coll.bounds.extents.x;

        SpawnPlatform();
    }

    void SpawnPlatform()
    {
        float x = Random.Range(leftBorder, rightBorder);
        var newPlatform = Instantiate(platform, new Vector3(x, oldPlatHeight), Quaternion.identity, parent);
        oldPlatHeight += maxHeightDiff;

        if (PowerUps.Count != 0)
        {
            Vector2 offset = new Vector2(Random.Range(-coll.bounds.extents.x, coll.bounds.extents.x), pwrUpHeightOffset);
            SpawnPowerUp(newPlatform.transform, offset);
        }
    }

    void SpawnPowerUp(Transform platform, Vector2 offset)
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= powerUpDropRate)
        {
            int randomPwrUp = Random.Range(0, PowerUps.Count);
            Instantiate(PowerUps[randomPwrUp], (Vector2)platform.position + offset, Quaternion.identity);
        }

    }

    void Update()
    {
        if (Mathf.Abs(oldPlatHeight - transform.position.y) < spawnThreshhold)
        {
            SpawnPlatform();
        }
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        leftBorder = mainCam.ScreenToWorldPoint(Vector2.zero).x + coll.bounds.extents.x;
        rightBorder = mainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - coll.bounds.extents.x;
#endif
    }
}
