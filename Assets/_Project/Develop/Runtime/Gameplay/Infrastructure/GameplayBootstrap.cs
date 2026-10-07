using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;
using System.Collections;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        [SerializeField] private CharacterGameplay _characterGameplay;

        private DIContainer _container;
        private GameplayInputArgs _inputArgs;

        private EntitiesLifeContext _entitiesLifeContext;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log($"Вы попали на уровень {_inputArgs.LevelNumber}");

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();

            _characterGameplay.Initialize(_container);

            yield break;
        }

        public override IEnumerator Run()
        {
            _characterGameplay.Run();

            yield break;
        }

        private void Update()
        {
            _entitiesLifeContext?.Update(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F))
            {
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }
        }
    }
}