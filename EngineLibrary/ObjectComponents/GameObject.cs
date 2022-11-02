using SharpDX;
using SharpDX.Direct2D1;

namespace EngineLibrary.ObjectComponents
{
    /// <summary>
    /// Игровой объект
    /// </summary>
    public class GameObject
    {
        private WindowRenderTarget renderTarget;
        private float worldScale;

        /// <summary>
        /// Родительский игровой объект
        /// </summary>
        public GameObject ParentGameObject { get; set; }

        /// <summary>
        /// Тэг игрового объекта
        /// </summary>
        public string GameObjectTag { get; set; }

        /// <summary>
        /// Активность игрового объекта
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Компонент перемещения
        /// </summary>
        public TransformComponent Transform { get; private set; }

        /// <summary>
        /// Компонент спрайта
        /// </summary>
        public SpriteComponent Sprite { get; set; }

        /// <summary>
        /// Компонент твердого тела
        /// </summary>
        public ColliderComponent Collider { get; private set; }

        /// <summary>
        /// Сценарий выполения
        /// </summary>
        public ObjectScript Script { get; private set; }

        /// <summary>
        /// Инциализация опций рендеринга игрового объекта
        /// </summary>
        /// <param name="target">Окно отрисовки</param>
        /// <param name="scale">Относительный размер</param>
        internal void InitalizeEngineSettings(WindowRenderTarget target, float scale)
        {
            renderTarget = target;
            worldScale = scale;
        }

        /// <summary>
        /// Инциализация компонетов игрового объекта
        /// </summary>
        /// <param name="component">Компонент игровго объекта</param>
        public void InitializeObjectComponent(object component)
        {
            switch(component)
            {
                case TransformComponent transformComponent:
                    Transform = transformComponent;
                    break;
                case SpriteComponent spriteComponent:
                    Sprite = spriteComponent;
                    break;
                case ColliderComponent colliderComponent:
                    Collider = colliderComponent;
                    break;
            }
        }

        /// <summary>
        /// Инициализация сценария поведения объекта
        /// </summary>
        /// <param name="objectScript">Сценарий игрового объекта</param>
        public void InitializeObjectScript(ObjectScript objectScript) 
        {
            Script = objectScript;
            Script.Start(this);
        }

        /// <summary>
        /// Метод отрисовки игрового объекта 
        /// </summary>
        public void Draw()
        {
            if (Collider != null)
                Collider.IsInactive = !IsActive;

            if (Script != null)
                Script.Update(this);

            if (!IsActive || (ParentGameObject != null && !ParentGameObject.IsActive)) return;

            if (Sprite == null) return;

            Vector2 translation = new Vector2(0,0);

            if (ParentGameObject != null)
            {
                translation.X = ParentGameObject.Transform.Position.X * ParentGameObject.Transform.Scale.Width;
                translation.Y = ParentGameObject.Transform.Position.Y * ParentGameObject.Transform.Scale.Height;
            }

            translation.X += Transform.Position.X * Transform.Scale.Width;
            translation.Y += Transform.Position.Y * Transform.Scale.Height;

            Matrix3x2 matrix = Matrix3x2.Rotation(0, translation);
            matrix *= Matrix3x2.Scaling(Transform.Scale.Width * worldScale / Sprite.WidthOfSprite, Transform.Scale.Height * worldScale / Sprite.HeightOfSprite, translation);
            matrix *= Matrix3x2.Translation(translation * worldScale);
            renderTarget.Transform = matrix;

            Sprite.PlayAnimation();
            if (Sprite.Bitmap != null)
                renderTarget.DrawBitmap(Sprite.Bitmap, 1f, BitmapInterpolationMode.Linear);
        }
    }
}
