using TMPro;

public class ExternalBoosters
{
    public TMP_Text textRef;
    public int usesLeft = 5;
    public bool CheckUses(int uses) { return uses > 0; } 
    public virtual void TryUse() { }
}
