using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public Sprite back;
    public Sprite front;
    public CanvasGroup uiElements;
    public Image background;
    public Image selectionArrow;

    public TextMeshProUGUI header;
    public TextMeshProUGUI body;

    public TextMeshProUGUI water;
    public TextMeshProUGUI wood;
    public TextMeshProUGUI mineral;
    public TextMeshProUGUI worship;

    public event CardClickedEventHandler CardClicked;

    public string Water
    {   
        set { water.text = value; }
    }

    public string Wood
    {
        set { wood.text = value; }
    }

    internal void EnableClick(bool interactable = true)
    {
        background.GetComponent<Button>().interactable = interactable;
    }

    public string Mineral
    {
        set { mineral.text = value; }
    }

    public string Worship
    {
        set { worship.text = value; }
    }

    public PlayerCard Details
    {
        get;
        private set;
    }

    public bool IsFaceUp
    {   
        get
        {
            return uiElements.alpha == 1;
        }
    }

    void Awake()
    {
        var button = background.gameObject.GetComponent<Button>();
        button.onClick.AddListener(new UnityAction(() =>
        {
            CardClicked?.Invoke(this, new CardClickedEventArgs(this, Details));
        }));
    }

    public void ApplyCard(PlayerCard card)
    {
        Details = card;
        header.text = card.Description;
        body.text = card.Body;

        ApplyValue(card.WaterDelta, s => Water = s);
        ApplyValue(card.WoodDelta, s => Wood = s);
        ApplyValue(card.MineralsDelta, s => Mineral = s);
        ApplyValue(card.WorshipDelta, s => Worship = s);
    }
    public void ShowSelectionArrow(bool show)
    {
        if (IsFaceUp)
        {
            selectionArrow.gameObject.SetActive(show);
        }
        else
        {
            selectionArrow.gameObject.SetActive(false);
        }
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
            ShowSelectionArrow(false);
        }
    }

    private void ApplyValue(int delta, Action<string> action)
    {
        if (delta==0)
        {
            action("");
            return;
        }

        if (delta < 0)
        {
            action("-" + Math.Abs(delta));
        }
        else
        {
            action("+" + Math.Abs(delta));
        }
    }
}
