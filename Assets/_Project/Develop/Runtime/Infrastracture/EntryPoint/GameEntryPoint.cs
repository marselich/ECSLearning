using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Старт проекта, сетап настроек");

            SetupAppSettings();

            Debug.Log("Процесс регистрации сервисов всего проекта");

            DIContainer projectContainer = new DIContainer();

            ProjectContextRegistrations.Process(projectContainer);

            projectContainer.Initialize();

            projectContainer.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(projectContainer));
        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();
            //PlayerDataProvider playerDataProvider = container.Resolve<PlayerDataProvider>();

            Debug.Log("Открывается штора загрузки");
            loadingScreen.Show();

            Debug.Log("Начинается инициализация сервисов");

            yield return container.Resolve<ConfigsProviderService>().LoadAsync();

            //bool isPlayerDataSaveExists = false;

            //yield return playerDataProvider.Exists(result => isPlayerDataSaveExists = result);

            //if (isPlayerDataSaveExists)
            //    yield return playerDataProvider.Load();
            //else
            //    playerDataProvider.Reset();

            yield return new WaitForSeconds(1); // для симуляции загрузки

            Debug.Log("Завершается инициализация сервисов");

            Debug.Log("Закрывается штора загрузки");
            loadingScreen.Hide();

            Debug.Log("Начинается переход на какую-то сцену");
            //yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
            yield return sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(1));
        }
    }
}