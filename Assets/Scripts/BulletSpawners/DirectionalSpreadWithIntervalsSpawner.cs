using UnityEngine;

public class DirectionalSpreadWithIntervalsSpawner : BulletSpawnerWithIntervals
{
    [SerializeField]
    protected float spreadAngle = 360f;

    private void Start()
    {
        initialRotation = Quaternion.Euler(90f, 90f, 180f);
        bulletCount = spawnIntervals.Length;
        StartCoroutine(SpawnRoutine());
    }

    protected override void Spawn()
    {
        float angleStep = spreadAngle / (bulletCount - 1); // равномерное распределение
        float startAngle = -spreadAngle / 2f; // от -90 до +90 при 180°

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i + currentAngleOffset;

            // Поворачиваем Vector3.down на угол вокруг XZ-плоскости (вокруг X)
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * shootDirection;

            Quaternion rotation = Quaternion.LookRotation(direction) * initialRotation;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            if (bullet.TryGetComponent<LaserBullet>(out var script))
            {
                script.SetDirection(direction, bulletSpeed, lifeTime);
            }
        }
    }
}