namespace Extensions.Configuration.Resolver
{
    internal interface IValueProvider
    {
        string GetValue(string key);

        bool TryGetValue(string key, out string? value);
    }
}
