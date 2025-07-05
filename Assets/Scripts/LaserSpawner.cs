using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRate = 1f;

    public float spawnZ = 40f;
    public float minX = -50f;
    public float maxX = 50f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / spawnRate)
        {
            SpawnBullet();
            timer = 0f;
        }
    }

    private void SpawnBullet()
    {
        float spawnX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(spawnX, 0f, spawnZ);

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, new Quaternion(0.5f, 0.5f, -0.5f, 0.5f));
    }
}
