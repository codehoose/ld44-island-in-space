using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour
{
    private bool musicPlaying;

    public Button play;
    public Button instructions;
    public Button musicToggle;
    public Button quit;
    public TextMeshProUGUI musicLabel;

    void Awake()
    {
        musicPlaying = PlayerPrefs.GetInt("music", 1) == 1;
        if (musicPlaying)
            musicLabel.text = "Music: On";
        else
            musicLabel.text = "Music: Off";

        play.onClick.AddListener(new UnityAction(() => {
            SceneManager.LoadScene("game");
        }));

        instructions.onClick.AddListener(new UnityAction(() => {
            SceneManager.LoadScene("instructions");
        }));

        musicToggle.onClick.AddListener(new UnityAction(() => {
            musicPlaying = !musicPlaying;
            PlayerPrefs.SetInt("music", musicPlaying ? 1 : 0);
            if (musicPlaying)
                musicLabel.text = "Music: On";
            else
                musicLabel.text = "Music: Off";
        }));

        quit.onClick.AddListener(new UnityAction(() => {
            Application.Quit();
        }));
    }
}
