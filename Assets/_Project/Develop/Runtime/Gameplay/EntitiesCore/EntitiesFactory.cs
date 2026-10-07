using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.RotationFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _lifeContext;
        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _lifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
        }

        public Entity CreateRigidbodyCharacterEntity(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/RigidbodyCharacterEntity");

            entity
                .AddMoveDirection()
                .AddMoveSpeed(new ReactiveVariable<float>() { Value = 10 })
                .AddRotationSpeed(new ReactiveVariable<float>() { Value = 900 });

            entity
                .AddSystem(new RigidbodyMovementSystem())
                .AddSystem(new RigidbodyRotationSystem());

            _lifeContext.Add(entity);

            return entity;
        }

        public Entity CreateCharacterControllerCharacterEntity(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/CharacterControllerCharacterEntity");

            entity
                .AddMoveDirection()
                .AddMoveSpeed(new ReactiveVariable<float>() { Value = 10 })
                .AddRotationSpeed(new ReactiveVariable<float>() { Value = 900 });

            entity
                .AddSystem(new CharacterControllerMovementSystem())
                .AddSystem(new TransformRotationSystem());

            _lifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}