using UnityEngine;

public class BulletSpawner : BaseBulletSpawner
{
    [SerializeField]
    protected float[] spawnIntervals;
    [SerializeField]
    private bool loop = true;

    private int currentIndex = 0;

    private void Start()
    {
        initialRotation = Quaternion.Euler(90f, 90f, 180f);
        bulletCount = spawnIntervals.Length;
    }

    protected virtual void Update()
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

        if (isRotate)
        {
            currentAngleOffset += rotationSpeed * Time.deltaTime;
            currentAngleOffset %= 360f; // чтобы не выходить за пределы круга
        }
    }
}
