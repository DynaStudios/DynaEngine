using System.Collections.Generic;
using DynaStudios.UI.Controls;
using DynaStudios.IO;
using OpenTK.Graphics.OpenGL;
using System;
using OpenTK.Input;
using DynaStudios.UI.Utils;

namespace DynaStudios.UI
{
    public class GUIController
    {

        private List<UIPanel> _panels;
        private Engine _engine;

        public bool IsVisible { get; set; }

        public GUIController(Engine engine)
        {
            this._engine = engine;
            this._panels = new List<UIPanel>();

            //GUI should be visible by default
            IsVisible = true;

            //Register Keyboard and Mouse Events
            _engine.InputDevice.Keyboard.KeyUp += Keyboard_KeyUp;
            _engine.InputDevice.Mouse.ButtonUp += Mouse_ButtonUp;
            _engine.InputDevice.Mouse.Move += Mouse_Move;
        }

        /// <summary>
        /// Handles User Keyboard KeyUps
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">Pressed Key</param>
        private void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            _engine.Logger.Debug("Received KeyUP Event. Key is: " + e.Key);

            #region Handle Internal GUI Controller KeyBindings
            //Check if F7 were pressed to Enable / Disable GUI Rendering
            if (e.Key == Key.F7)
            {
                if (IsVisible)
                {
                    _engine.Logger.Debug("Disable GUI Rendering");
                    IsVisible = false;
                }
                else
                {
                    _engine.Logger.Debug("Enable GUI Rendering");
                    IsVisible = true;
                }
            }
            #endregion

            if (IsVisible)
            {
                //TODO: Check if any UIControls registered for Keyboard Input. (Example: Chat Input Bar)
            }
        }

        /// <summary>
        /// Handles User MouseButton Clicks
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">Pressed MouseButton</param>
        private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsVisible)
            {
                //TODO: Check if any UIControls were clicked (Example: Button)
            }
        }

        /// <summary>
        /// Handles Users Mouse Movement
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">Mouse Position</param>
        private void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (IsVisible)
            {
                //TODO: Handle Mouse Movement for Hover Effects etc.
            }
        }

        /// <summary>
        /// Registers a new Panel to draw
        /// </summary>
        /// <param name="panel"></param>
        public void registerPanel(UIPanel panel)
        {
            //Maybe Panels should register here which Events they are interested in (Mouse, Keyboard)

            //Calculate Positions for added Panel
            calculatePosition(panel);

            _panels.Add(panel);
        }

        private void calculatePosition(UIPanel panel)
        {

            if (panel.Width == 0)
            {
                panel.Width = _engine.Width;
            }

            if (panel.Height == 0)
            {
                panel.Height = _engine.Height;
            }

            panel.StartX = PositionHelper.calculateStartX(panel, _engine.Width);
            panel.StartY = PositionHelper.calculateStartY(panel, _engine.Height);

            //At the end call the panels own resize method to calculate positions for his Children
            panel.resize();

        }

        public void resizeGui()
        {
            foreach (UIPanel panel in _panels)
            {
                calculatePosition(panel);
            }
        }

        public void render()
        {
            if (IsVisible)
            {
                //Disable Depth Rendering to draw 2D UIs
                switchToOrthoRendering();

                foreach (UIPanel panel in _panels)
                {
                    //Render Panel
                    panel.render();
                }

                //Enable Depth Rendering again
                switchBackToFrustrumRendering();
            }
        }

        /// <summary>
        /// Disable Depth Rendering to draw 2D UIs
        /// </summary>
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

        /// <summary>
        /// Enable Depth Rendering again
        /// </summary>
        private void switchBackToFrustrumRendering()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

    }
}
