using ShellDemo.Pages;

namespace ShellDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Trasy stron, których NIE ma w AppShell.xaml (nie są zakładkami).
		// Rejestrujemy je raz, przy starcie aplikacji (App.CreateWindow tworzy AppShell).
		// Potem GoToAsync("details?id=2") tworzy nową DetailsPage i wkłada ją na stos nawigacji bieżącej zakładki.
		Routing.RegisterRoute("details", typeof(DetailsPage));
		Routing.RegisterRoute("edit", typeof(EditPage));
	}
}
