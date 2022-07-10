using UnityEngine;

public class StarshipManager : MonoBehaviour
{
    public int kindAScore;
    public int kindBScore;
    public int kindCScore;
    public int kindDScore;

    public void AddScoreOfKind(ElementKind kind, int amount)
    {
        switch (kind)
        {
            case ElementKind.A:
                kindAScore += amount;
                break;
            case ElementKind.B:
                kindBScore += amount;
                break;
            case ElementKind.C:
                kindCScore += amount;
                break;
            case ElementKind.D:
                kindDScore += amount;
                break;
        }
    }
}
