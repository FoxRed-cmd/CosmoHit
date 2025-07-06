using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDirectedBurstSpawner : BaseBulletSpawner
{
    [SerializeField]
    protected float spawnRadius = 3f;
    [SerializeField]
    protected float delayBetweenBullets = 0.2f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    protected override void Spawn()
    {
        StartCoroutine(SpawnAndFireBurst());
    }

    private IEnumerator SpawnAndFireBurst()
    {
        List<GameObject> spawnedBullets = new List<GameObject>();

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(offset2D.x, 0f, offset2D.y);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(shootDirection) * initialRotation);
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
                script.SetDirection(shootDirection.normalized, bulletSpeed, lifeTime);
            }
        }
    }
}
