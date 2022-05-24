using System;
namespace CustomerApp.IServices
{
    public interface IDatetimeService
    {
        string TimeAgo(DateTime date);
    }
}
