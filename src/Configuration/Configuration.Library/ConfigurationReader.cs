using Configuration.Data.Abstractions.Interfaces;
using Configuration.Data.Models.Entities;
using Configuration.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Configuration.Library
{
    public class ConfigurationReader : IConfigurationReader, IDisposable
    {
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        private readonly IConfigurationStorage _configurationStorage;
        private readonly Timer _timer;
        private readonly SemaphoreSlim _semaphoreSlim = new(1);

        private List<ConfigModel> _configs;
        private bool _disposed = false;

        public ConfigurationReader(IConfigurationStorage configurationStorage, int refreshTimerIntervalInMs)
        {
            _applicationName = Assembly.GetEntryAssembly().GetName().Name ?? throw new ArgumentNullException(nameof(_applicationName));
            _configurationStorage = configurationStorage ?? throw new ArgumentNullException(nameof(configurationStorage));

            //hata fırlatmak yerine default değer de verilebilir.
            //_refreshTimerIntervalInMs = refreshTimerIntervalInMs > 0 ? refreshTimerIntervalInMs : 60_000;
            _refreshTimerIntervalInMs =
                refreshTimerIntervalInMs == 0 ? throw new ArgumentException("refreshTimerIntervalInMs 0 (Sıfır) olamaz!", nameof(refreshTimerIntervalInMs))
                : refreshTimerIntervalInMs;

            CheckForUpdatesAsync().GetAwaiter().GetResult();

            _timer = new Timer(async _ => await CheckForUpdatesAsync(), null, _refreshTimerIntervalInMs, _refreshTimerIntervalInMs);
        }

        /// <summary>
        /// Yapılandırma listesinden bir ögeyi döner.
        /// </summary>
        /// <typeparam name="T">Config Tipi</typeparam>
        /// <param name="key">Config Adı</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            _semaphoreSlim.Wait();
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (_configs == null || _configs.Count == 0)
                {
                    throw new InvalidOperationException("Yapılandırma listesi boş!");
                }

                var config = _configs?.FirstOrDefault(c => c.Name == key);
                if (config == null)
                {
                    throw new ConfigNotFoundException($"Sistemde {key} değeri bulunamadı!");
                }

                if (config.Type != typeof(T).FullName)
                {
                    throw new TypeMismatchException($"Sistemdeki {config.Name} değerinin tipi {config.Type} fakat alınmak istenen tip {typeof(T).Name}.Tip eşleşmedi!");
                }

                var value = ConvertValue<T>(config);

                return (T)value;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private static object ConvertValue<T>(ConfigModel config)
        {
            Type targetType = typeof(T);
            string configValue = config.Value;
            string invalidFormatMessage = $"Geçersiz {targetType.Name.ToLower()} değeri: {configValue}";

            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.String:
                    return configValue;

                case TypeCode.Int32:
                    if (int.TryParse(configValue, out int intValue))
                    {
                        return intValue;
                    }
                    throw new FormatException(invalidFormatMessage);
                case TypeCode.Boolean:
                    if (bool.TryParse(configValue, out bool boolValue))
                    {
                        return boolValue;
                    }
                    throw new FormatException(invalidFormatMessage);
                case TypeCode.Double:
                    if (double.TryParse(configValue, out double doubleValue))
                    {
                        return doubleValue;
                    }
                    throw new FormatException(invalidFormatMessage);
                default:
                    throw new InvalidCastException($"Geçersiz tip: {targetType.Name}");
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var newConfigs = await _configurationStorage.GetAllByApplicationNameAsync(_applicationName);

                if (newConfigs != null && newConfigs.Any())
                {
                    _configs = newConfigs;
                }
            }
            catch (Exception ex)
            {
                // Logla ve bildirim gönder
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer.Dispose();
                    _semaphoreSlim.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}