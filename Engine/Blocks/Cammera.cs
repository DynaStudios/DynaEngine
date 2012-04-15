using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    public class Cammera
    {
        private IWorldObject movedBy;
        public IWorldObject WorldObject { get; set; }

        public Cammera() { }

        public void move()
        {
            if (WorldObject != null)
            {
                movedBy = WorldObject;
                WorldPosition position = WorldObject.Position;
                GL.Translate(position.x, position.y, position.z);

                Direction direction = WorldObject.Direction;
                GL.Rotate(direction.y, 0.0f, 1.0f, 0.0f);
                GL.Rotate(direction.x, 1.0f, 0.0f, 0.0f);
                GL.Rotate(direction.rotation, 0.0f, 0.0f, 1.0f);
            }
        }

        public void unmove()
        {
            if (movedBy != null)
            {
                WorldPosition position = movedBy.Position;
                GL.Translate(position.x, position.y, position.z);

                Direction direction = movedBy.Direction;
                GL.Rotate(direction.y, 0.0f, -1.0f, 0.0f);
                GL.Rotate(direction.x, -1.0f, 0.0f, 0.0f);
                GL.Rotate(direction.rotation, 0.0f, 0.0f, -1.0f);
            }
        }
    }
}
