using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("General")]
        [HideInInspector] public EnemySpawner ES;
        private Transform player;
        private Animator anim;
        private PlayerController pc;
        private BossMovement bm;
        private BossAttack ba;
        private float distance = 0;
        private int faceDir = 1;
        private bool died;
    [Header("Health")]
        [SerializeField, Range(0, 50)] private int maxHealth;
        private int health;
    [Header("Attack")]
        [SerializeField, Range(0, 10f)] private float shootDistance;
        [SerializeField, Range(0f, 5f)] private float runDistance;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        bm = this.GetComponent<BossMovement>();
        ba = this.GetComponent<BossAttack>();
        player = GameObject.Find("Player").transform;
        pc = player.GetComponent<PlayerController>();

        health = maxHealth;
    }

    void Update()
    {
        if (player == null || pc.Died)
        {
            bm.SetDir(Vector2.zero, false);
            return;
        }
        if (move())
            return;
        shoot();
    }

    private bool move()
    {
        distance = Vector2.Distance(player.position, transform.position);
        Vector2 direction = player.position - transform.position;
        flip(direction);
        if (!died && distance > shootDistance && !ba.IsAttacking() && ba.CanAttack())
        {
            bm.SetDir(direction.normalized, false);
            return true;
        } else if (!died && !ba.IsAttacking() && (distance < runDistance || !ba.CanAttack()))
        {
            bm.SetDir(-direction.normalized, true);
            return true;
        }
        return false;
    }

    private void flip(Vector2 dir)
    {
        if (!died && ((dir.x > 0 && faceDir < 0) || (dir.x < 0 && faceDir > 0)))
        {
            transform.Rotate(Vector3.up * 180, Space.Self);
            faceDir = (dir.x < 0)? -1 : 1;
        }
    }

    private void shoot()
    {
        bm.SetDir(Vector2.zero, false);
        if (!died)
            ba.StartAttack();
    }

    public void Hurt(int amount, Transform player)
    {
        if (died)
            return;
        health -= amount;
        anim.SetTrigger("Hurt");
        if (health <= 0)
            die();
    }

    private void die()
    {
        died = true;
        anim.SetTrigger("Die");
        anim.SetBool("Died", true);
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        ES.EnemyDied();
    }

    public void PlaySound(string name)
    {
        ES.ESound.PlaySound(name);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, shootDistance);
        Gizmos.DrawWireSphere(transform.position, runDistance);
    }
}
