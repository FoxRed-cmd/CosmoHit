using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [SerializeField]
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
}
