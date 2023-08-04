using System.Windows.Input;

namespace MAUIWoL.Views;

public partial class About : ContentPage
{
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
    public About()
	{
		InitializeComponent();
        BindingContext = this;
    }
}