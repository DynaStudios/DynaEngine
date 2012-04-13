using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynaStudios;

namespace TestGame
{
    public class Game : Engine
    {

        public Engine Engine;

        public Game()
        {
            this.Engine = new Engine();
        }

        static void Main(string[] args)
        {

            Game game = new Game();
            game.Engine.Run();


        }
    }
}
