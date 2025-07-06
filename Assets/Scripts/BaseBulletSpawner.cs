using UnityEngine;

public class BaseBulletSpawner : MonoBehaviour
{
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected float bulletSpeed = 30f;
    [SerializeField]
    protected float lifeTime = 5f;
    [SerializeField]
    protected float spawnInterval = 2f;
    [SerializeField]
    protected Vector3 shootDirection = Vector3.back;

    protected Quaternion initialRotation = Quaternion.Euler(90f, 90f, 180f);
    protected float timer;

    private void Start()
    {
        Spawn();
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    protected virtual void Spawn()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(shootDirection) * initialRotation);

        if (bullet.TryGetComponent<LaserBullet>(out var script))
        {
            script.SetDirection(shootDirection, bulletSpeed, lifeTime);
        }
    }
}
