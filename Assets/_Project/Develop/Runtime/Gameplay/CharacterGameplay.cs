using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class CharacterGameplay : MonoBehaviour
    {
        [SerializeField] private CharacterType _characterType;

        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private Entity _entity;
        private Entity _teleportingCharacterEntity;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            switch (_characterType)
            {
                case CharacterType.Rigidbody:
                    _entity = _entitiesFactory.CreateRigidbodyCharacterEntity(Vector3.zero);
                    break;

                case CharacterType.CharacterController:
                    _entity = _entitiesFactory.CreateCharacterControllerCharacterEntity(Vector3.zero);
                    break;
            }

            _teleportingCharacterEntity = _entitiesFactory.CreateTeleportingCharacter(Vector3.zero + Vector3.forward * 2);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            _entity.MoveDirection.Value = input;

            if (Input.GetKeyDown(KeyCode.K))
            {
                _teleportingCharacterEntity.CurrentHealth.Value -= 50;
                Debug.Log(_teleportingCharacterEntity.CurrentHealth.Value);
            }
        }

        private enum CharacterType
        {
            Rigidbody,
            CharacterController
        }
    }
}