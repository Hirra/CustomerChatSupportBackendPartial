using System.Threading.Tasks;

namespace ChatAPI.Utilites.Interfaces
{
    public interface IChatSupport
    {
        Task<bool> RefuseNewChats();
    }
}
