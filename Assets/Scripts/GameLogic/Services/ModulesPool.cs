using System.Collections.Generic;

namespace QuanticCollapse
{
    public class ModulesPool
    {
        private int _generalPoolSize = 5;

        private PopUpComponentType[] _availableModules = new[]
        {
            PopUpComponentType.Header,
            PopUpComponentType.Text,
            PopUpComponentType.Image,
            PopUpComponentType.Price,
            PopUpComponentType.Button,
            PopUpComponentType.CloseButton,
        };

        private Dictionary<PopUpComponentType, Queue<IPopUpComponentData>> _modulesPoolsDictionary = new();

        public IPopUpComponentData SpawnModule(PopUpComponentType type) => _modulesPoolsDictionary[type].Dequeue();
        public void DeSpawnModule(PopUpComponentType type, IPopUpComponentData kind)
            => _modulesPoolsDictionary[type].Enqueue(kind);

        public void Initialize()
        {
            foreach (PopUpComponentType modules in _availableModules)
            {
                Queue<IPopUpComponentData> modulePool = new();

                switch (modules)
                {
                    case PopUpComponentType.Price:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new PricePopUpComponentData());
                        break;
                    case PopUpComponentType.Button:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new ButtonPopUpComponentData());
                        break;
                    case PopUpComponentType.CloseButton:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new CloseButtonPopUpComponentData());
                        break;
                    case PopUpComponentType.Image:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new ImagePopUpComponentData());
                        break;
                    case PopUpComponentType.Text:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new TextPopUpComponentData());
                        break;
                    case PopUpComponentType.Header:
                        for (int i = 0; i < _generalPoolSize; i++)
                            modulePool.Enqueue(new HeaderPopUpComponentData());
                        break;
                }
                _modulesPoolsDictionary.Add(modules, modulePool);
            }
        }
    }
}