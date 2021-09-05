namespace Mkb.RavenDbRepo.Configs
{
    public class RavenDbConfig
    {
        public RavenDbConfig(string[] urls, string dataBase)
        {
            DataBase = dataBase;
            Urls = urls;
        }

        public string[] Urls { get; }
        public string DataBase { get; }
    }
}