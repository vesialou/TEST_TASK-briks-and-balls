namespace BricksAndBalls.UI.Popups
{
    public struct GameOverPopupResult
    {
        public enum ResultType
        {
            None,
            Menu,
            Replay,
            Next
        }
        
        public ResultType State;
    }
}