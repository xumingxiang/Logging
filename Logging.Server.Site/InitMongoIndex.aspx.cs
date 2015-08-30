using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Logging.Server.Site
{
    public partial class InitMongoIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] a = new int[] { 1, 2, 3, 4 };
            int[] b = new int[] { 4, 3, 2, 1 };
            int[] c = new int[] { 4, 1, 2, 3 };
            Array.Sort(a);
            Array.Sort(b);
            Array.Sort(c);
            bool aa = string.Join(",", a) == "1,2,3,4";
            bool bb = b == c;
        }
    }
}