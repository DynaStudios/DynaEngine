using System.Collections.Generic;
using DynaStudios.UI.Controls;
using DynaStudios.IO;

namespace DynaStudios.UI
{
    public class GUIController
    {

        private List<UIPanel> _panels;
        private InputDevice _inputDevice;

        public GUIController(InputDevice inputDevice)
        {
            this._inputDevice = inputDevice;
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
            foreach (UIPanel panel in _panels)
            {
                panel.render();
            }
        }

    }
}
