using System.Collections;
using UnityEngine;

public class BaseBulletSpawner : MonoBehaviour
{
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected float bulletSpeed = 30f;
    [SerializeField]
    protected int bulletCount = 8;
    [SerializeField]
    protected float lifeTime = 5f;
    [SerializeField]
    protected float rotationSpeed = 30f; // градусов в секунду
    [SerializeField]
    protected bool isRotate = false; // включение вращения
    [SerializeField]
    protected float spawnInterval = 2f;
    [SerializeField]
    protected bool isSpawning = true;
    protected Vector3 shootDirection = Vector3.back;

    protected Quaternion initialRotation = Quaternion.Euler(90f, 90f, 180f);
    protected float currentAngleOffset = 0f;
    protected float timer;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    protected void Update()
    {
        if (isRotate)
        {
            RotateBullet();
        }
    }

    protected virtual IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    protected virtual void Spawn()
    {
        Vector3 direction = Quaternion.Euler(0f, currentAngleOffset, 0f) * shootDirection;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction) * initialRotation);

        if (bullet.TryGetComponent<LaserBullet>(out var script))
        {
            script.SetDirection(direction, bulletSpeed, lifeTime);
        }
    }

    protected void RotateBullet()
    {
        currentAngleOffset += rotationSpeed * Time.deltaTime;
        currentAngleOffset %= 360f; // чтобы не выходить за пределы круга
    }
}
