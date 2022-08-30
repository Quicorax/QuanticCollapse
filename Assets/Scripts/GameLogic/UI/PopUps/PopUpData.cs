using System;

public class PopUpData
{
    public bool HasHighlightedHeader = false;
    public bool HasBasicHeader = false;
    public string HeaderText;

    public bool HasButton = false;
    public string ButtonText;
    public Action onButtonClickedAction;

    public bool HasIcon = false;
    public string IconName;

    public bool HasBodyText = false;
    public string BodyText;

    public void SetHeader(string text, bool isHighlighted)
    {
        if (isHighlighted)
            HasHighlightedHeader = true;
        else
            HasBasicHeader = true;

        HeaderText = text;
    }

    public void SetButton(string text, Action action)
    {
        HasButton = true;
        ButtonText = text;
        onButtonClickedAction = action;
    }

    public void SetIcon(string text)
    {
        HasIcon = true;
        IconName = text;
    }
    public void SetBodyText(string text)
    {
        HasBodyText = true;
        BodyText = text;
    }
}
