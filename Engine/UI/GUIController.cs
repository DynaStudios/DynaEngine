using System.Collections.Generic;
using System.Globalization;
using DynaStudios.UI.Controls;
using DynaStudios.UI.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using QuickFont;

namespace DynaStudios.UI
{
    public class GuiController
    {
        private readonly Engine _engine;
        private readonly QFont _mainFont;
        private readonly List<UIPanel> _panels;

        public GuiController(Engine engine)
        {
            _engine = engine;
            _panels = new List<UIPanel>();

            //GUI should be visible by default
            IsVisible = true;

            //Init MainFont
            QFontBuilderConfiguration config = new QFontBuilderConfiguration(false);

            _mainFont = new QFont("Fonts/visitor2.ttf", 24, config) { Options = { UseDefaultBlendFunction = false, Colour = Color4.Green } };
            //Register Keyboard and Mouse Events
            _engine.InputDevice.Keyboard.KeyUp += KeyboardKeyUp;
            _engine.InputDevice.Mouse.ButtonUp += MouseButtonUp;
            _engine.InputDevice.Mouse.Move += MouseMove;
        }

        public bool IsVisible { get; set; }

        /// <summary>
        /// Registers a new Panel to draw
        /// </summary>
        /// <param name="panel"></param>
        public void RegisterPanel(UIPanel panel)
        {
            //Maybe Panels should register here which Events they are interested in (Mouse, Keyboard)

            //Calculate Positions for added Panel
            CalculatePosition(panel);

            _panels.Add(panel);
        }

        public void Render()
        {
            if (IsVisible)
            {
                //Disable Depth Rendering to draw 2D UIs
                SwitchToOrthoRendering();

                foreach (UIPanel panel in _panels)
                {
                    //Render Panel
                    panel.Render();
                }

                //Render FPS
                QFont.Begin();
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
                _mainFont.Print(_engine.FpsCalc.Framerate.ToString(CultureInfo.InvariantCulture));
                QFont.End();

                //Enable Depth Rendering again
                switchBackToFrustrumRendering();
            }
        }

        public void ResizeGui()
        {
            foreach (UIPanel panel in _panels)
            {
                CalculatePosition(panel);
            }
        }

        private void CalculatePosition(UIPanel panel)
        {
            if (panel.Width == 0)
            {
                panel.Width = _engine.Width;
            }

            if (panel.Height == 0)
            {
                panel.Height = _engine.Height;
            }

            panel.StartX = PositionHelper.CalculateStartX(panel, _engine.Width);
            panel.StartY = PositionHelper.CalculateStartY(panel, _engine.Height);

            //At the end call the panels own resize method to calculate positions for his Children
            panel.Resize();
        }

        /// <summary>
        /// Handles User Keyboard KeyUps
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">Pressed Key</param>
        private void KeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
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

            #endregion Handle Internal GUI Controller KeyBindings

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
        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
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
        private void MouseMove(object sender, MouseMoveEventArgs e)
        {
            if (IsVisible)
            {
                //TODO: Handle Mouse Movement for Hover Effects etc.
            }
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

        /// <summary>
        /// Disable Depth Rendering to draw 2D UIs
        /// </summary>
        private void SwitchToOrthoRendering()
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
    }
}