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
            //GL.Rotate(-Direction.y, 0.0f, 1.0f, 0.0f);
            //GL.Rotate(-Direction.x, 1.0f, 0.0f, 0.0f);
            //GL.Rotate(-Direction.rotation, 0.0f, 0.0f, 1.0f);
            render();
            //GL.Rotate(Direction.rotation, 0.0f, 0.0f, 1.0f);
            //GL.Rotate(Direction.x, 1.0f, 0.0f, 0.0f);
            //GL.Rotate(Direction.y, 0.0f, 1.0f, 0.0f);
            GL.Translate(-x, -y, -z);
        }
        public abstract void render();
    }
}
