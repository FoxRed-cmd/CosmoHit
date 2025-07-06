using UnityEngine;

public class DirectionalSpreadSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 30f;
    [SerializeField]
    private float lifeTime = 5f;
    [SerializeField]
    private float spawnInterval = 5f;
    [SerializeField]
    private int bulletCount = 8;
    [SerializeField]
    private float spreadAngle = 180f;
    [SerializeField]
    private Vector3 flyDirection = Vector3.down;

    private float timer;
    private Quaternion initialRotation = Quaternion.Euler(90f, 90f, 180f);

    private void Start()
    {
        SpawnBullets();
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBullets();
            timer = 0f;
        }
    }

    private void SpawnBullets()
    {
        float angleStep = spreadAngle / (bulletCount - 1); // равномерное распределение
        float startAngle = -spreadAngle / 2f; // от -90 до +90 при 180°

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // Поворачиваем Vector3.down на угол вокруг XZ-плоскости (вокруг X)
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * flyDirection;

            Quaternion rotation = Quaternion.LookRotation(direction) * initialRotation;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            if (bullet.TryGetComponent<LaserBullet>(out var script))
            {
                script.SetDirection(direction, bulletSpeed, lifeTime);
            }
        }
    }
}
