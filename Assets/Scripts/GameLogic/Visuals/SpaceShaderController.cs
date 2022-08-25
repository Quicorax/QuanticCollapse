using UnityEngine;

public class SpaceShaderController : MonoBehaviour
{
    const string ColorNameA = "_BgColorA";
    const string ColorNameB = "_BgColorB";
    [SerializeField] private LevelInjectedEventBus _LevelInjected;

    private MeshRenderer spaceMaterial;

    private void Awake()
    {
        _LevelInjected.Event += SetLevelData;
        spaceMaterial = GetComponent<MeshRenderer>();
    }

    private void OnDisable()
    {
        _LevelInjected.Event -= SetLevelData;
    }

    void SetLevelData(LevelGridData data)
    {
        spaceMaterial.material.SetColor(ColorNameA, data.SpaceColorA);
        spaceMaterial.material.SetColor(ColorNameB, data.SpaceColorB);
    }

    
}
