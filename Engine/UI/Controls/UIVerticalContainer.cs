using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public class UIVerticalContainer : IUiControl
    {
        private List<IUiControl> _controls;

        public int Height { get; set; }

        public int Width { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }

        public UIVerticalPosition VerticalPosition { get; set; }

        public IUiControl Parent { get; set; }

        public UIVerticalContainer()
        {
            _controls = new List<IUiControl>();
        }

        public void add(IUiControl control)
        {
            control.Parent = this;
            _controls.Add(control);
        }

        public void remove(IUiControl control)
        {
            _controls.Remove(control);
        }

        public void Render()
        {
            foreach (IUiControl control in _controls)
            {
                control.Render();
            }
        }

        public void Resize()
        {
            if (_controls.Count != 0)
            {
                int childHeight;
                int childWidth;

                int childStartX;

                foreach (IUiControl control in _controls)
                {
                }
            }
        }

        public void OnClicked(MouseButtonEventArgs arg)
        {
            //Spread some Click love to the right UIControl :)
        }

        public void OnKeyPressed(KeyboardKeyEventArgs arg)
        {
            //OMG a Key got pressed! But not yet handled :P
        }

        public void OnHoverEnter()
        {
            //Hey! You there! You got hovered. bwahahaha
        }

        public void OnHoverExit()
        {
            //kk... Bye!
        }
    }
}