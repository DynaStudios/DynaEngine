using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynaStudios;
using DynaStudios.UI.Controls;
using TestGame.Scenes;

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
            UIPanel panel = new UIPanel() { VerticalPosition = UIVerticalPosition.Bottom, Height = 400 };

            //Create Container for 2 TestButtons
            UIVerticalContainer vContainer = new UIVerticalContainer() { HorizontalPosition = UIHorizontalPosition.Left, VerticalPosition = UIVerticalPosition.Top };
            vContainer.add(new UIButton() { Text = "Hallo Welt", HorizontalPosition = UIHorizontalPosition.Center });

            UIButton exitButton = new UIButton() { Text = "Exit Game", HorizontalPosition = UIHorizontalPosition.Center };
            exitButton.OnClicked += new ClickedEventHandler(OnExitGameClick);
            vContainer.add(exitButton);

            panel.setContainingPanel(vContainer);

            //Add Panel to UiController
            UiController.registerPanel(panel);

            //Register Scene
            IScene scene = new BlockTestScene(this);
            loadScene(scene);

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
