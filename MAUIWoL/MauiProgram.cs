using MAUIWoL.Views;
using MAUIWoL.Data;
using Microsoft.Extensions.Logging;

namespace MAUIWoL;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<WoL>();
        builder.Services.AddTransient<AddConfigPage>();

        builder.Services.AddSingleton<WoLConfigDatabase>();

        return builder.Build();
	}
}
