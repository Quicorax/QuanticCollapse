using UnityEngine;

public class ColorPackModel
{
    public Color BaseColor;
    public Color SecondaryColor;
    public Color SignatureColor;

    public ColorPackModel(Color[] Colors)
    {
        BaseColor = Colors[0];
        SecondaryColor = Colors[1];
        SignatureColor = Colors[2];
    }
}

