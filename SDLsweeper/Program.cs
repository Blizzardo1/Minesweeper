namespace SDLsweeper; 

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var game = new Game(12, 1);
        game.Start();
        while (game.IsRunning) {
            game.Update();
            game.Draw();
        }
    }
}