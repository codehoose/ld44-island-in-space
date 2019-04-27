using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
    public LifeformDeck lifeformDeck;
    public PlanetDeck planetDeck;

    private GameState _state = GameState.Initial;

    void Awake()
    {
        lifeformDeck.CardClicked += LifeformDeck_CardClicked;
        planetDeck.CardClicked += PlanetDeck_CardClicked;
    }

    private void PlanetDeck_CardClicked(object sender, PlanetCardClickedEventArgs e)
    {
        
    }

    private void LifeformDeck_CardClicked(object sender, CardClickedEventArgs e)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //var btn = GetComponent<Button>();
        //btn.onClick.AddListener(new UnityAction(() => {
        //    lifeformDeck.Deal();
        //    planetDeck.Deal();
        //}));
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case GameState.Initial:
                _state = GameState.DealCards;
                break;
            case GameState.DealCards:
                lifeformDeck.Deal();
                planetDeck.Deal();
                _state = GameState.PlayerAction;
                break;
        }
    }
}
