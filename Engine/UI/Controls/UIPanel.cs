using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynaStudios.UI.Utils;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public class UIPanel : IUIPlacable, IUiControl
    {
        private IUiControl _uiControl;

        public int Height { get; set; }

        public int Width { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }

        public UIVerticalPosition VerticalPosition { get; set; }

        public IUiControl Parent { get; set; }

        /// <summary>
        /// Creates new UIPanel. UIPanel won't be rendered until you set an UIControl with setContainingPanel(IUIControl);
        /// </summary>
        public UIPanel() { }

        /// <summary>
        /// Creates new UIPanel and sets IUIControl given by constructor
        /// </summary>
        /// <param name="control"></param>
        public UIPanel(IUiControl control)
        {
            setContainingPanel(control);
        }

        /// <summary>
        /// Sets or replaces UIControl in the Panel. For example: VerticalContainer or an Image
        /// </summary>
        /// <param name="control">Control binded to the UIPanel</param>
        public void setContainingPanel(IUiControl control)
        {
            this._uiControl = control;
            this._uiControl.Parent = this;
        }

        /// <summary>
        /// Renders all UIControls inside UIPanel
        /// </summary>
        public void Render()
        {
            if (_uiControl != null)
            {
                _uiControl.Render();
            }
        }

        public void Resize()
        {
            if (Parent != null)
            {
                StartX = PositionHelper.CalculateStartX(this, Parent);
                StartY = PositionHelper.CalculateStartY(this, Parent);
            }

            _uiControl.Resize();
        }

        public void OnClicked(MouseButtonEventArgs arg)
        {
        }

        public void OnKeyPressed(KeyboardKeyEventArgs arg)
        {
        }

        public void OnHoverEnter()
        {
        }

        public void OnHoverExit()
        {
        }
    }
}