using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("General")]
        private Animator anim;
        private Rigidbody2D rb;
        private EnemyAI eAI;
    [Header("Movement")]
        [SerializeField, Range(0, 5f)] private float defaultSpeed;
        [SerializeField, Range(0, 1f)] private float soundDelay;
        private float soundTimer;
        private Vector2 dir = Vector2.zero;
        private float speed;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        eAI = this.GetComponent<EnemyAI>();
        
        soundTimer = soundDelay;
    }

    public void SetDir(Vector2 d)
    {
        speed = defaultSpeed;
        if (dir != d)
            dir = d;
        anim.SetBool("Running", dir != Vector2.zero);
    }

    public void Push(Vector2 d, float newSpeed)
    {
        speed = newSpeed;
        if (dir != d)
            dir = d;
    }

    void FixedUpdate()
    {
        rb.velocity = dir * speed;

        if (soundTimer < soundDelay)
        {
            soundTimer += Time.deltaTime;
            return;
        }
        if (dir.x != 0 || dir.y != 0)
        {
            eAI.ES.ESound?.PlaySound("Step");
            soundTimer = 0;
        }
    }
}
