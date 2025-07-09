using UnityEngine;

public class RandomAsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;

    [Header("Spawn")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = 12f;
    [SerializeField] private float maxY = 12f;

    [Header("Speed")]
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float minRotateSpeed = 20f;
    [SerializeField] private float maxRotateSpeed = 90f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnAsteroid();
            timer = 0f;
        }
    }

    void SpawnAsteroid()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new(x, y, transform.position.z);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        float moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        Vector3 rotateAxis = Random.onUnitSphere;
        float rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);

        var mover = asteroid.AddComponent<AsteroidMover>();
        mover.SetDirection(Vector3.back, moveSpeed, rotateAxis, rotateSpeed);
    }
}
