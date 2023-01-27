using UnityEngine;

public class Barrel : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [SerializeField] private Sprite damagedSprite;


    [SerializeField] private float force;
    [SerializeField] private float forceTime;
    private Vector2 forceDir;
    private float timer;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            rb.velocity = Vector2.zero;
    }

    public void Damaged(float dir, Transform player)
    {
        sr.sprite = damagedSprite;
        sr.flipX = (dir > transform.position.x);
        forceDir = (transform.position - player.position).normalized;
        rb.AddForce(forceDir * force, ForceMode2D.Impulse);
        timer = forceTime;
    }
}
