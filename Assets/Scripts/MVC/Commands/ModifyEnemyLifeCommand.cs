using UnityEngine.UI;

public class ModifyEnemyLifeCommand : IGridCommand
{
    private int _lifeAmount;
    private Slider _slider;
    private GenericEventBus winConfitionEventBus;
    public ModifyEnemyLifeCommand(GenericEventBus eventBus, int lifeAmountl, Slider UIView)
    {
        winConfitionEventBus = eventBus;
        _lifeAmount = lifeAmountl;
        _slider = UIView;
    }
    public void Do(VirtualGridModel Model)
    {
        Model.EnemyLife += _lifeAmount;
        _slider.value += _lifeAmount;

        if (Model.EnemyLife <= 0)
        {
            winConfitionEventBus.NotifyEvent();
        }
    }
}
