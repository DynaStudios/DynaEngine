using System.IO;

using OpenTK.Input;

using DynaStudios;
using DynaStudios.Blocks;
using DynaStudios.IO;


namespace TestGame.Scenes
{
    class CameraMan : IWorldObject
    {
        public Direction Direction { get; set; }
        public WorldPosition Position { get; set; }
        private InputDevice input;

        public CameraMan(InputDevice input)
        {
            this.input = input;
            Direction = new Direction();
            Position = new WorldPosition();
            input.Keyboard.KeyDown += Keyboard_KeyDown;
        }

        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            // TODO: movement must be relative to the direction
            switch (e.Key)
            {
                case (Key.A):
                    Position.x += 0.5;
                    break;
                case (Key.D):
                    Position.x -= 0.5;
                    break;
                case (Key.W):
                    Position.z += 0.5;
                    break;
                case (Key.S):
                    Position.z -= 0.5;
                    break;
                case (Key.Left):
                    Direction.Y -= 9.0;
                    break;
                case (Key.Right):
                    Direction.Y += 9.0;
                    break;
                case (Key.Up):
                    Direction.X -= 9.0;
                    break;
                case (Key.Down):
                    Direction.X += 9.0;
                    break;
                case (Key.Q):
                    Direction.Rotation -= 9.0;
                    break;
                case (Key.E):
                    Direction.Rotation += 9.0;
                    break;
            }
        }
    }

    public class BlockTestScene : IScene
    {
        public Engine Engine { get; set; }
        private Chunklet chunklet1;
        private Room _room;

        private CameraMan camerMan;

        public BlockTestScene(Engine engine)
        {
            Engine = engine;
            Engine.Logger.Debug("Loaded BlockTestScene");
        }

        public void loadScene()
        {

            camerMan = new CameraMan(Engine.InputDevice);
            camerMan.Position.z = -3.0;
            Engine.Camera.WorldObject = camerMan;

            chunklet1 = new Chunklet(0, 0, 0);

            _room = new Room(50,50,50);

            _room.loadXml(Path.Combine(DynaStudios.Utils.StreamTool.DIR, "Maps", "map.xml"), Engine.TextureManager);
        }

        public void doRender()
        {
            //chunklet1.render();
            _room.render();
        }

        public void unloadScene() {}
    }
}
