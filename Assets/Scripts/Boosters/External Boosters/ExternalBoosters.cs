public class ExternalBoosters
{
    public int usesLeft = 999;
    public bool CheckUses(int uses) { return uses > 0; } 
    public virtual void TryUse(VirtualGridView View) { }
}
