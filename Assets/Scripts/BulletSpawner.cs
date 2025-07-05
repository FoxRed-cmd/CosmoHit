using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private float lifeTime = 5f;
    [SerializeField]
    private float[] spawnIntervals;
    [SerializeField]
    private bool loop = true;

    private Vector3 shootDirection = Vector3.back;
    private Quaternion initialRotation = Quaternion.Euler(90f, 90f, 0f);

    private int currentIndex = 0;
    private float timer;

    private void Start()
    {
        SpawnBullet();
        timer = 0f;
    }

    void Update()
    {
        if (spawnIntervals.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= spawnIntervals[currentIndex])
        {
            SpawnBullet();
            timer = 0f;

            currentIndex++;

            if (currentIndex >= spawnIntervals.Length)
            {
                if (loop)
                    currentIndex = 0;
                else
                    enabled = false;
            }
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, initialRotation);

        var script = bullet.GetComponent<LaserBullet>();
        if (script != null)
        {
            script.SetDirection(shootDirection, bulletSpeed, lifeTime);
        }
    }
}
