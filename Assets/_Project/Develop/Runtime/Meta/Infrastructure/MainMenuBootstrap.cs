using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private ICoroutinesPerformer _coroutinesPerformer;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Инициализация сцены меню");

            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            yield break;
        }

        public override IEnumerator Run()
        {
            yield break;
        }

        private void Update()
        {

        }
    }
}