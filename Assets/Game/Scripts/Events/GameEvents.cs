using Game.Core;

namespace Game.Events
{
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

    //public struct ScoreUpdateEvent : IGameEvent
    //{
    //    public int ScoreChange { get; }
    //    public int newTotalScore { get; }
    //    public ScoreReason Reason { get; }
    //    public ScoreUpdateEvent(int scoreChange, int newTotalScore, ScoreReason reason)
    //    {
    //        ScoreChange = scoreChange;
    //        this.newTotalScore = newTotalScore;
    //        Reason = reason;
    //    }
    //}

    public struct ScoreChangeEvent : IGameEvent
    {
        public ScoreReason ScoreReason { get; }
        public int ScoreChange { get; }

        public ScoreChangeEvent(ScoreReason scoreReason, int scoreChange)
        {
            ScoreReason = scoreReason;
            ScoreChange = scoreChange;
        }
    }

    public struct TotalScoreUpdateEvent : IGameEvent
    {
        public int NewTotalScore { get; }
        public TotalScoreUpdateEvent(int newTotalScore)
        {
            NewTotalScore = newTotalScore;
        }
    }

    public enum ScoreReason
    {
        MeteorDestroyed,
        PerfectShot,
        Combo,
        Bonus
    }
}