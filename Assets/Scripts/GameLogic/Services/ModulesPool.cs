using System.Collections.Generic;

namespace QuanticCollapse
{
    public class ModulesPool
    {
        private readonly int _generalPoolSize = 5;

        private readonly PopUpComponentType[] _availableModules =
        {
            PopUpComponentType.Header,
            PopUpComponentType.Text,
            PopUpComponentType.Image,
            PopUpComponentType.Price,
            PopUpComponentType.Button,
            PopUpComponentType.CloseButton,
        };

        private readonly Dictionary<PopUpComponentType, Queue<IPopUpComponentData>> _modulesPoolsDictionary = new();

        public IPopUpComponentData SpawnModule(PopUpComponentType type) => _modulesPoolsDictionary[type].Dequeue();

        public void DeSpawnModule(PopUpComponentType type, IPopUpComponentData kind) =>
            _modulesPoolsDictionary[type].Enqueue(kind);

        public void Initialize()
        {
            foreach (var modules in _availableModules)
            {
                Queue<IPopUpComponentData> modulePool = new();

                switch (modules)
                {
                    case PopUpComponentType.Price:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new PricePopUpComponentData());
                        break;
                    case PopUpComponentType.Button:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new ButtonPopUpComponentData());
                        break;
                    case PopUpComponentType.CloseButton:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new CloseButtonPopUpComponentData());
                        break;
                    case PopUpComponentType.Image:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new ImagePopUpComponentData());
                        break;
                    case PopUpComponentType.Text:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new TextPopUpComponentData());
                        break;
                    case PopUpComponentType.Header:
                        for (var i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new HeaderPopUpComponentData());
                        break;
                }
                
                _modulesPoolsDictionary.Add(modules, modulePool);
            }
        }
    }
}