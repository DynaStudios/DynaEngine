using System.Collections.Generic;
using DynaStudios.UI.Controls;
using DynaStudios.IO;
using OpenTK.Graphics.OpenGL;
using System;
using OpenTK.Input;
using DynaStudios.UI.Utils;
using OpenTK;
using QuickFont;
using OpenTK.Graphics;

namespace DynaStudios.UI
{
    public class GUIController
    {

        private List<UIPanel> _panels;
        private Engine _engine;

        private QFont _mainFont;

        public bool IsVisible { get; set; }

        public GUIController(Engine engine)
        {
            this._engine = engine;
            this._panels = new List<UIPanel>();

            //GUI should be visible by default
            IsVisible = true;

            //Init MainFont
            QFontBuilderConfiguration config = new QFontBuilderConfiguration(false);
            
            _mainFont = new QFont("Fonts/visitor2.ttf", 24, config);
            _mainFont.Options.UseDefaultBlendFunction = false;
            _mainFont.Options.Colour = Color4.Green;

            //Register Keyboard and Mouse Events
            _engine.InputDevice.Keyboard.KeyUp += keyboard_KeyUp;
            _engine.InputDevice.Mouse.ButtonUp += mouse_ButtonUp;
            _engine.InputDevice.Mouse.Move += mouse_Move;
        }

        /// <summary>
        /// Handles User Keyboard KeyUps
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">Pressed Key</param>
        private void keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
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
            if (e.Key == Key.F8)
            {
                if (_engine.VSync == VSyncMode.On)
                {
                    _engine.Logger.Debug("Disable VSync");
                    _engine.VSync = VSyncMode.Off;
                }
                else
                {
                    _engine.Logger.Debug("Enable VSync");
                    _engine.VSync = VSyncMode.On;
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
        private void mouse_ButtonUp(object sender, MouseButtonEventArgs e)
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
        private void mouse_Move(object sender, MouseMoveEventArgs e)
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

                //Render FPS
                QFont.Begin();
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                _mainFont.Print(_engine.FpsCalc.Framerate.ToString());
                QFont.End();
                

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
            GL.Enable(EnableCap.Blend);
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
            GL.Disable(EnableCap.Blend);
        }

    }
}
