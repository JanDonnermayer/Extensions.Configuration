namespace Extensions.Configuration.Resolver
{
    internal interface IValueProvider
    {
        string GetValue(string key);

        (bool success, string? value) TryGetValue(string key);
    }
}
