using UnityEngine.UI;

public class ModifyPlayerLifeCommand : IGridCommand
{
    private int _lifeAmount;
    private Slider _slider;
    public ModifyPlayerLifeCommand(int lifeAmountl, Slider UIView)
    {
        _lifeAmount = lifeAmountl;
        _slider = UIView;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.PlayerLife += _lifeAmount;
        _slider.value += _lifeAmount;
    }
}
