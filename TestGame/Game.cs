using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynaStudios;
using DynaStudios.UI.Controls;

namespace TestGame
{
    public class Game : Engine
    {
        public Game() : base()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Create Test User Interface
            UIPanel panel = new UIPanel();

            //Create Container for 2 TestButtons
            UIVerticalContainer vContainer = new UIVerticalContainer();
            vContainer.add(new UIButton() { Text = "Hallo Welt" });
            vContainer.add(new UIButton() { Text = "Ich bin der Zweite" });

            panel.setContainingPanel(vContainer);

            //Add Panel to UiController
            UiController.registerPanel(panel);

        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
