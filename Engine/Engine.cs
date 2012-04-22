using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Logger = DynaStudios.DynaLogger.Logger;
using DynaStudios.DynaLogger.Appender;
using DynaStudios.IO;
using DynaStudios.UI;
using DynaStudios.Utils;
using DynaStudios.Blocks;

namespace DynaStudios
{
    public class Engine : GameWindow
    {
        public Logger Logger;

        public Dictionary<string, IScene> LoadedScenes { get; set; }
        public IScene ActiveScene { get; set; }
        private bool _sceneSwitchPending = false;
        private string _sceneSwitchName;

        private Camera _camera = new Camera();
        public Camera Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// You can add your GUI Elements to the UIController and also let them register to Mouse and Keyboard Events.
        /// 
        /// Add new UIPanels using UIController.register(UIPanel yourPanel);
        /// </summary>
        public GUIController UiController { get; set; }
        public InputDevice InputDevice { get; set; }
        public TextureController TextureManager { get; set; }
        //public SoundManager SoundManager { get; set; }
        public string DataPath { get; set; }

        public FramerateCalculator FpsCalc;

        public Engine(string dataPath)
            : base(1024, 768, new GraphicsMode(32, 1, 0, 4))
        {
            DataPath = dataPath;
            Logger = new Logger();
            Logger.Register(new ConsoleLogger());
            Logger.Register(new FileSystemLogger());
            Logger.Debug("Init Game.");

            FpsCalc = new FramerateCalculator();
            LoadedScenes = new Dictionary<string, IScene>();

        }

        public void forceResize()
        {
            resize(null, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            InputDevice = new InputDevice(Mouse, Keyboard);
            UiController = new GUIController(this);

            //Texture Manager
            TextureManager = new TextureController(this);
            //SoundManager = new SoundManager(this);

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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_sceneSwitchPending)
            {
                loadScene(LoadedScenes[_sceneSwitchName]);
            }

            if (ActiveScene != null)
            {
                //moves the camera
                _camera.move();

                ActiveScene.doRender();

                // unmoves the camera for the next frame
                _camera.moveBack();
            }

            //Start GUI/HUD Rendering here
            FpsCalc.CalculateFramePerSecond();
            //UiController.render();

            this.SwapBuffers();
        }

        public void addScene(string name, IScene scene)
        {
            LoadedScenes.Add(name, scene);
        }

        public void switchScene(string sceneName)
        {
            _sceneSwitchPending = true;
            _sceneSwitchName = sceneName;
        }

        /// <summary>
        /// Loads given scene to Engine and actives.
        /// 
        /// If any other Scene is loaded it will get unloaded but not removed!
        /// </summary>
        /// <param name="scene"></param>
        private void loadScene(IScene scene)
        {
            if (ActiveScene != null)
            {
                unloadScene(ActiveScene);
                ActiveScene = null;
            }
            scene.loadScene();
            ActiveScene = scene;

            _sceneSwitchName = null;
            _sceneSwitchPending = false;
        }

        private void unloadScene(IScene scene)
        {
            ActiveScene.unloadScene();
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
