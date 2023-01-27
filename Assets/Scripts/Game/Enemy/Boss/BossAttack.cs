using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("General")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform shootPoint;
        private Transform bulletParent;
        private Animator anim;
    [Header("Time")]
        [SerializeField, Range(0, 5f)] private float cooldown;
        private float timer;
        private bool attacking = false;
        private bool canAttack = true;

    public bool IsAttacking()
    {
        return attacking;
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    void Start()
    {
        anim = this.GetComponent<Animator>();
        bulletParent = GameObject.Find("BulletParent").transform;
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

    public void Shoot()
    {
        GameObject go = GameObject.Instantiate(projectile, shootPoint.position, Quaternion.identity, bulletParent);
        float distance = go.transform.position.x - transform.position.x;
        go.GetComponent<Bullet>().SetDir((distance > 0)? 1 : -1);
        attacking = false;
    }
}