using UnityEngine;

public class BulletShaker : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.1f; // сила дрожания
    [SerializeField]
    private float frequency = 10f;  // скорость дрожания

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float offsetX = Mathf.Sin(Time.time * frequency) * amplitude;
        float offsetZ = Mathf.Cos(Time.time * frequency) * amplitude;

        transform.position = initialPosition + new Vector3(offsetX, 0f, offsetZ);
    }
}
