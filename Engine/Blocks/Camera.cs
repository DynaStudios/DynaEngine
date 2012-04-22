using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    public class Camera
    {
        private IWorldObject movedBy;
        public IWorldObject WorldObject { get; set; }

        public Camera() { }

        public void move()
        {
            if (WorldObject != null)
            {
                movedBy = WorldObject;
                // HACK: GL.LoadIdentity should be called here

                Direction direction = WorldObject.Direction;
                GL.Rotate(direction.X, 1.0f, 0.0f, 0.0f);
                GL.Rotate(direction.Rotation, 0.0f, 0.0f, 1.0f);
                GL.Rotate(direction.Y, 0.0f, 1.0f, 0.0f);

                WorldPosition position = WorldObject.Position;
                GL.Translate(position.x, position.y, position.z);
            }
        }

        public void moveBack()
        {
            if (movedBy != null)
            {

                WorldPosition position = movedBy.Position;
                GL.Translate(-position.x, -position.y, -position.z);

                Direction direction = movedBy.Direction;
                GL.Rotate(-direction.Y, 0.0f, 1.0f, 0.0f);
                GL.Rotate(-direction.Rotation, 0.0f, 0.0f, 1.0f);
                GL.Rotate(-direction.X, 1.0f, 0.0f, 0.0f);
            }
        }
    }
}
