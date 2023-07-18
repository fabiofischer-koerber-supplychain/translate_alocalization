
using System.Xml;
using Google.Cloud.Translation.V2;

class Program
{
    static Program() {}


    static void Main(string[] args)
    {

        var client = TranslationClient.Create();
        
        string[] files = Directory.GetFiles(".\\translation_data", "*.alocalization");

        foreach(string file in files)
        {

            string translatedFilePath = $".\\translation_data\\translated\\{Path.GetFileName(file)}";

            Console.WriteLine(file);
            Console.WriteLine(translatedFilePath);

            string xmlFile = File.ReadAllText(file);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlFile);

            foreach(XmlNode data in xmldoc.GetElementsByTagName("LanguageId"))
            {
                data.InnerText = "pt-BR";
            }

            foreach(XmlNode data in xmldoc.GetElementsByTagName("LocalizedValue"))
            {
                var translation = client.TranslateText(data.InnerText, LanguageCodes.Portuguese, LanguageCodes.English);
                data.InnerText = translation.TranslatedText;
            }

            File.WriteAllText(translatedFilePath, xmldoc.InnerXml);

        }
    }
}
