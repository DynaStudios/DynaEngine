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
    public class Engine : GameWindow
    {

        public Logger Logger;

        private IDrawable cube1 = new Cube(0, 0, 0);
        private IDrawable cube2 = new Cube(0, 0, 1);
        private IDrawable cube3 = new Cube(1, 0, 0);
        private IDrawable cube4 = new Cube(0, 1, 0);

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
        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";
            GL.ClearColor(Color.Gray);

            //Hier kommt dann denke ich mal die Kamera hin?!

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            InputDevice = new InputDevice(Mouse, Keyboard);
            UiController = new GUIController(this);

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
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Render World Objects
            cube1.Render();
            cube2.Render();
            cube3.Render();
            cube4.Render();

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
