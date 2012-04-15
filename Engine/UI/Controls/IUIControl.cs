using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public interface IUIControl
    {
        /// <summary>
        /// Renders the UIControl
        /// </summary>
        void render();

        /// <summary>
        /// Called when the UIControl got clicked
        /// </summary>
        /// <param name="arg">Mouse Informations</param>
        void onClicked(MouseButtonEventArgs arg);

        /// <summary>
        /// Called when the UIControl received Keyboard Input
        /// </summary>
        /// <param name="arg"></param>
        void onKeyPressed(KeyboardKeyEventArgs arg);
    }
}
