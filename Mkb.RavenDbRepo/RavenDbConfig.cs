namespace Mkb.RavenDbRepo
{
    public class RavenDbConfig
    {
        public RavenDbConfig(string[] urls, string dataBase)
        {
            DataBase = dataBase;
            Urls = urls;
        }

        public string[] Urls { get; set; }
        public string DataBase { get; set; }
    }
}