using System.Net.Mail;
using System.Net;
using System.Net.Sockets;
using MAUIWoL.Data;
using MAUIWoL.Models;
using System.Collections.ObjectModel;

namespace MAUIWoL.Views;

public partial class WoL : ContentPage
{
    WoLConfigDatabase database;
    public ObservableCollection<WoLConfig> Items { get; set; } = new();
    public WoL(WoLConfigDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;
    }

    async void OnConfigAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddConfigPage), true, new Dictionary<string, object>
        {
            ["Item"] = new WoLConfig()
        });
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not WoLConfig item)
            return;

        await Shell.Current.GoToAsync(nameof(AddConfigPage), true, new Dictionary<string, object>
        {
            ["Item"] = item
        });
    }
}