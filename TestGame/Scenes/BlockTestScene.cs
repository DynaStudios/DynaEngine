using System.Collections.Generic;
using DynaStudios;
using DynaStudios.Blocks;
using DynaStudios.IO;
using OpenTK.Input;


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

    public class BlockTestScene : Scene
    {

        private Chunklet chunklet1;

        private List<AbstractDrawable> worldObjects = new List<AbstractDrawable>();
        private CameraMan camerMan;

        private Camera camera = new Camera();
        public Camera Camera
        {
            get { return camera; }
        }

        public BlockTestScene(Engine engine) : base(engine)
        {
            Engine.Logger.Debug("Loaded BlockTestScene");
        }

        public override void loadScene()
        {

            camerMan = new CameraMan(Engine.InputDevice);
            camerMan.Position.z = -3.0;
            Camera.WorldObject = camerMan;

            chunklet1 = new Chunklet(0, 0, 0);
            
        }

        public override void doRender()
        {
            //moves the camera
            camera.move();

            chunklet1.render(camera);

            // unmoves the camera for the next frame
            camera.moveBack();
        }

    }
}
