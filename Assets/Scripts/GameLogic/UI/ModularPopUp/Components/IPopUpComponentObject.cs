using System;
using UnityEngine;

public enum ModuleTypes { Price , Button , CloseButton , Image, Text, Header };
public interface IPopUpComponentObject
{
    public void SetData(IPopUpComponentData unTypedData, Action closeOnUse);
}
