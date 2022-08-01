using UnityEngine.UI;

public class ModifyEnemyLifeCommand : IGridCommand
{
    private int _lifeAmount;
    private Slider _slider;
    public ModifyEnemyLifeCommand(int lifeAmountl, Slider UIView)
    {
        _lifeAmount = lifeAmountl;
        _slider = UIView;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.EnemyLife += _lifeAmount;
        _slider.value += _lifeAmount;
    }
}
