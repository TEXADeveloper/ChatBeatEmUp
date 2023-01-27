using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DisplayHealth dh;
    private Animator anim;
    private PlayerMovement pm;
    private PlayerAttack pa;
    [SerializeField, Range(0, 1f)] private float invulnerabilityTime;
    private float timer;
    [SerializeField, Range(0, 200)] private int maxHealth = 100;
    int health;
    public bool Died = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();
        pa = this.GetComponent<PlayerAttack>();
        health = maxHealth;
        dh.SetMaxHealth(maxHealth);
        dh.SetHealth(health);
    }

    void Update()
    {
        timer = (timer > 0)? timer - Time.deltaTime : timer;
    }

    public void Hurt(int amount)
    {
        if (Died || pm.Dashing || timer > 0)
            return;
        timer = invulnerabilityTime;
        health -= amount;
        dh.SetHealth(health);
        anim.SetTrigger("Hurt");
        if (health <= 0)
            die();
    }

    private void die()
    {
        Died = true;
        anim.SetTrigger("Die");
        anim.SetBool("Died", true);
        pm.enabled = false;
        pa.enabled = false;
    }
}
