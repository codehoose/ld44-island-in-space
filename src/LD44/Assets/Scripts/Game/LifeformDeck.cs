using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeformDeck : MonoBehaviour
{
    private readonly float[] xpos = new float[] { -120, -40, 40 };
    private PlayerDeckFactory factory = new PlayerDeckFactory();

    public CardBehaviour[] pool;

    public event CardClickedEventHandler CardClicked;

    void Awake()
    {
        foreach(var card in pool)
        {
            card.CardClicked += (s, e) =>
            {
                CardClicked?.Invoke(this, e);
            };
        }
    }

    internal void EnableCards(bool interactable = true)
    {
        foreach (var card in pool)
        {
            card.EnableClick(interactable);
        }
    }

    public void Deal()
    {
        for(var i = 0; i< xpos.Length; i++)
        {
            var x = xpos[i];
            var cb = pool[i];

            var cardDetail = factory.Deal();
            cb.ApplyCard(cardDetail);
            cb.transform.localPosition = new Vector3(x, 0);
            cb.ShowFront(true);
        }   
    }

    public void ClearCards()
    {
        foreach (var card in pool)
        {
            card.gameObject.transform.localPosition = new Vector3(-360, 0);
            card.ShowFront(false);
        }
    }

    internal void ApplyDeltas(IslandResources resources)
    {
        foreach(var card in pool)
        {
            if (!card.IsFaceUp)
                continue;

            card.Details.Apply(resources);
        }

        resources.Generation++;
    }
}
