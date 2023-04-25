using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] private float time;
    [SerializeField, Range(0, 7f)] private float speed;
    [SerializeField, Range(0, 10)] private int damage;
    private Vector2 dir;

    void Start()
    {
        Destroy(this.gameObject, time);
    }

    public void SetDir(Vector3 playerPos)
    {
        dir = (playerPos - transform.position).normalized;
        transform.right = playerPos - transform.position;
    }

    void Update()
    {
        transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerController>().Hurt(damage);
        if (col.transform.CompareTag("Barrel"))
            col.gameObject.GetComponent<Barrel>().Damaged(transform.position.x, this.transform);
        Destroy(this.gameObject);
    }
}
