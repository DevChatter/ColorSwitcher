using ColorSwitcher.Core;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ColorSwitcher.Web.Hubs.Emitters
{
    public class ColorEmitter : IColorEmitter
    {
        private readonly IHubContext<ColorHub, IColorNotification> _colorHub;

        public ColorEmitter(IHubContext<ColorHub, IColorNotification> colorHub)
        {
            _colorHub = colorHub;
        }

        public Task ShowNewColor(string color)
        {
            return _colorHub.Clients.All.ColorChanged(color);
        }
    }
}