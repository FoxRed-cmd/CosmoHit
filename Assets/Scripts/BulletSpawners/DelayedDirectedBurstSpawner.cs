using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    protected bool waitAllBullets = false;

    private void Start()
    {
        if (spawnIntervals.Length != 0)
        {
            bulletCount = spawnIntervals.Length;
            spawnInterval = spawnIntervals.Sum();
        }
        StartCoroutine(SpawnRoutine());
    }

    protected override void Spawn()
    {
        StartCoroutine(SpawnAndFireBurst());
    }

    private IEnumerator SpawnAndFireBurst()
    {
        Stack<GameObject> spawnedBullets = new Stack<GameObject>();

        Vector3 direction = Quaternion.Euler(0f, currentAngleOffset, 0f) * shootDirection;

        int currentIndex = 0;

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

            if (spawnIntervals.Length > 0 && spawnIntervals != null)
            {
                if (currentIndex >= spawnIntervals.Length)
                {
                    currentIndex = 0;
                }
                delayBetweenBullets = spawnIntervals[currentIndex++];
            }

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction) * initialRotation);

            if (!waitAllBullets && bullet != null && bullet.TryGetComponent<LaserBullet>(out var script))
            {
                script.SetDirection(direction, bulletSpeed, lifeTime);
            }
            else
            {
                spawnedBullets.Push(bullet);
            }

            yield return new WaitForSeconds(delayBetweenBullets);
        }

        if (waitAllBullets)
        {
            for (int i = 0; i < spawnedBullets.Count; i++)
            {
                var bullet = spawnedBullets.Pop();
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
}
