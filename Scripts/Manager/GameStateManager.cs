public class GameStateManager : GenericSingleton<GameStateManager>
{
    public GameState gameState;

    public GameState CurrentGameState { get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
        {
            return;
        }

        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }
}