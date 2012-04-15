using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.UI.Controls
{
    public class UIPanel
    {
        private IUIControl _uiControl;

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

    }
}
