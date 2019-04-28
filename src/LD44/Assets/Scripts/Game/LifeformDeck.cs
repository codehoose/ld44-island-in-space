using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeformDeck : MonoBehaviour
{
    private readonly float[] xpos = new float[] { -120, -40, 40 };
    private PlayerDeckFactory factory = new PlayerDeckFactory();
    private MusicBehaviour audioSource;

    public CardBehaviour[] pool;
    public AudioClip[] wavs;
    public AudioClip shhwhh;

    public event CardClickedEventHandler CardClicked;

    void Awake()
    {
        audioSource = GetComponent<MusicBehaviour>();

        foreach (var card in pool)
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
            if (card.IsFaceUp)
            {
                card.EnableClick(interactable);
            }
            else
            {
                card.EnableClick(false);
            }
        }
    }

    public IEnumerator Deal()
    {
        for (var i = 0; i < xpos.Length; i++)
        {
            var x = xpos[i];
            var cb = pool[i];

            var cardDetail = factory.Deal();
            cb.ApplyCard(cardDetail);

            var endPos = new Vector3(x, 0);
            var startPos = new Vector3(x, 112);
            cb.transform.localPosition = startPos;
            var time = 0f;
            var duration = 0.05f;
            audioSource.PlayOneShot(shhwhh);
            while (time < 1f)
            {
                var currentPos = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime / duration;
                cb.transform.localPosition = currentPos;
                yield return null;
            }
            cb.transform.localPosition = endPos;
            yield return new WaitForSeconds(0.5f);
        }

        for (var i = 0; i < xpos.Length; i++)
        {
            var cb = pool[i];
            cb.ShowFront(true);
            audioSource.PlayOneShot(wavs[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator UnDeal()
    {
        audioSource.PlayOneShot(shhwhh);
        for (var i = 0; i < xpos.Length; i++)
        {
            var x = xpos[i];
            var cb = pool[i];
            cb.ShowFront(false);

            var cardDetail = factory.Deal();
            cb.ApplyCard(cardDetail);

            var endPos = new Vector3(x, 112);
            var startPos = cb.transform.localPosition;
            var time = 0f;
            var duration = 0.05f;
            while (time < 1f)
            {
                var currentPos = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime / duration;
                cb.transform.localPosition = currentPos;
                yield return null;
            }
            cb.transform.localPosition = endPos;
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

    internal void ShowSelectionArrow(bool show)
    {
        foreach (var card in pool)
        {
            card.ShowSelectionArrow(show);
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
