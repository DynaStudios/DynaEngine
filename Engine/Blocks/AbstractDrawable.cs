using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    public abstract class AbstractDrawable : IWorldObject
    {
        public virtual Direction Direction { get; set; }
        public virtual WorldPosition Position { get; set; }

        public AbstractDrawable()
        {
            Direction = new Direction();
            Position = new WorldPosition();
        }

        public void doRender()
        {
            var x = Position.x;
            var y = Position.y;
            var z = Position.z;

            GL.Translate(x, y, z);
            GL.Rotate(Direction.Y, 0.0f, 1.0f, 0.0f);
            GL.Rotate(Direction.Rotation, 0.0f, 0.0f, 1.0f);
            GL.Rotate(Direction.X, 1.0f, 0.0f, 0.0f);
            render();
            GL.Rotate(-Direction.X, 1.0f, 0.0f, 0.0f);
            GL.Rotate(-Direction.Rotation, 0.0f, 0.0f, 1.0f);
            GL.Rotate(-Direction.Y, 0.0f, 1.0f, 0.0f);
            GL.Translate(-x, -y, -z);
        }
        public abstract void render();
    }
}
