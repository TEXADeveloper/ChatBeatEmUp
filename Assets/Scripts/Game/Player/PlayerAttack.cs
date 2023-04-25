using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    [Header("Attack")]
        [SerializeField, Range(0, 2f)] private float attackArea; 
        [SerializeField, Range(0, 10)] private int attackDamage;
    [Header("Stab")]
        [SerializeField, Range(0, 2f)] private float stabArea; 
        [SerializeField, Range(0, 10)] private int stabDamage;
    [Header("Shoot")]
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private Transform bulletParent;

    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitPoint.position, attackArea);
        foreach(Collider2D col in colliders)
        {
            if (col.transform.CompareTag("Enemy"))
                col.GetComponent<EnemyAI>().Hurt(attackDamage, this.transform);
            if (col.transform.CompareTag("Boss"))
                col.GetComponent<BossAI>().Hurt(attackDamage, this.transform);
            if (col.transform.CompareTag("Barrel"))
                col.gameObject.GetComponent<Barrel>().Damaged(transform.position.x, this.transform);
        } 
    }

    public void Shoot()
    {
        GameObject go = GameObject.Instantiate(bulletObject, hitPoint.position, Quaternion.identity, bulletParent);
        float distance = go.transform.position.x - transform.position.x;
        go.GetComponent<Bullet>().SetDir((distance > 0)? 1 : -1);
    } 

    public void Stab()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitPoint.position, stabArea);
        foreach(Collider2D col in colliders)
        {
            if (col.transform.CompareTag("Enemy"))
                col.GetComponent<EnemyAI>().Hurt(stabDamage, this.transform);
            if (col.transform.CompareTag("Boss"))
                col.GetComponent<BossAI>().Hurt(attackDamage, this.transform);
            if (col.transform.CompareTag("Barrel"))
                col.gameObject.GetComponent<Barrel>().Damaged(transform.position.x, this.transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitPoint.position, attackArea);
        Gizmos.DrawWireSphere(hitPoint.position, stabArea);
    }
}
