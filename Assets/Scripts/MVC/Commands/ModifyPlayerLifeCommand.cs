using UnityEngine.UI;

public class ModifyPlayerLifeCommand : IGridCommand
{
    private int _lifeAmount;
    private Slider _slider;
    private GenericEventBus loseConditionEventBus;
    public ModifyPlayerLifeCommand(GenericEventBus eventBus, int lifeAmountl, Slider UIView)
    {
        loseConditionEventBus = eventBus;
        _lifeAmount = lifeAmountl;
        _slider = UIView;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.PlayerLife += _lifeAmount;
        _slider.value += _lifeAmount;


        if (Model.PlayerLife <= 0)
        {
            loseConditionEventBus.NotifyEvent();
        }
    }
}
