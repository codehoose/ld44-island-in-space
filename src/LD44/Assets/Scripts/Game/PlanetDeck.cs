using System.Collections;
using UnityEngine;

public class PlanetDeck : MonoBehaviour
{
    private readonly float[] xpos = new float[] { -80, 0 }; //{ -120, -40, 40 };
    private PlanetDeckFactory factory = new PlanetDeckFactory();
    private AudioSource audioSource;

    public PlanetCardBehaviour[] pool;
    public AudioClip[] wavs;
    public AudioClip shhwhh;

    public event PlanetCardClickedEventHandler CardClicked;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (var card in pool)
        {
            card.CardClicked += (s, e) =>
            {
                CardClicked?.Invoke(this, e);
            };
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
            
            var endPos = new Vector3(x, -112);
            var startPos = new Vector3(x, -224);
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

            var endPos = new Vector3(x, -224);
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
