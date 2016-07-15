using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Alerting
{
    public abstract class BaseAlerting
    {
        public BaseAlerting(AlertingType alertingType)
        {
            this.AlertingType = alertingType;
        }

        protected abstract void Alert();


        protected AlertingType AlertingType { get; set; }
    }
}
