using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.NewsRepository.Settings
{
    public class SettingsModel
    {
        [YamlProperty("NewsRepository.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("NewsRepository.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("NewsRepository.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("NewsRepository.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }

        [YamlProperty("NewsRepository.CleanAndKeepLastRecordsCount")]
        public int CleanAndKeepLastRecordsCount { get; set; }
    }
}
