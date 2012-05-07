using System.IO;

using OpenTK.Input;

using DynaStudios;
using DynaStudios.Blocks;
using DynaStudios.IO;

using DynaStudios.Chunks;

namespace TestGame.Scenes
{
    public class StupedWorldScene : IScene
    {
        public Engine Engine { get; set; }
        private Region _region;

        private CameraMan camerMan;

        public StupedWorldScene(Engine engine)
        {
            Engine = engine;
            Engine.Logger.Debug("Loaded StupedWorldScene");
        }

        public void loadScene()
        {
            camerMan = new CameraMan(Engine.InputDevice);
            camerMan.Position.z = -3.0;
            Engine.Camera.WorldObject = camerMan;
            _region = new Region("maps", 0, 0);
            _region.generateStupedWorld();
        }

        public void doRender()
        {
            _region.render(camerMan.Position);
        }

        public void unloadScene() {}
    }
}
