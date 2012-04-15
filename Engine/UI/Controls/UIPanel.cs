using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.UI.Controls
{
    public class UIPanel
    {
        private IUIControl _uiControl;

        public UIPanel() { }

        public UIPanel(IUIControl control)
        {
            setContainingPanel(control);
        }

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
