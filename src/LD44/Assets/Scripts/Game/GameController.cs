using System.Collections;
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
    bool musicPlaying;

    public Button endTurn;
    public Button gameOver;
    public Button musicButton;
    public GaugeBehaviour water;
    public GaugeBehaviour mineral;
    public GaugeBehaviour wood;
    public GaugeBehaviour worship;
    public TextMeshProUGUI generation;
    public TextMeshProUGUI highestWorshipLevel;
    public TextMeshProUGUI furthestGeneration;
    public TextMeshProUGUI musicLabel;

    public CanvasGroup gameOverPanel;

    public AudioClip selectCard;
    public AudioClip attackCard;
    public AudioClip endTurnAudio;

    private MusicBehaviour audioSource;

    private int highestWorship = 5;
    
    void Awake()
    {
        _resources = new IslandResources();
        _lifeformDeck = GetComponent<LifeformDeck>();
        _planetDeck = GetComponent<PlanetDeck>();
        audioSource = GetComponent<MusicBehaviour>();
        highestWorship = _resources.Worship;

        _lifeformDeck.CardClicked += LifeformDeck_CardClicked;
        _planetDeck.CardClicked += PlanetDeck_CardClicked;

        endTurn.onClick.AddListener(new UnityAction(() => {
            _state = GameState.RoundOver;
        }));

        gameOver.onClick.AddListener(new UnityAction(()=> {
            SceneManager.LoadScene("main");
        }));

        musicPlaying = PlayerPrefs.GetInt("music", 1) == 1;
        if (musicPlaying)
            musicLabel.text = "Music On";
        else
            musicLabel.text = "Music Off";

        var musicController = GetComponent<MusicBehaviour>();
        musicButton.onClick.AddListener(new UnityAction(()=> {
            musicPlaying = !musicPlaying;
            if (musicPlaying)
                musicLabel.text = "Music On";
            else
                musicLabel.text = "Music Off";
            musicController.ToggleMusic(musicPlaying);
        }));
    }

    private void PlanetDeck_CardClicked(object sender, PlanetCardClickedEventArgs e)
    {
        if (_state == GameState.PlayerAction)
        {
            _state = GameState.SetupChooseLifeformCard;
            _planetCard = e.Details;
            _planetCardBehaviour = e.Card;
            audioSource.PlayOneShot(selectCard);
        }
    }

    private void LifeformDeck_CardClicked(object sender, CardClickedEventArgs e)
    {
        if (_state == GameState.ChooseLifeformCard)
        {
            _state = GameState.PlayerAction;
            _planetCard.ApplyAction(e.Details, e.Card);
            _planetCardBehaviour.ShowFront(false);
            _lifeformDeck.ShowSelectionArrow(false);
            _planetCard = null;
            _planetCardBehaviour = null;
            audioSource.PlayOneShot(attackCard);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case GameState.Initial:
                _state = GameState.DealCards;
                endTurn.interactable = false;
                UpdateGauges();
                break;
            case GameState.DealCards:
                StartCoroutine(DealCards());
                _state = GameState.WaitingOnCoroutine;
                break;
            case GameState.PlayerAction:
                _planetDeck.EnableCards();
                _lifeformDeck.EnableCards(false);
                break;
            case GameState.SetupChooseLifeformCard:
                _planetDeck.EnableCards(false);
                _lifeformDeck.EnableCards();
                _lifeformDeck.ShowSelectionArrow(true);
                _state = GameState.ChooseLifeformCard;
                break;
            case GameState.RoundOver:
                StartCoroutine(EndTheRound());
                break;
            case GameState.YouAreDead:
                _planetDeck.EnableCards(false);
                _lifeformDeck.EnableCards(false);
                endTurn.interactable = false;
                gameOverPanel.interactable = true;
                gameOverPanel.blocksRaycasts = true;
                gameOverPanel.alpha = 1f;
                highestWorshipLevel.text = highestWorship.ToString();
                furthestGeneration.text = _resources.Generation.ToString();
                _state = GameState.GameOver;
                break;
        }
    }

    private IEnumerator EndTheRound()
    {
        audioSource.PlayOneShot(endTurnAudio);
        endTurn.interactable = false;
        EndTurn();
        if (_state == GameState.YouAreDead)
        {
            yield break;
        }
        else
        {
            yield return _planetDeck.UnDeal();
            yield return _lifeformDeck.UnDeal();
            yield return new WaitForSeconds(2);
            _state = GameState.DealCards;
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
            _state = GameState.WaitingOnCoroutine;
    }

    private void UpdateGauges()
    {
        water.Set(_resources.Water);
        wood.Set(_resources.Wood);
        mineral.Set(_resources.Minerals);
        worship.Set(_resources.Worship);
        generation.text = string.Format("{0:000}", _resources.Generation);
    }

    private IEnumerator DealCards()
    {
        endTurn.interactable = false;
        yield return _lifeformDeck.Deal();
        yield return new WaitForSeconds(1f);
        yield return _planetDeck.Deal();
        _state = GameState.PlayerAction;
        endTurn.interactable = true;
    }
}
