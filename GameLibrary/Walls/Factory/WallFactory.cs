using EngineLibrary.ObjectComponents;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Walls.Factory
{
    public abstract class WallFactory
    {
        /// <summary>
        /// Создание стены
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public abstract GameObject CreateWall(Vector2 position, string tagName);
    }
}
