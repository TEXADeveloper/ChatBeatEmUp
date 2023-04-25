using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Config gameConfig;
    [SerializeField] private Scroll[] scrolls;
    [SerializeField] private TMP_InputField inputField;

    void Awake()
    {
        loadConfig();
    }

    private void loadConfig()
    {
        gameConfig.channelName = PlayerPrefs.GetString("Username", "texaplayer");
        gameConfig.configValues = new int[scrolls.Length];
        gameConfig.configValues[0] = PlayerPrefs.GetInt("MaxEnemies", 50);
        gameConfig.configValues[1] = PlayerPrefs.GetInt("Cooldown", 0);
        gameConfig.configValues[2] = PlayerPrefs.GetInt("CooldownPerUser", 5);
        gameConfig.configValues[3] = PlayerPrefs.GetInt("EnemiesAtATime", 1);
        gameConfig.volume = PlayerPrefs.GetFloat("Volume", 1);
        PlayerPrefs.Save();
        
    }

    void Start()
    {
        slider.value = gameConfig.volume;
        inputField.text = (gameConfig.channelName.Equals("texaplayer"))? "" : gameConfig.channelName;
    }

    public void UpdateVolume(float value)
    {
        gameConfig.volume = value;
        PlayerPrefs.SetFloat("Volume", gameConfig.volume);
        PlayerPrefs.Save();
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }

    public void UpdateName(string value)
    {
        gameConfig.channelName = (value.Length == 0)? "texaplayer" : value.ToLower();
        PlayerPrefs.SetString("Username", gameConfig.channelName);
        PlayerPrefs.Save();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
