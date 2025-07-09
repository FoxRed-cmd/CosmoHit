using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 entryTarget = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 exitDirection = Vector3.back;
    [SerializeField] private float entrySpeed = 10f;
    [SerializeField] private float exitSpeed = 15f;

    [Header("Timing")]
    [SerializeField] private float stayDuration = 3f;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAngleX = 15f; // Наклон вперёд/назад (Z движение)
    [SerializeField] private float tiltAngleZ = 20f; // Наклон влево/вправо (X движение)
    [SerializeField] private float tiltSmooth = 5f;
    private Quaternion baseRotation = Quaternion.Euler(0f, 270f, 0f);

    [Header("Swerve (Staying wiggle)")]
    [SerializeField] private float swerveInterval = 0.5f; // интервал между рывками
    [SerializeField] private float swerveDistance = 2f;   // расстояние рывка
    [SerializeField] private float swerveSpeed = 20f;     // скорость рывка

    [Header("Smooth Swerve (sinusoidal)")]
    [SerializeField] private bool useSmoothSwerve = false;
    [SerializeField] private float swerveAmplitude = 2f;
    [SerializeField] private float swerveFrequency = 2f;

    private float baseXPosition;


    private State currentState = State.Entering;

    private float stayTimer = 0f;
    private Vector3 moveInput = Vector3.zero;

    private float swerveTimer = 0f;
    private int swerveDirection = 1; // 1 = вправо, -1 = влево
    private Vector3 swerveTarget;
    private bool isSwerveActive = false;
    private float swerveLocalTime = 0f;

    private BaseBulletSpawner bulletSpawner;

    private void Start()
    {
        baseXPosition = transform.position.x;
        if (TryGetComponent<BaseBulletSpawner>(out var bulletSpawner))
        {
            this.bulletSpawner = bulletSpawner;
            this.bulletSpawner.enabled = false;
        }
    }

    private void Update()
    {
        moveInput = Vector3.zero;

        switch (currentState)
        {
            case State.Entering:
                moveInput = (entryTarget - transform.position).normalized;
                transform.position += moveInput * entrySpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, entryTarget) < 0.1f)
                {
                    currentState = State.Staying;
                    baseXPosition = transform.position.x;
                    swerveLocalTime = 0f;
                    stayTimer = 0f;

                    bulletSpawner.enabled = true;
                }
                break;

            case State.Staying:
                
                stayTimer += Time.deltaTime;
                Swerve();
                if (stayTimer >= stayDuration)
                {
                    currentState = State.Exiting;
                    bulletSpawner.enabled = false;
                }
                break;

            case State.Exiting:
                moveInput = exitDirection.normalized;
                transform.position += moveInput * exitSpeed * Time.deltaTime;

                if (IsOffScreen())
                {
                    Destroy(gameObject);
                }
                break;
        }

        Tilt();
    }

    private void Tilt()
    {
        float tiltZ = -moveInput.z * tiltAngleZ;
        float tiltX = -moveInput.x * tiltAngleX;

        Quaternion tiltRotation = Quaternion.Euler(tiltX, 0f, tiltZ);
        Quaternion targetRotation = baseRotation * tiltRotation;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSmooth);
    }

    private bool IsOffScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x < -0.2f || viewportPos.x > 1.2f || viewportPos.y < -0.2f || viewportPos.y > 1.2f;
    }

    private void SimpleServe()
    {
        // Обновляем направление наклона во время рывка
        if (isSwerveActive)
        {
            Vector3 moveDir = (swerveTarget - transform.position);
            float distance = moveDir.magnitude;

            // Устанавливаем moveInput как направление рывка (для Tilt)
            moveInput = moveDir.normalized;

            if (distance > 0.1f)
            {
                transform.position += moveDir.normalized * swerveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = swerveTarget;
                isSwerveActive = false;
                moveInput = Vector3.zero; // наклон вернётся в нейтраль
            }
        }
        else
        {
            moveInput = Vector3.zero;
        }

        if (swerveTimer >= swerveInterval)
        {
            swerveDirection *= -1; // влево/вправо
            swerveTarget = transform.position + new Vector3(swerveDirection * swerveDistance, 0f, 0f);
            isSwerveActive = true;
            swerveTimer = 0f;
        }
    }

    private void SmoothSwerve()
    {
        swerveLocalTime += Time.deltaTime;
        float offsetX = Mathf.Sin(swerveLocalTime * swerveFrequency) * swerveAmplitude;
        Vector3 newPos = new Vector3(baseXPosition + offsetX, transform.position.y, transform.position.z);
        moveInput = new Vector3(Mathf.Cos(swerveLocalTime * swerveFrequency), 0f, 0f); // производная синуса = косинус — задаёт направление наклона
        transform.position = newPos;
    }

    private void Swerve()
    {
        swerveTimer += Time.deltaTime;

        if (useSmoothSwerve)
        {
            SmoothSwerve();
        }
        else
        {
            SimpleServe();
        }
    }

    public void Initialize(Vector3 target, Vector3 exitDir, float waitTime)
    {
        entryTarget = target;
        exitDirection = exitDir;
        stayDuration = waitTime;
    }
}
