using System.Threading.Tasks;

namespace QuanticCollapse
{
    public interface IGameProgressionProvider
    {
        Task<bool> Initialize();
        string Load();
        void Save(string text);
    }
}