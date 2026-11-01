using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer c, GameplayInputArgs args)
        {
            c.RegisterAsSingle<EntitiesFactory>(CreateEntitiesFactory);

            c.RegisterAsSingle<EntitiesLifeContext>(CreateEntitiesLifeContext);

            c.RegisterAsSingle<MonoEntitiesFactory>(CreateMonoEntitiesFactory).NonLazy();

            c.RegisterAsSingle<CollidersRegistryService>(CreateCollidersRegistryService);

            c.RegisterAsSingle<BrainsFactory>(CreateBrainsFactory);

            c.RegisterAsSingle<AIBrainsContext>(CreateAIBrainsContext);
        }

        private static AIBrainsContext CreateAIBrainsContext(DIContainer c)
            => new AIBrainsContext();

        private static BrainsFactory CreateBrainsFactory(DIContainer c)
            => new BrainsFactory(c);

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer c)
            => new EntitiesLifeContext();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer c)
            => new EntitiesFactory(c);

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer c)
            => new CollidersRegistryService();

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer c)
            => new MonoEntitiesFactory(
                c.Resolve<ResourcesAssetsLoader>(),
                c.Resolve<EntitiesLifeContext>(),
                c.Resolve<CollidersRegistryService>());
    }
}