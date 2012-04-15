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

namespace DynaStudios
{
    class CammeraMan : IWorldObject {
        public Direction Direction { get; set; }
        public WorldPosition Position { get; set; }
        private InputDevice input;

        public CammeraMan (InputDevice input) {
            this.input = input;
            Direction = new Direction();
            Position = new WorldPosition();
            input.Keyboard.KeyDown += Keyboard_KeyDown;
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            // TODO: movement must be relative to the direction
            switch (e.Key)
            {
                case (Key.A):
                    Position.x += 0.5;
                    break;
                case (Key.D):
                    Position.x -= 0.5;
                    break;
                case (Key.W):
                    Position.z += 0.5;
                    break;
                case (Key.S):
                    Position.z -= 0.5;
                    break;
                case (Key.Left):
                    Direction.y -= 16.0;
                    break;
                case (Key.Right):
                    Direction.y += 16.0;
                    break;
            }
        }
    }

    public class Engine : GameWindow
    {
        public Logger Logger;

        private Cammera cammera = new Cammera();
        public Cammera Cammera {
            get { return cammera; }
        }

        private List<AbstractDrawable> worldObjects = new List<AbstractDrawable>();
        private CammeraMan cammerMan;

        /// <summary>
        /// You can add your GUI Elements to the UIController and also let them register to Mouse and Keyboard Events.
        /// 
        /// Add new UIPanels using UIController.register(UIPanel yourPanel);
        /// </summary>
        public GUIController UiController { get; set; }
        public InputDevice InputDevice { get; set; }

        public Engine()
            : base(1024, 768, new GraphicsMode(32, 0, 0, 4))
        {
            Logger = new Logger();
            Logger.Register(new ConsoleLogger());
            Logger.Register(new FileSystemLogger());
            Logger.Debug("Init Game.");
            Cube cube1 = new Cube(0, 0, 0);
            Cube cube2 = new Cube(0, 0, 2);
            Cube cube3 = new Cube(2, 0, 0);
            Cube cube4 = new Cube(0, 2, 0);
            cube1.color = Color.AliceBlue;
            cube2.color = Color.White;
            cube3.color = Color.Red;
            cube4.color = Color.Brown;
            worldObjects.Add(cube1);
            worldObjects.Add(cube2);
            worldObjects.Add(cube3);
            worldObjects.Add(cube4);
        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";
            GL.ClearColor(Color.Gray);

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            InputDevice = new InputDevice(Mouse, Keyboard);
            UiController = new GUIController(this);

            cammerMan = new CammeraMan(InputDevice);
            cammerMan.Position.z = -3.0;
            Cammera.WorldObject = cammerMan;

            resize(null, EventArgs.Empty);
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
            GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //moves the cammera
            cammera.move();

            //Render World Objects
            foreach (AbstractDrawable worldObject in worldObjects)
            {
                // TODO: only one cube is visible right now but it should be 4!!!
                worldObject.doRender();
            }

            // unmoves the cammera for the next frame
            cammera.moveBack();

            //Start GUI/HUD Rendering here
            UiController.render();

            this.SwapBuffers();
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
