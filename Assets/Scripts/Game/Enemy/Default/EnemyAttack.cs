using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("General")]
        private Animator anim;
        [SerializeField] private Transform hitPoint;
    [Header("Attack")]
        [SerializeField, Range(0, 20)] private int damage;
        [SerializeField, Range(0, 1f)] private float attackArea;
        [SerializeField, Range(0, 5f)] private float cooldown;
        private float timer;
        private bool attacking = false;
        private bool canAttack = true;

    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            canAttack = true;
    }

    public void StartAttack()
    {
        if (!canAttack)
            return;
        anim.SetTrigger("Attack");
        timer = cooldown;
        canAttack = false;
        attacking = true;
    }

    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitPoint.position, attackArea);
        foreach(Collider2D col in colliders)
        {
            if (col.tag.Equals("Player"))
                col.GetComponent<PlayerController>().Hurt(damage);
            if (col.transform.CompareTag("Barrel"))
                col.gameObject.GetComponent<Barrel>().Damaged(transform.position.x, this.transform);
        }
        attacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitPoint.position, attackArea);
    }
}
