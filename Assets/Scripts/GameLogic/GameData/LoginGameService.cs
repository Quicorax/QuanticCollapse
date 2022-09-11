using System.Threading.Tasks;
using Unity.Services.Authentication;

public partial class LoadSceneLogic
{
    public class LoginGameService : IService
    {
        public async Task Initialize()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        public void Clear()
        {
        }
    }
}
