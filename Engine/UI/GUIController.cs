using System.Collections.Generic;
using DynaStudios.UI.Controls;
using DynaStudios.IO;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.UI
{
    public class GUIController
    {

        private List<UIPanel> _panels;
        private Engine _engine;

        public GUIController(Engine engine)
        {
            this._engine = engine;
            this._panels = new List<UIPanel>();
        }

        /// <summary>
        /// Registers a new Panel to draw
        /// </summary>
        /// <param name="panel"></param>
        public void registerPanel(UIPanel panel)
        {
            //Maybe Panels should register here which Events they are interested in (Mouse, Keyboard)

            _panels.Add(panel);
        }

        public void render()
        {
            switchToOrthoRendering();

            foreach (UIPanel panel in _panels)
            {
                panel.render();
            }

            switchBackToFrustrumRendering();
        }

        private void switchToOrthoRendering()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, _engine.Width, 0, _engine.Height, -5, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void switchBackToFrustrumRendering()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

    }
}
