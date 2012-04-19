using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Logger = DynaStudios.DynaLogger.Logger;
using DynaStudios.DynaLogger.Appender;
using DynaStudios.Blocks;
using DynaStudios.IO;
using DynaStudios.UI;
using DynaStudios.Utils;

namespace DynaStudios
{

    public class Engine : GameWindow
    {
        public Logger Logger;

        public List<Scene> LoadedScenes { get; set; }
        public Scene ActiveScene { get; set; }

        /// <summary>
        /// You can add your GUI Elements to the UIController and also let them register to Mouse and Keyboard Events.
        /// 
        /// Add new UIPanels using UIController.register(UIPanel yourPanel);
        /// </summary>
        public GUIController UiController { get; set; }
        public InputDevice InputDevice { get; set; }

        public FramerateCalculator FpsCalc;

        //

        public Engine()
            : base(1024, 768, new GraphicsMode(32, 1, 0, 4))
        {
            Logger = new Logger();
            Logger.Register(new ConsoleLogger());
            Logger.Register(new FileSystemLogger());
            Logger.Debug("Init Game.");

            FpsCalc = new FramerateCalculator();
            LoadedScenes = new List<Scene>();

            //


        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            InputDevice = new InputDevice(Mouse, Keyboard);
            UiController = new GUIController(this);

            

            resize(null, EventArgs.Empty);

            //Enable OpenGL Modes
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(Color.Gray);


        }

        protected override void OnResize(EventArgs e)
        {
            Logger.Debug("OnResize called. New Width: " + base.Width + "px | New Height: " + base.Height + "px");
            resize(null, EventArgs.Empty);
            UiController.resizeGui();
        }

        protected override void OnClosed(EventArgs e)
        {
            //Unload Libs etc. here
            Logger.Debug("Closing Application");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (ActiveScene != null)
            {
                ActiveScene.doRender();
            }

            //Start GUI/HUD Rendering here
            FpsCalc.CalculateFramePerSecond();
            UiController.render();

            this.SwapBuffers();
        }
        
        /// <summary>
        /// Loads given scene to Engine and actives.
        /// 
        /// If any other Scene is loaded it will get unloaded but not removed!
        /// </summary>
        /// <param name="scene"></param>
        public void loadScene(Scene scene)
        {
            if (ActiveScene != null)
            {
                ActiveScene.unloadScene();
                ActiveScene = null;
            }
            LoadedScenes.Add(scene);
            scene.loadScene();
            ActiveScene = scene;
        }

        /// <summary>
        /// Recalculates Viewport
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, base.Width, base.Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }
    }
}
