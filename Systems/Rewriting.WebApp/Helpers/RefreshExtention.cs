using Microsoft.AspNetCore.Components;

namespace Rewriting.WebApp;

public static class RefreshExtention
{
    public static void RefreshPage(this NavigationManager navigationManager)
    {
        navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
    }
}
