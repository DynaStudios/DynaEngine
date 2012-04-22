using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.UI.Controls
{
    public interface IUIPlacable
    {
        int Height { get; set; }

        int Width { get; set; }

        int StartX { get; set; }

        int StartY { get; set; }

        UIHorizontalPosition HorizontalPosition { get; set; }

        UIVerticalPosition VerticalPosition { get; set; }

        IUiControl Parent { get; set; }
    }
}