using ShellDemo.Models;

namespace ShellDemo.Pages;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}

	// Wywoływane za każdym razem, gdy strona staje się widoczna
	// (także po powrocie z "details"/"edit") – odświeżamy listę, bo nazwa mogła się zmienić.
	protected override void OnAppearing()
	{
		base.OnAppearing();
		ItemsView.ItemsSource = null;
		ItemsView.ItemsSource = ItemStore.All;
	}

	// Wywoływane po dotknięciu elementu listy.
	private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is not Item item)
			return;

		// Czyścimy zaznaczenie, żeby ten sam element można było dotknąć ponownie.
		ItemsView.SelectedItem = null;

		// Trasa względna + parametr w query string (zawsze jako tekst).
		// Shell tworzy DetailsPage i ustawia jej właściwość oznaczoną [QueryProperty("ItemId", "id")].
		await Shell.Current.GoToAsync($"details?id={item.Id}");
	}
}
