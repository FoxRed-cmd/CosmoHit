using System.Collections;
using UnityEngine;

public class BulletSpawnerWithIntervals : BaseBulletSpawner
{
    [SerializeField]
    protected float[] spawnIntervals;

    private void Start()
    {
        initialRotation = Quaternion.Euler(90f, 90f, 180f);
        bulletCount = spawnIntervals.Length;
        StartCoroutine(SpawnRoutine());
    }

    protected override IEnumerator SpawnRoutine()
    {
        if (spawnIntervals.Length == 0 || spawnIntervals == null)
        {
            isSpawning = false;
        }

        int currentIndex = 0;

        while (isSpawning)
        {
            if (currentIndex >= spawnIntervals.Length)
            {
                currentIndex = 0;
            }
            spawnInterval = spawnIntervals[currentIndex++];
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
