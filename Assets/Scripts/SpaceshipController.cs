using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    
    [SerializeField]
    private float moveForce = 20f; // Сила движения корабля
    [SerializeField]
    private float maxSpeed = 30f; // Максимальная скорость корабля
    [SerializeField]
    private float drag = 0.95f; // Коэффициент замедления (чем ближе к 1 — тем дольше тянет)
    
    [SerializeField]
    private float tiltAngleX = 15f;
    [SerializeField]
    private float tiltAngleY = 20f;
    [SerializeField]
    private float tiltSmooth = 5f;

    [SerializeField] private float screenMarginX = 0.05f; // 5% экрана
    [SerializeField] private float screenMarginY = 0.08f;

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 moveInput;

    private Quaternion baseRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        baseRotation = transform.localRotation;
    }

    
    void Update()
    {
        ClampToScreen();
        Tilt();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveInput != Vector3.zero) // Проверяем, есть ли ввод движения
        {
            rb.AddForce(moveInput * moveForce, ForceMode.Acceleration);

            if (rb.linearVelocity.magnitude > maxSpeed) // Ограничиваем скорость корабля
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
        else // Если нет ввода движения, применяем замедление
        {
            rb.linearVelocity *= drag;
        }
    }

    // Метод для наклона корабля в зависимости от направления движения
    private void Tilt() 
    {
        float tiltX = -moveInput.z * tiltAngleX;
        float tiltY = -moveInput.x * tiltAngleY;

        Quaternion tiltRotation = Quaternion.Euler(tiltX, tiltY, 0f);
        Quaternion targetRotation = baseRotation * tiltRotation;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSmooth);
    }

    private void ClampToScreen()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp(viewPos.x, screenMarginX, 1 - screenMarginX);
        viewPos.y = Mathf.Clamp(viewPos.y, screenMarginY, 1 - screenMarginY);

        Vector3 clampedWorldPos = mainCamera.ViewportToWorldPoint(viewPos);
        clampedWorldPos.y = transform.position.y; // сохраняем высоту

        transform.position = clampedWorldPos;
    }
}
