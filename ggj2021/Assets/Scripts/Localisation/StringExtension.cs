public static class StringExtensions
{
    public static string Localised(this string entry)
    {
        return LocalisationSystem.GetLocalisedValue(entry);
    }
}