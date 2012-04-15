using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public class UIVerticalContainer : IUIControl
    {

        private List<IUIControl> _controls;

        public UIVerticalContainer()
        {
            _controls = new List<IUIControl>();
        }

        public void add(IUIControl control)
        {
            _controls.Add(control);
        }

        public void render()
        {
            
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
