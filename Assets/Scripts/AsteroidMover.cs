using UnityEngine;

public class AsteroidMover : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.back;
    private float moveSpeed = 3f;

    private Vector3 rotationAxis = Vector3.up;
    private float rotationSpeed = 45f;

    private float destroyZ = -200f;

    public void SetDirection(Vector3 dir, float moveSpd, Vector3 rotAxis, float rotSpd)
    {
        moveDirection = dir.normalized;
        moveSpeed = moveSpd;
        rotationAxis = rotAxis.normalized;
        rotationSpeed = rotSpd;
    }

    void Update()
    {
        // Движение
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Вращение
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        // Удаление, если вышел за нижнюю границу экрана
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
