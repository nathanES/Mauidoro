using Mauidoro.ViewModel;
using Microsoft.Extensions.Logging;

namespace Mauidoro;

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
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>(); // est créé qu'une fois 
		
		builder.Services.AddTransient<DetailPage>(); // est créé à chaque fois que l'on va sur la page, puis est détruit
		builder.Services.AddTransient<DetailViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
