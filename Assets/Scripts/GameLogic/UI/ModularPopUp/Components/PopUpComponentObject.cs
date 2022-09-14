using System;
using UnityEngine;

public class PopUpComponentObject : MonoBehaviour
{
    public string ModuleConcept;
    public virtual void SetData(PopUpComponentData unTypedData, Action closeOnUse) { }
}
