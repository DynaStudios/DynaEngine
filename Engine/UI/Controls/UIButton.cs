using System;
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public delegate void ClickedEventHandler();

    public class UiButton : IUiControl
    {
        public String Text { get; set; }

        #region IUIControl Members

        public int Height { get; set; }

        public int Width { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public UIHorizontalPosition HorizontalPosition { get; set; }

        public UIVerticalPosition VerticalPosition { get; set; }

        public IUiControl Parent { get; set; }

        public void Render()
        {
        }

        public void Resize()
        {
        }

        public void OnClicked(MouseButtonEventArgs arg)
        {
        }

        public void OnKeyPressed(KeyboardKeyEventArgs arg)
        {
        }

        public void OnHoverEnter()
        {
        }

        public void OnHoverExit()
        {
        }

        #endregion IUIControl Members

        public event ClickedEventHandler Clicked;

        public void OnClicked()
        {
            ClickedEventHandler handler = Clicked;
            if (handler != null) handler();
        }
    }
}