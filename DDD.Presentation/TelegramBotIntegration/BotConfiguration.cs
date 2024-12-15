using Microsoft.Extensions.Configuration;

namespace DDD.Presentation.TelegramBotIntegration
{
    /// <summary>
    /// Класс для конфигурации
    /// </summary>
    public static class BotConfiguration
    {
        /// <summary>
        /// Загрузка конфигурации из файла 
        /// </summary>
        private static IConfiguration LoadConfiguration()
        {
            // Путь к корневой папке проекта
            var basePath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;

            if (basePath == null)
                throw new InvalidOperationException("Не удалось определить корневую папку проекта.");
            
            var appSettingsPath = Path.Combine(basePath, "appsettings.json");
            var appSettingsDevelopmentPath = Path.Combine(basePath, "appsettings.Development.json");
            
            if (!File.Exists(appSettingsPath))
                throw new FileNotFoundException($"Configuration file not found at: {appSettingsPath}");
            
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true)
                .AddJsonFile(appSettingsDevelopmentPath, optional: true, reloadOnChange: true);

            return configBuilder.Build();
        }

        /// <summary>
        /// Возвращает api-token
        /// </summary>
        public static string GetApiKey()
        {
            var config = LoadConfiguration();
            // Получаем значение API ключа из конфигурации
            return config["BotConfig:ApiKey"] ?? throw new InvalidOperationException("API ключ не найден в конфигурации.");
        }
    }
}