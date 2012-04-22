using DynaStudios.UI.Controls;

namespace DynaStudios.UI.Utils
{
    public class PositionHelper
    {
        public static int CalculateStartX(IUiControl control, IUiControl parentControl)
        {
            return 0;
        }

        public static int CalculateStartY(IUiControl control, IUiControl parentControl)
        {
            return 0;
        }

        public static int CalculateStartX(IUiControl control, int parentWidth)
        {
            int startX = 0;

            switch (control.HorizontalPosition)
            {
                case UIHorizontalPosition.Left:
                    startX = 0;
                    break;
                case UIHorizontalPosition.Center:
                    startX = (parentWidth / 2) - (control.Width / 2);
                    break;
                case UIHorizontalPosition.Right:
                    startX = parentWidth - control.Width;
                    break;
            }
            return startX;
        }

        public static int CalculateStartY(IUiControl control, int parentHeight)
        {
            int startY = 0;

            switch (control.VerticalPosition)
            {
                case UIVerticalPosition.Top:
                    startY = 0;
                    break;
                case UIVerticalPosition.Middle:
                    startY = (parentHeight / 2) - (control.Height / 2);
                    break;
                case UIVerticalPosition.Bottom:
                    startY = parentHeight - control.Height;
                    break;
            }

            return startY;
        }
    }
}