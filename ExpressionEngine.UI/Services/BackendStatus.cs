namespace ExpressionEngine.UI.Services
{
    public sealed class BackendStatus
    {
        public bool IsAvailable { get; private set; } = true;
        public event Action? Changed;

        public void MarkDown()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                Changed?.Invoke();
            }
        }

        public void MarkUp()
        {
            if (!IsAvailable)
            {
                IsAvailable = true;
                Changed?.Invoke();
            }
        }
    }
}
