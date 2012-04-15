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

            Logger.Debug("Create GUI Elements");
            //Create Test User Interface
            UIPanel panel = new UIPanel();

            //Create Container for 2 TestButtons
            UIVerticalContainer vContainer = new UIVerticalContainer();
            vContainer.add(new UIButton() { Text = "Hallo Welt", Position = UIPosition.Center });
            vContainer.add(new UIButton() { Text = "Exit Game", Position = UIPosition.Center });

            panel.setContainingPanel(vContainer);

            //Add Panel to UiController
            UiController.registerPanel(panel);

        }

        /// <summary>
        /// Delegate Click Handler for Exit Game TestButton
        /// </summary>
        public void OnExitGameClick()
        {
            Logger.Debug("Exit Game Button were clicked. Closing Game");
            Close();
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
