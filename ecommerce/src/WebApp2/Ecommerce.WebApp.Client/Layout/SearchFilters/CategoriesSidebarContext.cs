namespace Ecommerce.WebApp.Client.Layout.SearchFilters;

public class CategoriesSidebarContext
{
    public event Action? OnChange;
    public bool IsOpen { get; set; } = false;
    public void SetIsOpen(bool value) {
        Console.WriteLine($"Setting IsOpen to {value}");
        IsOpen = value;
        OnChange?.Invoke();
    }
}