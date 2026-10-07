using ShellDemo.Models;

namespace ShellDemo.Pages;

// SPOSÓB 1: [QueryProperty]
// Mapuje parametr z adresu "details?id=2" na właściwość ItemId tej strony.
// Kolejność: Shell tworzy stronę (konstruktor) → ustawia ItemId → strona się pojawia (OnAppearing).
[QueryProperty(nameof(ItemId), "id")]
public partial class DetailsPage : ContentPage
{
	private Item? _item;

	public DetailsPage()
	{
		InitializeComponent();
		// Tu ItemId jest jeszcze puste – parametr przychodzi dopiero po konstruktorze.
	}

	// Setter wywołuje MAUI podczas nawigacji, zanim strona zostanie pokazana.
	// Wartość z query string zawsze przychodzi jako string – parsujemy sami.
	public string ItemId
	{
		set
		{
			_item = ItemStore.Get(int.Parse(value));
			ShowItem();
		}
	}

	// Po powrocie z "edit" (GoToAsync("..")) nazwa mogła się zmienić – odświeżamy widok.
	protected override void OnAppearing()
	{
		base.OnAppearing();
		ShowItem();
	}

	private void ShowItem()
	{
		if (_item is null)
			return;

		IdLabel.Text = $"Parametr id = {_item.Id}";
		NameLabel.Text = _item.Name;
		DescriptionLabel.Text = _item.Description;
	}

	private async void OnEditClicked(object? sender, EventArgs e)
	{
		if (_item is null)
			return;

		// Przekazanie OBIEKTU (nie tekstu). ShellNavigationQueryParameters = parametry jednorazowe:
		// nie zostają w historii, więc powrót ".." nie przekaże ich ponownie.
		// Odbiera je EditPage w ApplyQueryAttributes (IQueryAttributable).
		await Shell.Current.GoToAsync("edit", new ShellNavigationQueryParameters
		{
			["item"] = _item
		});
	}

	// ".." = jeden poziom wstecz (zdejmuje tę stronę ze stosu) – to samo co strzałka wstecz.
	private async void OnBackClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("..");
}
