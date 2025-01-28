using Microsoft.Extensions.Logging;

namespace BangkokTravel
{
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
                    fonts.AddFont("Manrope-Regular.ttf", "ManropeRegular");
                    fonts.AddFont("Manrope-SemiBold.ttf", "ManropeSemiBold");
                    fonts.AddFont("Manrope-Bold.ttf", "ManropeBold");
                    fonts.AddFont("Pattaya-Regular.ttf", "PattayaRegular");
                    fonts.AddFont("Prompt-Regular", "ThaiRegular");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
