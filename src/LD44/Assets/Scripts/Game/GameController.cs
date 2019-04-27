using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(PlanetDeck))]
[RequireComponent(typeof(LifeformDeck))]
public class GameController : MonoBehaviour
{
    private LifeformDeck _lifeformDeck;
    private PlanetDeck _planetDeck;
    private PlanetCard _planetCard = null;
    private PlanetCardBehaviour _planetCardBehaviour = null;
    private IslandResources _resources;

    private GameState _state = GameState.Initial;

    public Button endTurn;
    public GaugeBehaviour water;
    public GaugeBehaviour mineral;
    public GaugeBehaviour wood;
    public GaugeBehaviour worship;

    void Awake()
    {
        _resources = new IslandResources();
        _lifeformDeck = GetComponent<LifeformDeck>();
        _planetDeck = GetComponent<PlanetDeck>();

        _lifeformDeck.CardClicked += LifeformDeck_CardClicked;
        _planetDeck.CardClicked += PlanetDeck_CardClicked;

        endTurn.onClick.AddListener(new UnityAction(() => {
            _state = GameState.RoundOver;
        }));
    }

    private void PlanetDeck_CardClicked(object sender, PlanetCardClickedEventArgs e)
    {
        if (_state == GameState.PlayerAction)
        {
            _state = GameState.SetupChooseLifeformCard;
            _planetCard = e.Details;
            _planetCardBehaviour = e.Card;
        }
    }

    private void LifeformDeck_CardClicked(object sender, CardClickedEventArgs e)
    {
        if (_state == GameState.ChooseLifeformCard)
        {
            _state = GameState.PlayerAction;
            _planetCard.ApplyAction(e.Details, e.Card);
            _planetCardBehaviour.ShowFront(false);
            _planetCard = null;
            _planetCardBehaviour = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case GameState.Initial:
                _state = GameState.DealCards;
                UpdateGauges();
                break;
            case GameState.DealCards:
                _lifeformDeck.Deal();
                _planetDeck.Deal();
                _state = GameState.PlayerAction;
                break;
            case GameState.PlayerAction:
                _planetDeck.EnableCards();
                _lifeformDeck.EnableCards(false);
                break;
            case GameState.SetupChooseLifeformCard:
                _planetDeck.EnableCards(false);
                _lifeformDeck.EnableCards();
                _state = GameState.ChooseLifeformCard;
                break;
            case GameState.RoundOver:
                EndTurn();
                break;
            case GameState.YouAreDead:
                _planetDeck.EnableCards(false);
                _lifeformDeck.EnableCards(false);
                endTurn.interactable = false;
                break;
        }
    }

    private void EndTurn()
    {
        _lifeformDeck.ApplyDeltas(_resources);
        UpdateGauges();
        if (_resources.IsDead)
            _state = GameState.YouAreDead;
        else
            _state = GameState.DealCards;
    }

    private void UpdateGauges()
    {
        water.Set(_resources.Water);
        wood.Set(_resources.Wood);
        mineral.Set(_resources.Minerals);
        worship.Set(_resources.Worship);
    }
}
