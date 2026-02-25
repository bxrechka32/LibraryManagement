using System.Windows;
using LibraryManagement.Data;

namespace LibraryManagement;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Инициализация базы данных при запуске приложения
        using var context = new LibraryContext();
        DbInitializer.Initialize(context);
    }
}
