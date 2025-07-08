using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private Vector3 outPosition;
    [SerializeField]
    private float spawnInterval = 5f;
    [SerializeField]
    private float stayTime = 5f;

    private float stayTimer = 0f;

    private void Start()
    {
        spawnPosition = transform.position;
        SpawnEnemy();
        stayTimer = 0f;
}

    private void Update()
    {
        stayTimer += Time.deltaTime;
        if (stayTimer >= spawnInterval)
        {
            SpawnEnemy();
            stayTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyShip = Instantiate(enemyShipPrefab);
        enemyShip.transform.position = spawnPosition;
        if (enemyShip.TryGetComponent<EnemyShipController>(out var controller))
        {
            controller.Initialize(targetPosition, outPosition, stayTime);
        }
    }
}
