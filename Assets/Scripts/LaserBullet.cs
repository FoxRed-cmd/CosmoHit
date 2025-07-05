using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    private float speed = 15f;
    [SerializeField]
    private float lifeTime = 5f;

    private Vector3 direction = Vector3.back;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection, float newSpeed)
    {
        direction = newDirection.normalized;
        speed = newSpeed;
    }
}
