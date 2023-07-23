using SDL2;
using SDL2.TTF;

namespace SDLsweeper; 

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        _ = SDL.Init(InitFlags.Everything);
        _ = TTF.Init();
        
        var game = new Game(12, 1);
        game.Start();
        while (game.IsRunning) {
            if(SDL.PollEvent(out Event e) >= 1)
                game.Update(e);
            game.Draw();
        }
    }
}