using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public interface IUIControl : IUIPlacable
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

        /// <summary>
        /// Called when Mouse Enters UIControl area
        /// </summary>
        void onHoverEnter();

        /// <summary>
        /// Called when Mouse Leaves UIControl area
        /// </summary>
        void onHoverExit();
    }
}
