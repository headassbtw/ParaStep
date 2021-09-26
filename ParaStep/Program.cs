using System;

namespace ParaStep
{
    public static class Program
    {
        public static Game1 Game;
        [STAThread] 
        static void Main()
        {
            Game = new Game1();
            Game.Run();
        }
    }
}