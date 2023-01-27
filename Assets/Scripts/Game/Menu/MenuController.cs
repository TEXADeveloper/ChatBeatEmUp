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

    void Start()
    {
        slider.value = (gameConfig.volume == 0)? slider.value : gameConfig.volume;
        inputField.text = (gameConfig.channelName.Length == 0)? "" : gameConfig.channelName;
    }

    public void UpdateVolume(float value)
    {
        gameConfig.volume = value;
        mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }

    public void UpdateName(string value)
    {
        gameConfig.channelName = (value.Length == 0)? "texaplayer" : value.ToLower();
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
