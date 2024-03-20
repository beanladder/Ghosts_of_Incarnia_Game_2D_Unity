using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    [SerializeField] private float MoveSpeed = 1f;
    [SerializeField] float dashSpeed = 7f;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float dashCooldown = 1.5f; // Adjust the cooldown duration
    public static PlayerController Instance;
    private Vector2 movement;
    private BobbyDeol playerControls;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer SpriteRenderer;
    private KnockBack knockBack;
    float startingMoveSpeed;
    bool facingLeft = false;
    public bool isDashing = false;
    bool dashOnCooldown = false;
    public AudioSource DashAudioController;
    [SerializeField]
    // New variable to track dash cooldown



    private void Awake()
    {
        Instance = this;
        playerControls = new BobbyDeol();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = MoveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        FaceDirection();
        Move();

    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);




    }

    private void Move()
    {
        if (knockBack.GettingKnockedback)
        {
            return;

        }
        rb.MovePosition(rb.position + movement * (MoveSpeed * Time.fixedDeltaTime));

    }

    public void FaceDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mousePos.x < playerScreenPoint.x)
        {
            SpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            SpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    void Dash()
    {
        if (!isDashing && !dashOnCooldown)
        {
            DashAudioController.Play();
            isDashing = true;
            MoveSpeed *= dashSpeed;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
        else
        {
            DashAudioController.Stop();
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = 0.2f;
        float dashCD = dashCooldown;

        yield return new WaitForSeconds(dashTime);

        MoveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCD);

        isDashing = false;
        dashOnCooldown = false;
    }
}
