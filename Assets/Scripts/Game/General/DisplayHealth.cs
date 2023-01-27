using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = this.GetComponent<Slider>();
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }
}
