namespace UserInterface
{
    public sealed class GameWindow : ScreenObserver
    {
        public override UIScreen Screen => UIScreen.Game;
    }
}