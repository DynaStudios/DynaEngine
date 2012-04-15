using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;
using DynaStudios.UI.Utils;

namespace DynaStudios.UI.Controls
{
    public class UIPanel : IUIPlacable, IUIControl
    {
        private IUIControl _uiControl;

        public int Height { get; set; }
        public int Width { get; set; }

        public int StartX { get; set; }
        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }
        public UIVerticalPosition VerticalPosition { get; set; }

        public IUIControl Parent { get; set; }

        /// <summary>
        /// Creates new UIPanel. UIPanel won't be rendered until you set an UIControl with setContainingPanel(IUIControl);
        /// </summary>
        public UIPanel() { }

        /// <summary>
        /// Creates new UIPanel and sets IUIControl given by constructor
        /// </summary>
        /// <param name="control"></param>
        public UIPanel(IUIControl control)
        {
            setContainingPanel(control);
        }

        /// <summary>
        /// Sets or replaces UIControl in the Panel. For example: VerticalContainer or an Image
        /// </summary>
        /// <param name="control">Control binded to the UIPanel</param>
        public void setContainingPanel(IUIControl control)
        {
            this._uiControl = control;
            this._uiControl.Parent = this;
        }

        /// <summary>
        /// Renders all UIControls inside UIPanel
        /// </summary>
        public void render()
        {
            if (_uiControl != null)
            {
                _uiControl.render();
            }
        }

        public void resize()
        {

            if (Parent != null)
            {
                StartX = PositionHelper.calculateStartX(this, Parent);
                StartY = PositionHelper.calculateStartY(this, Parent);
            }

            _uiControl.resize();

        }

        public void onClicked(MouseButtonEventArgs arg)
        {

        }

        public void onKeyPressed(KeyboardKeyEventArgs arg)
        {

        }

        public void onHoverEnter()
        {

        }

        public void onHoverExit()
        {

        }
    }
}
