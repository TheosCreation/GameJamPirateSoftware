using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sword : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public ParticleSystem swordSwing;
    private PlayerController player;
    public float rotationSpeed = 10f;
    [HideInInspector] public float originalRotationSpeed = 0f;

    private float currentSwingSpeed;
    private float maxSwingSpeed;
    private float previousRotation;
    public float rotationOffset = 0;
    [SerializeField] private float previousSwingSpeed = -1000f;
    [SerializeField] private float swingSpeedThreshold = 10f;
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private float shakeFrequency = 300f;
    [SerializeField] private float shakeDuration = 0.25f;

    private AudioSource audioSource;
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private float minVolume = 0.5f;
    [SerializeField] private float maxVolume = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        swordSwing = GetComponentInChildren<ParticleSystem>();
        player = GetComponentInParent<PlayerController>();
        originalRotationSpeed = rotationSpeed;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        RotateTowardsMouse();
        CalculateSwingSpeed();
    }

    private void CalculateSwingSpeed()
    {
        float rotationDelta = Mathf.DeltaAngle(previousRotation, rb.rotation);
        currentSwingSpeed = Mathf.Abs(rotationDelta / Time.fixedDeltaTime);

        if (currentSwingSpeed > maxSwingSpeed)
        {
            maxSwingSpeed = currentSwingSpeed;
        }

  

        float speedChange = previousSwingSpeed - currentSwingSpeed;
        if (speedChange > swingSpeedThreshold)
        {
            float shakeIntensity = Mathf.Lerp(shakeMagnitude, shakeMagnitude * 2, speedChange / swingSpeedThreshold);
            float shakeFreq = Mathf.Lerp(shakeFrequency, shakeFrequency * 2, speedChange / swingSpeedThreshold);
            player.TriggerScreenShake(shakeIntensity, shakeFreq, shakeDuration);

            float speedRatio = currentSwingSpeed / maxSwingSpeed;
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, 1 - speedRatio);
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, 1 - speedRatio);
            audioSource.Play();

        }

        previousSwingSpeed = currentSwingSpeed;
        previousRotation = rb.rotation;
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float angle = rotationOffset + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rotation);
    }

    public float GetCurrentSwingSpeed()
    {
        return currentSwingSpeed;
    }

    public float GetMaxSwingSpeed()
    {
        return maxSwingSpeed;
    }
}
