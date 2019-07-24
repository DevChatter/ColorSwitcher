using System;
using System.Threading.Tasks;

namespace ColorSwitcher.Extensions
{
    public static class TaskExtensions
    {
        public static async void RunInBackgroundSafely(
            this Task task, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (!(onException is null))
            {
                onException(ex);
            }
        }
    }
}