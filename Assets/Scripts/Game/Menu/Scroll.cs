using UnityEngine;
using TMPro;

public class Scroll : MonoBehaviour
{
    [SerializeField] private Config gameConfig;
    [SerializeField, Range(0, 3)] private int pos;
    private TMP_InputField input;
    [HideInInspector] public int value;

    void Start()
    {
        input = this.GetComponent<TMP_InputField>();
        if (gameConfig.configValues != null)
            value = gameConfig.configValues[pos]; 
        else
        {
            gameConfig.configValues = new int[4];
            int.TryParse(input.text, out value); 
        }    
        input.text = value.ToString();
    }

    public void UpdateValue(int amount)
    {
        value = ((value + amount) < 0)? 0 : value + amount;
        input.text = value.ToString();
        gameConfig.configValues[pos] = value;
    } 

    public void ChangeValue(string amount)
    {
        int num;
        int.TryParse(amount, out num);
        value = (num < 0)? 0 : num;
        gameConfig.configValues[pos] = value;
    }
}
