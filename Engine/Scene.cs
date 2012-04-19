using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios
{
    public abstract class Scene
    {
        public Engine Engine { get; set; }
        
        public Scene(Engine engine)
        {
            this.Engine = engine;
        }

        public virtual void loadScene()
        {

        }

        public virtual void doRender()
        {

        }

        public virtual void unloadScene()
        {

        }

    }
}
