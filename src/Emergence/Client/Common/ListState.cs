using System;

namespace Emergence.Client.Common
{
    public class ListState
    {
        public event Action OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
