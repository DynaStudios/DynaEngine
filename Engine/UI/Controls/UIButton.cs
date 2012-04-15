using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public delegate void ClickedEventHandler();

    public class UIButton : IUIControl
    {

        public event ClickedEventHandler OnClicked;

        public int Height { get; set; }
        public int Width { get; set; }

        public int StartX { get; set; }
        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }
        public UIVerticalPosition VerticalPosition { get; set; }

        public String Text { get; set; }

        public IUIControl Parent { get; set; }

        public UIButton()
        {
        }

        public void render()
        {
            
        }

        public void resize()
        {
            
        }

        public void onClicked(MouseButtonEventArgs arg)
        {
            if (OnClicked != null)
                OnClicked();
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
