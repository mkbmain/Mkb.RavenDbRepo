namespace Mkb.RavenDbRepo
{
    public class RavenConfig
    {
        public RavenConfig(string[] urls, string dataBase)
        {
            DataBase = dataBase;
            Urls = urls;
        }

        public string[] Urls { get; set; }
        public string DataBase { get; set; }
    }
}