namespace Configuration.Library
{
    public interface IConfigurationReader
    {
        T GetValue<T>(string key);
    }
}
