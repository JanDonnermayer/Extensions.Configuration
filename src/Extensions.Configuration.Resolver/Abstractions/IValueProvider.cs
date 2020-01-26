namespace Microsoft.Extensions.Configuration
{
    internal interface IValueProvider
    {
        string GetValue(string key);

        bool TryGetValue(string key, out string? value);
    }
}
