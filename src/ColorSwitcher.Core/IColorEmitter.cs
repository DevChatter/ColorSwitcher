using System.Threading.Tasks;

namespace ColorSwitcher.Core
{
    public interface IColorEmitter
    {
        Task ShowNewColor(string color);
    }
}