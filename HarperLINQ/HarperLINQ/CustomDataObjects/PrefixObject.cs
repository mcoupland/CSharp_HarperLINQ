using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class Prefix
    {
        public static List<Prefix> GetPrefixes()
        {
            List<Prefix> prefixes = new List<Prefix>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                prefixes = (from a in context.Prefixes
                            where a.displayname != null
                            select a).ToList<Prefix>();
            }
            return prefixes;
        }
        public static List<Prefix> GetSFGPrefixes()
        {
            List<Prefix> prefixes = new List<Prefix>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                prefixes = (from a in context.Prefixes
                            where a.displayname != null
                            && a.sfgcode != null
                            select a).ToList<Prefix>();
            }
            return prefixes;
        }
        public static Prefix GetPrefixBySFGCode(string sfgcode)
        {
            Prefix prefix = new Prefix();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                prefix = (from a in context.Prefixes
                          where a.sfgcode == sfgcode
                          select a).SingleOrDefault();
            }
            return prefix;
        }
    }
}
