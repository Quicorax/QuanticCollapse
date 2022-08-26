

using UnityEngine;

public class DeAthomizerExternalBooster : ExternalBoosterBase, IExternalBooster
{
    private UserInputManager _inputManager;

    const string DeAthomizer = "DeAthomizer";
    public DeAthomizerExternalBooster(MasterSceneManager master, ExternalBoosterElements elements, VirtualGridView view)
    {
        View = view;
        MasterSceneManager = master;
        ButtonRef = elements.buttonReference;
        TextRef = elements.textRefeference;

        SetCountText();
        SetButtonInteractable();

        _inputManager = Object.FindObjectOfType<UserInputManager>(); //TODO: Remove this Find
    }

    public override void Execute()
    {
        if (_inputManager.deAthomizerBoostedInput)
        {
            MasterSceneManager.Inventory.AddElement(DeAthomizer, 1);

            _inputManager.deAthomizerBoostedInput = false;
        }
        else
        {
            MasterSceneManager.Inventory.RemoveElement(DeAthomizer, 1);
            SetButtonInteractable();

            _inputManager.deAthomizerBoostedInput = true;
        }

        SetCountText();
    }
    void SetButtonInteractable() => ButtonRef.interactable = CheckBoosterNotEmpty(MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer));
    public void SetCountText() => SetBoosterCountText(MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer), TextRef);
}
