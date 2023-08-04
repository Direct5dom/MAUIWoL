using MAUIWoL.Views;

namespace MAUIWoL;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AddConfigPage), typeof(AddConfigPage));
    }
}
