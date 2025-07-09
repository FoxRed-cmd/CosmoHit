using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDirectedBurstSpawner : BaseBulletSpawner
{
    [SerializeField]
    private SpawnShape spawnShape = SpawnShape.Circle;
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private float ellipseRadiusX = 3f;
    [SerializeField]
    private float ellipseRadiusZ = 1.5f;
    [SerializeField]
    protected float delayBetweenBullets = 0.2f;
    [SerializeField]
    protected float[] spawnIntervals;

    private int currentIndex = 0;

    private void Start()
    {
        bulletCount = spawnIntervals.Length;
    }

    private void Update()
    {
        if (spawnIntervals.Length > 0 && spawnIntervals != null)
        {
            timer += Time.deltaTime;
            spawnInterval = spawnIntervals[currentIndex];
            if (timer >= spawnInterval)
            {
                Spawn();
                timer = 0f;

                currentIndex++;

                if (currentIndex >= spawnIntervals.Length)
                {
                    currentIndex = 0;
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                Spawn();
                timer = 0f;
            }
        }

        if (isRotate)
        {
            currentAngleOffset += rotationSpeed * Time.deltaTime;
            currentAngleOffset %= 360f; // чтобы не выходить за пределы круга
        }

    }

    protected override void Spawn()
    {
        StartCoroutine(SpawnAndFireBurst());
    }

    private IEnumerator SpawnAndFireBurst()
    {
        List<GameObject> spawnedBullets = new List<GameObject>();

        Vector3 direction = Quaternion.Euler(0f, currentAngleOffset, 0f) * shootDirection;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 offset = Vector3.zero;

            switch (spawnShape)
            {
                case SpawnShape.Circle:
                    Vector2 circleOffset = Random.insideUnitCircle * radius;
                    offset = new Vector3(circleOffset.x, 0f, circleOffset.y);
                    break;

                case SpawnShape.Ellipse:
                    float angle = Random.Range(0f, Mathf.PI * 2f);
                    float u = Random.Range(0f, 1f) + Random.Range(0f, 1f);
                    float r = (u > 1) ? 2 - u : u; // равномерное распределение

                    float x = Mathf.Cos(angle) * ellipseRadiusX * r;
                    float z = Mathf.Sin(angle) * ellipseRadiusZ * r;

                    offset = new Vector3(x, 0f, z);
                    break;
            }

            Vector3 spawnPosition = transform.position + offset;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction) * initialRotation);
            spawnedBullets.Add(bullet);

            yield return new WaitForSeconds(delayBetweenBullets);
        }

        foreach (var bullet in spawnedBullets)
        {
            if (bullet != null && bullet.TryGetComponent<BulletShaker>(out var shakerScript))
            {
                Destroy(shakerScript);
            }

            if (bullet != null && bullet.TryGetComponent<LaserBullet>(out var script))
            {
                script.SetDirection(direction, bulletSpeed, lifeTime);
            }
        }
    }
}
