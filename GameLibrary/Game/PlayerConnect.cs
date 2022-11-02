using EngineLibrary.ObjectComponents;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Game
{
    public class PlayerConnect : ObjectScript
    {
        public override void Start(GameObject gameObject = null)
        {
        }

        public override void Update(GameObject gameObject)
        {
        }

        /// <summary>
        /// Изменение позиции подключенного игрока
        /// </summary>
        /// <param name="position"></param>
        public void ChangePosition(GameObject gameObject, Vector2 position)
        {
            gameObject.Transform.SetMovement(position);
        }
    }
}
