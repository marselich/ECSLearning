using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class ProjectContextRegistrations
    {
        public static void Process(DIContainer c)
        {
            c.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);

            c.RegisterAsSingle<ConfigsProviderService>(CreateConfigsProviderService);

            c.RegisterAsSingle<ResourcesAssetsLoader>(CreateResourcesAssetsLoader);

            c.RegisterAsSingle<SceneLoaderService>(CreateSceneLoaderService);

            c.RegisterAsSingle<ILoadingScreen>(CreateStandartLoadingScreen);

            c.RegisterAsSingle<SceneSwitcherService>(CreateSceneSwitcherService);
        }

        private static ConfigsProviderService CreateConfigsProviderService(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) => new ResourcesAssetsLoader();

        private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            CoroutinesPerformer coroutinesPerformerPrefab
                = resourcesAssetsLoader.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Object.Instantiate(coroutinesPerformerPrefab);
        }

        private static SceneLoaderService CreateSceneLoaderService(DIContainer c) => new SceneLoaderService();

        private static StandartLoadingScreen CreateStandartLoadingScreen(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            StandartLoadingScreen standartLoadingScreenPrefab
                = resourcesAssetsLoader.Load<StandartLoadingScreen>("Utilities/StandartLoadingScreen");

            return Object.Instantiate(standartLoadingScreenPrefab);
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
            => new SceneSwitcherService(
                c.Resolve<SceneLoaderService>(),
                c.Resolve<ILoadingScreen>(),
                c);
    }
}