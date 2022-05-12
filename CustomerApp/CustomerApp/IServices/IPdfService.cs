using System;
using System.Threading.Tasks;

namespace CustomerApp.IServices
{
    public interface IPdfService
    {
        Task View(string url, string name);
    }
}
