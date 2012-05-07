using System;
using DynaStudios;
using DynaStudios.UI.Controls;
using TestGame.Scenes;

namespace TestGame
{
    public class Game : Engine
    {
        public Game()
            : base(System.Reflection.Assembly.GetExecutingAssembly().Location + "/Maps")
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Logger.Debug("Create GUI Elements");
            //Create Test User Interface
            UIPanel panel = new UIPanel { VerticalPosition = UIVerticalPosition.Bottom, Height = 400 };
            //Create Container for 2 TestButtons
            UIVerticalContainer vContainer = new UIVerticalContainer { HorizontalPosition = UIHorizontalPosition.Left, VerticalPosition = UIVerticalPosition.Top };
            vContainer.add(new UiButton { Text = "Hallo Welt", HorizontalPosition = UIHorizontalPosition.Center });
            UiButton exitButton = new UiButton { Text = "Exit Game", HorizontalPosition = UIHorizontalPosition.Center };
            exitButton.Clicked += OnExitGameClick;
            vContainer.add(exitButton);

            panel.setContainingPanel(vContainer);

            //Add Panel to UiController
            UiController.RegisterPanel(panel);

            //Register Scene
            //IScene scene = new BlockTestScene(this);
            //addScene("blockTest", scene);
            //switchScene("blockTest");
            IScene scene = new StupedWorldScene(this);
            addScene("StupedWorldScene", scene);
            switchScene("StupedWorldScene");
        }

        /// <summary>
        /// Delegate Click Handler for Exit Game TestButton
        /// </summary>
        public void OnExitGameClick()
        {
            Logger.Debug("Exit Game Button were clicked. Closing Game");
            Close();
        }

        private static void Main()
        {
            Game game = new Game();
            game.Run();
        }
    }
}