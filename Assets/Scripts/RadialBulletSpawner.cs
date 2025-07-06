using UnityEngine;

public class RadialBulletSpawner : MonoBehaviour
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
    private bool rotatePattern = false; // включение вращения
    [SerializeField]
    private float rotationSpeed = 30f; // градусов в секунду

    private float timer;
    private float currentAngleOffset = 0f;

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

        if (rotatePattern)
        {
            currentAngleOffset += rotationSpeed * Time.deltaTime;
            currentAngleOffset %= 360f; // чтобы не выходить за пределы круга
        }
    }

    private void SpawnRadialBullets()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep + currentAngleOffset;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Quaternion rotation = Quaternion.LookRotation(dir) * initialRotation;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            if (bullet.TryGetComponent<LaserBullet>(out var script))
            {
                script.SetDirection(dir, bulletSpeed, lifeTime);
            }
        }
    }
}
