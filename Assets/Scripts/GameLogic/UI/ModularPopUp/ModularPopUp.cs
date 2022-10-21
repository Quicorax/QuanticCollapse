using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class ModularPopUp : MonoBehaviour
    {
        [SerializeField] 
        private CanvasGroup _canvasGroup;
        [SerializeField] 
        private RectTransform _parent;
        [SerializeField] 
        private VerticalLayoutGroup _layout;

        private AddressablesService _addressables;

        private int _initialPanelOffset = 35;

        private float _moduleSize;
        private void Awake()
        {
            _addressables = ServiceLocator.GetService<AddressablesService>();
        }
        public void GeneratePopUp(IPopUpComponentData[] componentsToAdd)
        {
            _canvasGroup.alpha = 0;
            int currentModules = 0;

            _moduleSize = _initialPanelOffset;

            foreach (var moduleData in componentsToAdd)
            {
                _moduleSize += moduleData.ModuleHeight + 20;

                string adressableKey = "Module_" + moduleData.ModuleConcept;
                _addressables.LoadAdrsOfComponent<IPopUpComponentObject>(adressableKey, _parent, component =>
                {
                    component.SetData(moduleData, CloseSelf);
                    currentModules++;

                    if (currentModules == componentsToAdd.Length)
                    {
                        GenerationComplete();
                        ServiceLocator.GetService<PopUpService>().DeSpawnPopUp();
                    }
                });
            }
        }
        public void CloseSelf()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, 0.2f).OnComplete(() => _addressables.ReleaseAddressable(gameObject));
        }

        void GenerationComplete()
        {
            _parent.sizeDelta += new Vector2(0, _moduleSize);

            _parent.DOPunchScale(Vector3.one * 0.1f, .5f);
            _canvasGroup.DOFade(1, 0.3f);
            StartCoroutine(SetElementsOnDisposition());
        }
        IEnumerator SetElementsOnDisposition()
        {
            _layout.enabled = true;
            yield return new WaitForEndOfFrame();
            _layout.enabled = false;
        }
    }
}