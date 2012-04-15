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
        }

        public void render()
        {
            foreach (UIPanel panel in _panels)
            {
                //Render each User Interface Panel
            }
        }

    }
}
