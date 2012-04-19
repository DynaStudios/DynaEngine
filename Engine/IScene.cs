using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios
{
    public interface IScene
    {
        Engine Engine { get; }
        void loadScene();
        void doRender();
        void unloadScene();
    }
}
