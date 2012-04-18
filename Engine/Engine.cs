using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Logger = DynaStudios.DynaLogger.Logger;
using DynaStudios.DynaLogger.Appender;
using DynaStudios.Blocks;
using DynaStudios.IO;
using DynaStudios.UI;

namespace DynaStudios
{
    class CameraMan : IWorldObject {
        public Direction Direction { get; set; }
        public WorldPosition Position { get; set; }
        private InputDevice input;

        public CameraMan (InputDevice input) {
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

    public class Engine : GameWindow
    {
        public Logger Logger;

        private Camera camera = new Camera();
        public Camera Camera {
            get { return camera; }
        }

        private List<AbstractDrawable> worldObjects = new List<AbstractDrawable>();
        private CameraMan camerMan;

        /// <summary>
        /// You can add your GUI Elements to the UIController and also let them register to Mouse and Keyboard Events.
        /// 
        /// Add new UIPanels using UIController.register(UIPanel yourPanel);
        /// </summary>
        public GUIController UiController { get; set; }
        public InputDevice InputDevice { get; set; }

        private Chunklet chunklet1;

        public Engine()
            : base(1024, 768, new GraphicsMode(32, 1, 0, 4))
        {
            Logger = new Logger();
            Logger.Register(new ConsoleLogger());
            Logger.Register(new FileSystemLogger());
            Logger.Debug("Init Game.");
            Block block1 = new Block(0, 0, 0);
            Block block2 = new Block(0, 0, 2);
            Block block3 = new Block(2, 0, 0);
            Block block4 = new Block(0, 2, 0);
            block1.color = Color.AliceBlue;
            block2.color = Color.White;
            block3.color = Color.Red;
            block4.color = Color.Brown;
            worldObjects.Add(block3);
            worldObjects.Add(block1);
            worldObjects.Add(block4);
            worldObjects.Add(block2);

            //chunklet1 = new Chunklet();


        }

        protected override void OnLoad(EventArgs e)
        {
            Logger.Debug("Called OnLoad();");
            base.Title = "DynaEngine Sample Game";

            //Init User Interface Controller
            Logger.Debug("Register GUI Controller");
            InputDevice = new InputDevice(Mouse, Keyboard);
            UiController = new GUIController(this);

            camerMan = new CameraMan(InputDevice);
            camerMan.Position.z = -3.0;
            Camera.WorldObject = camerMan;

            resize(null, EventArgs.Empty);

            //Enable OpenGL Modes
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(Color.Gray);


        }

        protected override void OnResize(EventArgs e)
        {
            Logger.Debug("OnResize called. New Width: " + base.Width + "px | New Height: " + base.Height + "px");
            resize(null, EventArgs.Empty);
            UiController.resizeGui();
        }

        protected override void OnClosed(EventArgs e)
        {
            //Unload Libs etc. here
            Logger.Debug("Closing Application");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lookat);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //moves the camera
            camera.move();

            //Render World Objects
            foreach (AbstractDrawable worldObject in worldObjects)
            {
                worldObject.doRender();
            }

            //chunklet1.render(camerMan);

            // unmoves the camera for the next frame
            camera.moveBack();

            //Start GUI/HUD Rendering here
            UiController.render();

            this.SwapBuffers();
        }
        
        /// <summary>
        /// Recalculates Viewport
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, base.Width, base.Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }
    }
}
