using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform bodyVisual;
    [SerializeField] private Transform armPivot;

    [Header("Arm Aim")]
    [SerializeField] private float armRotateSpeed = 120f;
    [SerializeField] private float armMinAngle = -60f;
    [SerializeField] private float armMaxAngle = 60f;

    [Header("Collect")]
    [SerializeField] private float collectRadius = 1.5f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Transform collectPoint;

    [Header("Footstep Audio")]
    [SerializeField] private float walkInterval = 0.4f;
    [SerializeField] private float runInterval = 0.25f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

    [SerializeField] private float staminaDrain = 25f;
    [SerializeField] private float staminaRegen = 15f;

    [SerializeField] private float regenDelay = 1f;

    private float regenTimer;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float armInput;
    private float currentArmAngle;
    private float footstepTimer;

    private bool isSprinting;
    private bool sprintButtonHeld;
    private bool facingRight = true;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int HitHash = Animator.StringToHash("Hit");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    private void Update()
    {
        HandleStamina();
        isSprinting = sprintButtonHeld && currentStamina > 0f;

        HandleFlip();
        HandleAnimation();
        HandleArmRotation();
        HandleFootstepSound();
    }

    private void FixedUpdate()
    {
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        Vector2 v = rb.linearVelocity;
        v.x = moveInput.x * currentSpeed;
        rb.linearVelocity = v;
    }

    private void HandleFlip()
    {
        if (moveInput.x > 0.01f && !facingRight)
        {
            Flip(true);
        }
        else if (moveInput.x < -0.01f && facingRight)
        {
            Flip(false);
        }
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;

        if (bodyVisual != null)
        {
            Vector3 scale = bodyVisual.localScale;
            scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            bodyVisual.localScale = scale;
        }
    }

    private void HandleAnimation()
    {
        if (anim == null) return;

        float animSpeed = 0f;

        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            animSpeed = isSprinting ? 1f : 0.5f;
        }

        anim.SetFloat(SpeedHash, animSpeed);
    }

    private void HandleArmRotation()
    {
        if (armPivot == null) return;

        currentArmAngle += armInput * armRotateSpeed * Time.deltaTime;
        currentArmAngle = Mathf.Clamp(currentArmAngle, armMinAngle, armMaxAngle);

        armPivot.localRotation = Quaternion.Euler(0f, 0f, currentArmAngle);
    }

    private void HandleFootstepSound()
    {
        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            if (AudioManager.Instance != null)
            {
                if (isSprinting && currentStamina > 0f)
                {
                    AudioManager.Instance.PlayRunLoop();
                }
                else
                {
                    AudioManager.Instance.PlayFootstep();
                }
            }
        }
        else
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopLoop();
            }
        }
    }

    public void MoveLeftDown() => moveInput = Vector2.left;
    public void MoveRightDown() => moveInput = Vector2.right;
    public void MoveButtonUp() => moveInput = Vector2.zero;

    public void AimUpDown() => armInput = -1f;
    public void AimDownDown() => armInput = 1f;
    public void AimButtonUp() => armInput = 0f;

    public void SprintDown() => sprintButtonHeld = true;
    public void SprintUp() => sprintButtonHeld = false;

    public void CollectItemButton()
    {
        if (collectPoint == null)
        {
            Debug.LogError("ยังไม่ได้ใส่ CollectPoint ใน Inspector");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            collectPoint.position,
            collectRadius,
            itemLayer
        );

        Debug.Log("เจอของ: " + hits.Length);

        foreach (var hit in hits)
        {
            CollectibleItem item = hit.GetComponent<CollectibleItem>();
            if (item != null)
            {
                item.TryCollect();
                return;
            }
        }

        Debug.Log("ไม่มีของในระยะ");
    }

    private void OnDrawGizmosSelected()
    {
        if (collectPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(collectPoint.position, collectRadius);
    }

    private void HandleStamina()
    {
        if (sprintButtonHeld && Mathf.Abs(moveInput.x) > 0.01f)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            regenTimer = 0f;
        }
        else
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }

    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }
}