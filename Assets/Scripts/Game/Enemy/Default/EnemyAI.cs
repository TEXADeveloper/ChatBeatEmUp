using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    [Header("General")]
        [HideInInspector] public EnemySpawner ES;
        private Transform player;
        private Animator anim;
        private PlayerController pc;
        private EnemyMovement em;
        private EnemyAttack ea;
        private float distance = 0;
        private int faceDir = 1;
        private bool died;
    [Header("Health")]
        [SerializeField, Range(0, 10)] private int maxHealth;
        private int health;
        [SerializeField] private float force;
        [SerializeField] private float forceTime;
        private Vector2 forceDir;
        private float timer;
        private bool pushing = false;
    [Header("Attack")]
        [SerializeField, Range(0, 3f)] private float attackDistance;


    void Start()
    {
        anim = this.GetComponent<Animator>();
        em = this.GetComponent<EnemyMovement>();
        ea = this.GetComponent<EnemyAttack>();
        player = GameObject.Find("Player").transform;
        pc = player.GetComponent<PlayerController>();

        health = maxHealth;
    }

    void Update()
    {
        if (player == null || pc.Died)
            return;
        if (pushing)
        {
            push();
            return;
        }
        if (move())
            return;
        attack();
    }

    private void push()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            pushing = false;
        em.Push(forceDir, force);
    }

    private bool move()
    {
        distance = Vector2.Distance(player.position, transform.position);
        Vector2 direction = player.position - transform.position;
        flip(direction);
        if (!died && distance > attackDistance && !ea.IsAttacking())
        {
            em.SetDir(direction.normalized);
            return true;
        }
        return false;
    }

    private void attack()
    {
        em.SetDir(Vector2.zero);
        if (!died)
            ea.StartAttack();
    }

    private void flip(Vector2 dir)
    {
        if (!died && ((dir.x > 0 && faceDir < 0) || (dir.x < 0 && faceDir > 0)))
        {
            transform.Rotate(Vector3.up * 180, Space.Self);
            faceDir = (dir.x < 0)? -1 : 1;
        }
    }

    public void Hurt(int amount, Transform player)
    {
        if (died)
            return;
        health -= amount;
        anim.SetTrigger("Hurt");
        pushing = true;
        timer = forceTime;
        forceDir = (transform.position - player.position).normalized;
        em.SetDir(Vector2.zero);
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
}
