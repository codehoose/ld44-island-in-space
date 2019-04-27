using UnityEngine;

public class PlanetDeck : MonoBehaviour
{
    private readonly float[] xpos = new float[] { -80, 0 }; //{ -120, -40, 40 };
    private PlanetDeckFactory factory = new PlanetDeckFactory();

    public PlanetCardBehaviour[] pool;

    public event PlanetCardClickedEventHandler CardClicked;

    void Awake()
    {
        foreach (var card in pool)
        {
            card.CardClicked += (s, e) =>
            {
                CardClicked?.Invoke(this, e);
            };
        }
    }

    public void Deal()
    {
        for (var i = 0; i < xpos.Length; i++)
        {
            var x = xpos[i];
            var cb = pool[i];

            var cardDetail = factory.Deal();
            cb.ApplyCard(cardDetail);
            cb.transform.localPosition = new Vector3(x, -112);
            cb.ShowFront(true);
        }
    }

    public void ClearCards()
    {
        foreach (var card in pool)
        {
            card.gameObject.transform.localPosition = new Vector3(-360, -112);
            card.ShowFront(false);
        }
    }

    internal void EnableCards(bool interactable = true)
    {
        foreach (var card in pool)
        {
            if (card.IsFaceUp())
                card.EnableClick(interactable);
            else
                card.EnableClick(false);
        }
    }
}
