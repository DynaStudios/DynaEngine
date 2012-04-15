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

        public String Text { get; set; }
        public UIPosition Position { get; set; }

        public void render()
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
