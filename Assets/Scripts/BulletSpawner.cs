using UnityEngine;

public class BulletSpawner : BaseBulletSpawner
{
    [SerializeField]
    private float[] spawnIntervals;
    [SerializeField]
    private bool loop = true;

    private int currentIndex = 0;

    private void Start()
    {
        initialRotation = Quaternion.Euler(90f, 90f, 0f);
        Spawn();
        timer = 0f;
    }

    void Update()
    {
        if (spawnIntervals.Length == 0) return;

        timer += Time.deltaTime;
        spawnInterval = spawnIntervals[currentIndex];

        if (timer >= spawnInterval)
        {
            Spawn();
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
}
