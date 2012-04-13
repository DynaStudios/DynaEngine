using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DynaStudios.DynaLogger;

using Logger = DynaStudios.DynaLogger.Logger;
using DynaStudios.DynaLogger.Appender;

namespace DynaStudios
{
    public class Engine : GameWindow
    {

        private Logger _logger;

        public Engine() : base(800, 600, new GraphicsMode(32, 0, 0, 4))
        {
            _logger = new Logger();
            _logger.Register(new ConsoleLogger());
            _logger.Debug("Init Game.");
        }

        protected override void OnLoad(EventArgs e)
        {
            _logger.Debug("Called OnLoad();");
            GL.ClearColor(Color.Gray);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(0.0f, -1.0f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();

            this.SwapBuffers();
        }

    }
}
