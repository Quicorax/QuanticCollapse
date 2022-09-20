using System.Threading.Tasks;

public interface IIAPGameService : IService
{
    public bool IsReady();
    public Task<bool> StartPurchase(string product);
    public string GetRemotePrice(string product);

}
