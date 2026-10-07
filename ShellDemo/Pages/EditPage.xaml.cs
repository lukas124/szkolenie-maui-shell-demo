using ShellDemo.Models;

namespace ShellDemo.Pages;

// SPOSÓB 2: IQueryAttributable
// Wszystkie parametry nawigacji trafiają naraz do jednej metody ApplyQueryAttributes.
// Pozwala odebrać obiekt przekazany przez ShellNavigationQueryParameters (nie tylko tekst).
public partial class EditPage : ContentPage, IQueryAttributable
{
	private Item? _item;

	public EditPage()
	{
		InitializeComponent();
	}

	// MAUI wywołuje tę metodę podczas nawigacji do strony: po konstruktorze, przed OnAppearing.
	// Klucz "item" to ten sam klucz, którego użyła DetailsPage w GoToAsync("edit", ...).
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		_item = (Item)query["item"];

		InfoLabel.Text = $"Otrzymano obiekt Item (Id = {_item.Id})";
		NameEntry.Text = _item.Name;
	}

	private async void OnSaveClicked(object? sender, EventArgs e)
	{
		if (_item is not null)
			_item.Name = NameEntry.Text ?? "";

		// Wracamy do DetailsPage – jej OnAppearing pokaże nową nazwę.
		await Shell.Current.GoToAsync("..");
	}
}
