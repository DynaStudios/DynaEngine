using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.UI.Controls
{
    public class UIPanel
    {
        private List<IUIControl> _uiControls;

        public UIPanel()
        {
            this._uiControls = new List<IUIControl>();
        }

        /// <summary>
        /// Renders all UIControls inside UIPanel
        /// </summary>
        public void render()
        {
            foreach (IUIControl control in _uiControls)
            {
                control.render();
            }
        }

    }
}
