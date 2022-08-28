using UnityEngine;

public class GameLevelsView : MonoBehaviour
{
    public GameLevelsController GameLevelsController;

    [SerializeField] private LevelView _levelView;
    [SerializeField] private Transform _parent;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        GameLevelsController = new();

        foreach (LevelModel level in GameLevelsController.GameLevelsModel.Levels)
        {
            Instantiate(_levelView, _parent).Initialize(level, OnNavigateToLevel);
        }
    }

    private void OnNavigateToLevel(LevelModel levelModel)
    {
        GameLevelsController.NavigateToLevel(levelModel);
    }
}

