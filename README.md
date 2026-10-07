# Shell Demo – nawigacja Shell w .NET MAUI

Minimalne demo do slajdu „Shell w pigułce” (szkolenie „Podstawy .NET MAUI”).
Pokazuje **wyłącznie** nawigację Shell – bez MVVM Toolkit, bez dodatkowego DI, logika w code-behind.

Projekt powstał z szablonu `dotnet new maui -n ShellDemo` (.NET 10 / MAUI 10), usunięto `MainPage`
oraz platformy MacCatalyst/Windows.

## Co pokazuje demo

| Element | Gdzie w kodzie |
|---|---|
| `TabBar` z 2 zakładkami (`Lista`, `O aplikacji`) | `ShellDemo/AppShell.xaml` |
| Rejestracja tras stron spoza `AppShell.xaml`: `Routing.RegisterRoute("details", typeof(DetailsPage))` | `ShellDemo/AppShell.xaml.cs` |
| Nawigacja z parametrem tekstowym: `GoToAsync($"details?id={item.Id}")` | `ShellDemo/Pages/ListPage.xaml.cs` |
| **Sposób 1** odbioru parametru: `[QueryProperty(nameof(ItemId), "id")]` | `ShellDemo/Pages/DetailsPage.xaml.cs` |
| Przekazanie **obiektu**: `GoToAsync("edit", new ShellNavigationQueryParameters { ["item"] = _item })` | `ShellDemo/Pages/DetailsPage.xaml.cs` |
| **Sposób 2** odbioru parametru: `IQueryAttributable.ApplyQueryAttributes(...)` | `ShellDemo/Pages/EditPage.xaml.cs` |
| Powrót: strzałka w pasku nawigacji albo `GoToAsync("..")` | `DetailsPage`, `EditPage` |
| Trasa absolutna: `GoToAsync("//list")` (przełączenie zakładki) | `ShellDemo/Pages/AboutPage.xaml.cs` |

### Dwa sposoby odbioru parametrów – kiedy który?

- **`[QueryProperty]`** (DetailsPage) – jeden atrybut na parametr, MAUI ustawia wskazaną właściwość
  po konstruktorze, a przed `OnAppearing`. Wartość z adresu (`?id=2`) zawsze przychodzi jako `string`.
  Dobre dla prostych parametrów: id, filtr, tryb.
- **`IQueryAttributable`** (EditPage) – wszystkie parametry trafiają naraz do `ApplyQueryAttributes(IDictionary<string, object>)`,
  również po konstruktorze i przed `OnAppearing`. Można odebrać **obiekt** przekazany przez
  `ShellNavigationQueryParameters` (parametry jednorazowe – nie są przekazywane ponownie przy powrocie `..`).
  Dobre, gdy parametrów jest kilka albo chcemy przekazać gotowy obiekt.

Przepływ w demo: **Lista** → dotknięcie elementu → **Szczegóły** (`details?id=1`, `[QueryProperty]`)
→ „Edytuj” → **Edycja** (obiekt `Item`, `IQueryAttributable`) → „Zapisz i wróć” (`..`) → Szczegóły z nową nazwą.

## Mapa tras

```
//list                 zakładka „Lista”        → ListPage      (AppShell.xaml, Route="list")
//about                zakładka „O aplikacji”  → AboutPage     (AppShell.xaml, Route="about")

//list/details?id=N    szczegóły elementu      → DetailsPage   (Routing.RegisterRoute("details", ...))
//list/details/edit    edycja elementu         → EditPage      (Routing.RegisterRoute("edit", ...))
                                                 parametr: obiekt Item pod kluczem "item"
..                     jeden poziom wstecz
```

## Uruchomienie

Wymagania: .NET SDK 10 (patrz `global.json`) z workloadem `maui`
(`dotnet workload install maui`), Android SDK z emulatorem; dla iOS – macOS z Xcode.

```bash
cd ShellDemo

# Android – uruchomiony emulator (lub podłączone urządzenie)
dotnet build -t:Run -f net10.0-android

# iOS – symulator (domyślny symulator wybrany przez narzędzia .NET)
dotnet build -t:Run -f net10.0-ios
```

Gdy podłączonych jest kilka urządzeń/emulatorów albo chcesz wskazać konkretny symulator:

```bash
# Android – konkretny emulator (lista: adb devices)
dotnet build -t:Run -f net10.0-android "-p:AdbTarget=-s emulator-5554"

# iOS – konkretny symulator po UDID (lista: xcrun simctl list devices)
dotnet build -t:Run -f net10.0-ios -p:_DeviceName=:v2:udid=<UDID>
```

Uwaga: w `ShellDemo.csproj` jest `<ValidateXcodeVersion>false</ValidateXcodeVersion>`, żeby build iOS
nie blokował się, gdy pakiet .NET for iOS oczekuje nieco nowszego Xcode niż zainstalowany.
W projekcie produkcyjnym usuń to ustawienie i trzymaj zgodne wersje.

## Struktura

```
ShellDemo/
  AppShell.xaml(.cs)      TabBar + rejestracja tras
  Models/Item.cs          model + stała lista danych (ItemStore)
  Pages/ListPage          zakładka 1, nawigacja details?id=...
  Pages/AboutPage         zakładka 2, trasa absolutna //list
  Pages/DetailsPage       [QueryProperty]
  Pages/EditPage          IQueryAttributable
```
