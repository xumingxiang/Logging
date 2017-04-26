namespace Logging.Server.Alerting
{
    public abstract class BaseAlerting
    {
        public BaseAlerting(AlertingType alertingType)
        {
            this.AlertingType = alertingType;
        }

        public abstract void Alert();

        protected AlertingType AlertingType { get; set; }
    }
}