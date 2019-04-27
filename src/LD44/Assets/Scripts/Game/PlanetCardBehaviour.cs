using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlanetCardBehaviour : MonoBehaviour
{
    public Sprite back;
    public Sprite front;
    public CanvasGroup uiElements;
    public Image background;

    public TextMeshProUGUI header;
    public TextMeshProUGUI body;

    public event PlanetCardClickedEventHandler CardClicked;

    public PlanetCard Card
    {
        get;
        private set;
    }

    void Awake()
    {
        var button = background.gameObject.GetComponent<Button>();
        button.onClick.AddListener(new UnityAction(() =>
        {
            CardClicked?.Invoke(this, new PlanetCardClickedEventArgs(this, Card));
        }));
    }

    public void EnableClick(bool enable = true)
    {
        background.GetComponent<Button>().interactable = enable;
    }

    public void ApplyCard(PlanetCard card)
    {
        Card = card;
        header.text = card.Description;
        body.text = card.Body;
    }

    public void ShowFront(bool showFront)
    {
        if (showFront)
        {
            background.sprite = front;
            uiElements.alpha = 1;
        }
        else
        {
            background.sprite = back;
            uiElements.alpha = 0;
        }
    }

    public bool IsFaceUp()
    {
        return uiElements.alpha == 1;
    }
}
