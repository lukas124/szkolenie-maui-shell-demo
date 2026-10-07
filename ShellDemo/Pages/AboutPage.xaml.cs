namespace ShellDemo.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

	// Przy każdym wejściu w zakładkę pokazujemy bieżący adres Shell (np. //about).
	protected override void OnAppearing()
	{
		base.OnAppearing();
		LocationLabel.Text = $"Bieżąca lokalizacja: {Shell.Current.CurrentState.Location}";
	}

	// "//" = trasa absolutna: przełącza na zakładkę "list" (z AppShell.xaml)
	// i czyści stos nawigacji tej zakładki do jej strony głównej.
	private async void OnGoToListClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//list");
}
