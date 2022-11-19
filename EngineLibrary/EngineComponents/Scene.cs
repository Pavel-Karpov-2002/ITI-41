using System.Collections.Generic;
using EngineLibrary.ObjectComponents;
using SharpDX.Direct2D1;

namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Абстрактный класс сцены
    /// </summary>
    public abstract class Scene
    {
        private WindowRenderTarget renderTarget;
        private float worldScale;

        /// <summary>
        /// Список текущих игровых объектов для отрисовки
        /// </summary>
        public List<GameObject> gameObjects = new List<GameObject>();

        /// <summary>
        /// Список игровых объектов для добалвения в список отрисовки
        /// </summary>
        public List<GameObject> gameObjectsToAdd = new List<GameObject>();

        /// <summary>
        /// Список игровых объектов для удаления их списка отрисовки
        /// </summary>
        public List<GameObject> gameObjectsToRemove = new List<GameObject>();
        
        /// <summary>
        /// Состояния отрисовки сцены
        /// </summary>
        public bool IsDrawScene = true;

        /// <summary>
        /// Инициализация сцены и игровых объектов
        /// </summary>
        /// <param name="target">Окно отрисовки</param>
        /// <param name="scale">Относительный размер объектов</param>
        public void InitializeScene(WindowRenderTarget target, float scale)
        {
            renderTarget = target;
            worldScale = scale;

            CreateGameObjectsOnScene();
            InitializeEngineSettingsToGameObjects(gameObjects);
        }

        /// <summary>
        /// Установка опций рендеринга игровым объектам
        /// </summary>
        private void InitializeEngineSettingsToGameObjects(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.InitalizeEngineSettings(renderTarget, worldScale);
        }

        /// <summary>
        /// Создание игровых объектов на сцене
        /// </summary>
        protected abstract void CreateGameObjectsOnScene();

        /// <summary>
        /// Отрисовка сцены (игровых объектов)
        /// </summary>
        public void DrawScene()
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw();

            AddRenderGameObjects();
            RemoveRenderGameObjects();

            if (!IsDrawScene)
                gameObjects.Clear();
        }

        /// <summary>
        /// Добавление игрового объекта для отрисовки
        /// </summary>
        private void AddRenderGameObjects()
        {
            InitializeEngineSettingsToGameObjects(gameObjectsToAdd);
            gameObjects.AddRange(gameObjectsToAdd);
            gameObjectsToAdd.Clear();
        }

        /// <summary>
        /// Удаление игрового объекта из отрисовки
        /// </summary>
        private void RemoveRenderGameObjects()
        {
            foreach(GameObject removeGameObject in gameObjectsToRemove)
            {
                gameObjects.Remove(removeGameObject);
            }

            gameObjectsToRemove.Clear();
        }
    }
}
