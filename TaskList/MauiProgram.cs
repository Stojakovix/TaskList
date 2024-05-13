using Microsoft.Extensions.Logging;
using TaskList.Data;
using TaskList.Model;
using TaskList.ViewModel;
using TaskList.Views;



namespace TaskList
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<Conn>();
            builder.Services.AddSingleton<MainItemPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            
#endif

            return builder.Build();
        }
    }
}
