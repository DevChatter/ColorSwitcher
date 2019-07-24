using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ColorSwitcher.Web.Hubs
{
    public class ColorHub : Hub<IColorNotification>
    {
        
    }

    public interface IColorNotification
    {
        Task ColorChanged(string color);
    }
}