public enum GameState
{
    Initial,
    DealCards,
    PlayerAction,
    SetupChooseLifeformCard,
    ChooseLifeformCard,
    RoundOver,
    YouAreDead,
    GameOver,
    // WaitingOnCoroutine - Special case -- no handler, up to transition to move to next state
    WaitingOnCoroutine
}
