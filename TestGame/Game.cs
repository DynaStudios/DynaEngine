using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynaStudios;

namespace TestGame
{
    public class Game : Engine
    {
        public Game() : base()
        {}

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
