using Game.Core;

namespace Game.Events
{
    #region Game State Events
    public struct GameStateChangeEvent : IGameEvent
    {
        public GameState PreviousState { get; }
        public GameState NewState { get; }
        public GameStateChangeEvent(GameState previousState, GameState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    }

    public struct GameStartEvent : IGameEvent
    {
        public bool IsNewGame { get; }
        public GameStartEvent(bool isNewGame)
        {
            IsNewGame = isNewGame;
        }
    }

    public struct GameOverEvent : IGameEvent
    {
        public int FinalScore { get; }
        public int HighScore { get; }
        public bool IsNewHighScore { get; }
        public GameOverEvent(int finalScore, int highScore, bool isNewHighScore)
        {
            FinalScore = finalScore;
            HighScore = highScore;
            IsNewHighScore = isNewHighScore;
        }
    }

    public struct ScoreUpdateEvent : IGameEvent
    {
        public int ScoreChange { get; }
        public int newTotalScore { get; }
        public ScoreReason Reason { get; }
        public ScoreUpdateEvent(int scoreChange, int newTotalScore, ScoreReason reason)
        {
            ScoreChange = scoreChange;
            this.newTotalScore = newTotalScore;
            Reason = reason;
        }
    }

    public enum ScoreReason
    {
        MeteorDestroyed,
        PerfectShot,
        Combo,
        Bonus
    }
    #endregion

    #region Player Events
    public struct PlayerDeathEvent : IGameEvent
    {
        public int RemainingLives { get; }
        public DeathCause Cause { get; }
        public PlayerDeathEvent(int remainingLives, DeathCause cause)
        {
            RemainingLives = remainingLives;
            Cause = cause;
        }

    }

    public enum DeathCause
    {
        MeteorHit,
        Other
    }
    #endregion
}