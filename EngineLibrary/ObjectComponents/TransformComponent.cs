using SharpDX;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Компонент перемещения игрового объекта
    /// </summary>
    public class TransformComponent
    {
        /// <summary>
        /// Позиция игрового объекта
        /// </summary>
        public Vector2 Position { get => position; set => position = value; }
        private Vector2 position;

        /// <summary>
        /// Размер игрового объекта
        /// </summary>
        public Size2F Scale { get => scale; set => scale = value; }
        private Size2F scale;

        private Vector2 movementInCurrentFrame;

        /// <summary>
        /// Конструктор компонента
        /// </summary>
        /// <param name="position">начальная позиция</param>
        /// <param name="scale">Начальный размер</param>
        public TransformComponent(Vector2 position, Size2F scale)
        {
            movementInCurrentFrame = Vector2.Zero;
            this.position = position;
            this.scale = scale;
        }

        /// <summary>
        /// Перемещение объкта
        /// </summary>
        /// <param name="movement">Вектор перемещения</param>
        public void SetMovement(Vector2 movement)
        {            
            movementInCurrentFrame = movement;

            position.X += movement.X;
            position.Y -= movement.Y;
        }

        /// <summary>
        /// Возврат позиция в этом кадре
        /// </summary>
        public void ResetMovement()
        {
            position.X -= movementInCurrentFrame.X;
            
            position.Y += movementInCurrentFrame.Y;
        }
    }
}
