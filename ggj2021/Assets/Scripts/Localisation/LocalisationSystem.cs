using System.Collections.Generic;

public class LocalisationSystem
{
    public enum Language
    {
        English,
        Portugues,
    }

    public static Language language = Language.English;

    public static Dictionary<string, string> localisedPT;
    public static Dictionary<string, string> localisedEN;

    public static bool isInit;

    public static CSVLoader csvLoader;

    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.loadCSV();

        updateDictionaries();

        isInit = true;
    }

    public static void updateDictionaries()
    {
        localisedPT = csvLoader.getDictionaryValues("pt");
        localisedEN = csvLoader.getDictionaryValues("en");
    }

    public static Dictionary<string, string> getDictionaryForEditor()
    {
        if (!isInit) { Init(); }
        return localisedPT;
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }

        string value = key;

        switch (language)
        {
            case Language.Portugues:
                localisedPT.TryGetValue(key, out value);
                break;
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
        }

        return value;
    }

    public static void add(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.loadCSV();
        csvLoader.add(key, value);
        csvLoader.loadCSV();

        updateDictionaries();
    }

    public static void replace(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.loadCSV();
        csvLoader.edit(key, value);
        csvLoader.loadCSV();

        updateDictionaries();
    }

    public static void remove(string key)
    {
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.loadCSV();
        csvLoader.remove(key);
        csvLoader.loadCSV();

        updateDictionaries();
    }
}