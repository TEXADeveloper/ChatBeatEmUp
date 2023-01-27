using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerSound ps;
    private int facingDir = 1;
    [Header("Movement")]
        [SerializeField] private float speed;
        [SerializeField, Range(0, 1f)] private float soundDelay;
        private float soundTimer;
        private Vector2 direction;
    [Header("Dash")]
        [SerializeField] private TrailRenderer tr;
        [SerializeField] private float dashSpeed;
        [SerializeField, Range(0f, 1f)] private float dashTime;
        private float timer = 0;
        [HideInInspector] public bool Dashing;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        ps = this.GetComponent<PlayerSound>();

        PlayerInput.Movement += changeDirection;
        PlayerInput.Dash += dash;

        soundTimer = soundDelay;
    }

    private void changeDirection(Vector2 dir)
    {
        if ((dir.x < 0 && facingDir > 0) || (dir.x > 0 && facingDir < 0))
        {
            transform.Rotate(Vector3.up * 180, Space.Self);
            facingDir = (dir.x < 0)? -1 : 1;
        }
        direction = dir;
        anim.SetBool("Running", (dir.x != 0 || dir.y != 0));
    }

    void FixedUpdate()
    {
        if (!Dashing)
            move();
        else if (timer > 0)
            timer -= Time.fixedDeltaTime;
        else if (Dashing)
        {
            tr.emitting = false;
            Dashing = false;
        }
    }

    private void move()
    {
        rb.velocity = direction * speed;

        if (soundTimer < soundDelay)
        {
            soundTimer += Time.fixedDeltaTime;
            return;
        }
        if (direction.x != 0 || direction.y != 0)
        {
            ps.PlaySound("Step");
            soundTimer = 0;
        }
    }

    private void dash()
    {
        if (!Dashing)
        {
            Dashing = true;
            rb.velocity = new Vector2(dashSpeed * facingDir, rb.velocity.y);
            timer = dashTime;
            anim.SetTrigger("Dash");
            tr.emitting = true;
            ps.PlaySound("Dash");
        }
    }

    void OnDisable()
    {
        PlayerInput.Movement -= changeDirection;
        PlayerInput.Dash -= dash;
    }
}
