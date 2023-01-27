using UnityEngine;

public class ObjectLayer : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sr.sortingOrder = (int) Mathf.Round(transform.position.y * -100);
    }
}
