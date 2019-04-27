using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
    public Button gameOver;
    public GaugeBehaviour water;
    public GaugeBehaviour mineral;
    public GaugeBehaviour wood;
    public GaugeBehaviour worship;
    public TextMeshProUGUI generation;
    public TextMeshProUGUI highestWorshipLevel;
    public CanvasGroup gameOverPanel;

    private int highestWorship = 0;

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

        gameOver.onClick.AddListener(new UnityAction(()=> {
            SceneManager.LoadScene("main");
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
                gameOverPanel.interactable = true;
                gameOverPanel.blocksRaycasts = true;
                gameOverPanel.alpha = 1f;
                highestWorshipLevel.text = highestWorship.ToString();
                _state = GameState.GameOver;
                break;
        }
    }

    private void EndTurn()
    {
        _lifeformDeck.ApplyDeltas(_resources);

        // Store the player's highest worship
        if (_resources.Worship > highestWorship)
        {
            highestWorship = _resources.Worship;
        }

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
        generation.text = string.Format("{0:000}", _resources.Generation);
    }
}
