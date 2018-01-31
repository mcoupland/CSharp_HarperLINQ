using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class SFG_ProdCode
    {
        public static SFG_ProdCode GetFromExtCode(string extcode)
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                return (from a in context.SFG_ProdCodes where a.ExtCode == extcode select a).Single();
            }
        }
        public static SFG_ProdCode GetFromIntCode(string intcode)
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                return (from a in context.SFG_ProdCodes where a.IntCode == intcode select a).Single();
            }
        }
    }
}
