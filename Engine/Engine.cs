using System;
using System.Drawing;
using OpenTK;
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

        public CammeraMan () {
            Direction = new Direction();
            Position = new WorldPosition();
        }
    }

    public class Engine : GameWindow
    {
        public Logger Logger;

        private Cammera cammera = new Cammera();
        public Cammera Cammera {
            get { return cammera; }
        }

        private Cube cube1 = new Cube(0, 0, 0);
        private Cube cube2 = new Cube(0, 0, 1);
        private Cube cube3 = new Cube(1, 0, 0);
        private Cube cube4 = new Cube(0, 1, 0);
        private CammeraMan cammerMan;

        /// <summary>
        /// You can add your GUI Elements to the UIController and also let them register to Mouse and Keyboard Events.
        /// 
        /// Add new UIPanels using UIController.register(UIPanel yourPanel);
        /// </summary>
        public GUIController UiController { get; set; }

        public Engine()
            : base(1024, 768, new GraphicsMode(32, 0, 0, 4))
        {
            Logger = new Logger();
            Logger.Register(new ConsoleLogger());
            Logger.Register(new FileSystemLogger());
            Logger.Debug("Init Game.");
            cammerMan = new CammeraMan();
            cammerMan.Position.z = -3.0;
            Cammera.WorldObject = cammerMan;
            cube1.color = Color.AliceBlue;
            cube2.color = Color.White;
            cube3.color = Color.Red;
            cube4.color = Color.Brown;
        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";
            GL.ClearColor(Color.Gray);

            //Hier kommt dann denke ich mal die Kamera hin?!
            InputDevice inputDevice = new InputDevice(Mouse, Keyboard);

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            UiController = new GUIController(inputDevice);

            resize(null, EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e)
        {
            Logger.Debug("OnResize called. New Width: " + base.Width + "px | New Height: " + base.Height + "px");
            resize(null, EventArgs.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            //Unload Libs etc. here
            Logger.Debug("Closing Application");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //cammerMan.Direction.y -= 0.004;
            //cammerMan.Position.z -= 0.0001;
            //Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //moves the cammera
            cammera.move();

            //Render World Objects
            cube1.Render();
            cube2.Render();
            cube3.Render();
            cube4.Render();

            // unmoves the cammera for the next frame
            cammera.unmove();

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
