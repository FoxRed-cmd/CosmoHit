using System;
using UnityEngine;

public class RadialBulletSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 30f;
    [SerializeField]
    private float spawnInterval = 5f;

    private float timer;

    private Quaternion initialRotation = Quaternion.Euler(90f, 90f, 180f);

    private void Start()
    {
        SpawnRadialBullets();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRadialBullets();
            timer = 0f;
        }
    }

    private void SpawnRadialBullets()
    {
        int bulletCount = 8;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Quaternion rotation = Quaternion.LookRotation(dir) * initialRotation;

            Console.WriteLine($"Spawning bullet at angle: {angle} degrees, direction: {dir}, rotation: {rotation}");

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            var script = bullet.GetComponent<LaserBullet>();
            if (script != null)
            {
                script.SetDirection(dir, bulletSpeed);
            }
        }
    }
}
