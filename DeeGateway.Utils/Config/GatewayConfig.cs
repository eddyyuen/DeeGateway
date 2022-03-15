using BeetleX.EventArgs;

namespace DeeGateway.Utils.Config
{
    public class GatewayConfig
    {
        public Httpconfig HttpConfig { get; set; }
        public Database DataBase { get; set; }

        public string  Jwt_Token { get; set; }
    }

    public class Httpconfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public int SSLPort { get; set; }
        public string CertificateFile { get; set; }
        public string CertificatePassword { get; set; }
        public int MaxBodyLength { get; set; }
        public bool OutputStackTrace { get; set; }
        public string StaticResurceType { get; set; }
        public string DefaultPage { get; set; }
        public string NoGzipFiles { get; set; }
        public string CacheFiles { get; set; }
        public int BufferSize { get; set; }
        public int WebSocketMaxRPS { get; set; }
        public int CacheLogMaxSize { get; set; }
        public int BufferPoolMaxMemory { get; set; }
        public int MaxConnections { get; set; }
        public bool LogToConsole { get; set; }
        public bool WriteLog { get; set; }
        public LogType LogLevel { get; set; }
        public bool UrlIgnoreCase { get; set; }
        public string Manager { get; set; }
        public string ManagerPWD { get; set; }      
        public bool UseIPv6 { get; set; }
        public int PacketCombined { get; set; }
        public int SessionTimeOut { get; set; }

    }

    public class Database
    {
        public string DBType { get; set; }
        public string ConnectionString { get; set; }
        public int Interval { get; set; }
        public string Cache { get; set; }
    }
}
