namespace Rewriting.WebApp
{
    public partial class MainLayout
    {
        public bool IsMenuOpened { get; set; }

        public void ToggleMenu()
        {
            IsMenuOpened = !IsMenuOpened;
        }
    }
}
