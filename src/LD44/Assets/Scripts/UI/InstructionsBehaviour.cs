using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsBehaviour : MonoBehaviour
{
    bool firstPage = true;
    public Button back;
    public Button howToPlay;
    public Button shamelessPlug;
    public TextMeshProUGUI howToPlayLabel;
    public TextMeshProUGUI firstPageText;
    public TextMeshProUGUI secondPageText;

    void Awake()
    {
        firstPageText.alpha = 1;
        secondPageText.alpha = 0;

        back.onClick.AddListener(new UnityAction(()=>
        {
            SceneManager.LoadScene("main");
        }));

        shamelessPlug.onClick.AddListener(new UnityAction(() =>
        {
            Application.OpenURL("https://www.youtube.com/c/sloankelly");
        }));

        howToPlay.onClick.AddListener(new UnityAction(()=> {
            firstPage = !firstPage;
            if (firstPage)
            {
                firstPageText.alpha = 1f;
                secondPageText.alpha = 0f;
                howToPlayLabel.text = "How to Play";
            }
            else
            {
                firstPageText.alpha = 0f;
                secondPageText.alpha = 1f;
                howToPlayLabel.text = "Overview";
            }
        }));
    }
}
