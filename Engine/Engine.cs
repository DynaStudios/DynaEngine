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

        private Logger _logger;

        private IDrawable cube1 = new Cube(0, 0, 0);
        private IDrawable cube2 = new Cube(0, 0, 1);
        private IDrawable cube3 = new Cube(1, 0, 0);
        private IDrawable cube4 = new Cube(0, 1, 0);

        private GUIController _uiController;

        public Engine()
            : base(1024, 768, new GraphicsMode(32, 0, 0, 4))
        {
            _logger = new Logger();
            _logger.Register(new ConsoleLogger());
            _logger.Register(new FileSystemLogger());
            _logger.Debug("Init Game.");
        }

        protected override void OnLoad(EventArgs e)
        {
            _logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";
            GL.ClearColor(Color.Gray);

            //Hier kommt dann denke ich mal die Kamera hin?!
            InputDevice inputDevice = new InputDevice(Mouse, Keyboard);

            //Init User Interface Controller
            _uiController = new GUIController(inputDevice);

            resize(null, EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e)
        {
            _logger.Debug("OnResize called. New Width: " + base.Width + "px | New Height: " + base.Height + "px");
            resize(null, EventArgs.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            //Unload Libs etc. here
            _logger.Debug("Closing Application");
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
            _uiController.render();

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
