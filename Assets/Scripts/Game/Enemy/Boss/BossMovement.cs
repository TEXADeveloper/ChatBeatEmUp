using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("General")]
        private Animator anim;
        private Rigidbody2D rb;
        private BossAI bAI;
    [Header("Movement")]
        [SerializeField, Range(0, 5f)] private float speed;
        private Vector2 dir = Vector2.zero;

    public void SetDir(Vector2 d)
    {
        if (dir != d)
            dir = d;
        anim.SetBool("Running", dir != Vector2.zero);
    }

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        bAI = this.GetComponent<BossAI>();
    }

    void FixedUpdate()
    {
        rb.velocity = dir * speed;
    }
}