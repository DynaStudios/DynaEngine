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

        public void remove(IUIControl control)
        {
            _controls.Remove(control);
        }

        public void render()
        {
            foreach (IUIControl control in _controls)
            {
                control.render();
            }
        }

        public void onClicked(MouseButtonEventArgs arg)
        {
            //Spread some Click love to the right UIControl :)
        }

        public void onKeyPressed(KeyboardKeyEventArgs arg)
        {
            //OMG a Key got pressed! But not yet handled :P
        }

        public void onHoverEnter()
        {
            //Hey! You there! You got hovered. bwahahaha
        }

        public void onHoverExit()
        {
            //kk... Bye!
        }
    }
}
