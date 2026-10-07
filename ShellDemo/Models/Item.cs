namespace ShellDemo.Models;

// Prosty model elementu listy – bez MVVM, bez INotifyPropertyChanged.
public class Item
{
	public int Id { get; init; }
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";
}

// Stała lista danych „w pamięci” – zamiast bazy czy API, żeby demo było o samej nawigacji.
public static class ItemStore
{
	public static List<Item> All { get; } =
	[
		new() { Id = 1, Name = "Przegląd gaśnic", Description = "Budynek A, parter" },
		new() { Id = 2, Name = "Wymiana filtrów", Description = "Serwerownia, piętro 2" },
		new() { Id = 3, Name = "Kontrola oświetlenia", Description = "Parking podziemny" },
	];

	public static Item? Get(int id) => All.FirstOrDefault(i => i.Id == id);
}
