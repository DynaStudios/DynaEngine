using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynaStudios;

namespace TestGame
{
    public class Game
    {

        public Engine engine;

        public Game()
        {
            engine = new Engine();
        }

        public void run()
        {
            engine.Run();
        }

        static void Main(string[] args)
        {

            Game game = new Game();
            game.run();


        }
    }
}
