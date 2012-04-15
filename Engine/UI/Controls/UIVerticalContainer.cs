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

        public int Height { get; set; }
        public int Width { get; set; }

        public int StartX { get; set; }
        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }
        public UIVerticalPosition VerticalPosition { get; set; }

        public IUIControl Parent { get; set; }

        public UIVerticalContainer()
        {
            _controls = new List<IUIControl>();
        }

        public void add(IUIControl control)
        {
            control.Parent = this;
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

        public void resize()
        {
            if (_controls.Count != 0)
            {
                int childHeight;
                int childWidth;

                int childStartX;

                foreach (IUIControl control in _controls)
                {

                }
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
