using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text text;
    [HideInInspector] public int score = 0;

    public void AddScore()
    {
        score++;
        text.text = score.ToString();
    }

    void Start()
    {
        text = this.GetComponent<TMP_Text>();
    }
}
