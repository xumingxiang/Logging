namespace Logging.Client
{
    /// <summary>
    /// 日志开关
    /// </summary>
    public class LogOnOff
    {
        public int AppId { get; set; }

        public byte Debug { get; set; }

        public byte Info { get; set; }

        public byte Warm { get; set; }

        public byte Error { get; set; }
    }
}