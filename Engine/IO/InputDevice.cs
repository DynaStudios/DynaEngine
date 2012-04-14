using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.IO
{
    public class InputDevice
    {

        public MouseDevice Mouse { get; set; }
        public KeyboardDevice Keyboard { get; set; }

        public InputDevice(MouseDevice mouse, KeyboardDevice keyboard)
        {
            Mouse = mouse;
            Keyboard = keyboard;
        }

    }
}
