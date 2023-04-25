using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] private float time;
    [SerializeField, Range(0, 7f)] private float speed;
    [SerializeField, Range(0, 5)] private int damage;

    void Start()
    {
        Destroy(this.gameObject, time);
    }

    public void SetDir(int direction)
    {
        transform.localRotation = Quaternion.Euler(0, (direction > 0)? 0 : 180, 0);
    }

    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0 , 0));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Enemy"))
            col.gameObject.GetComponent<EnemyAI>().Hurt(damage, this.transform);
        if (col.transform.CompareTag("Boss"))
            col.gameObject.GetComponent<BossAI>().Hurt(damage, this.transform);
        if (col.transform.CompareTag("Barrel"))
            col.gameObject.GetComponent<Barrel>().Damaged(transform.position.x, this.transform);
        Destroy(this.gameObject);
    }
}
