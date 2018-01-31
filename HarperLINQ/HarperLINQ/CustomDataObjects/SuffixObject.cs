using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class Suffix
    {
        public static List<Suffix> GetSuffixes() 
        {
            List<Suffix> suffixes = new List<Suffix>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                suffixes = (from a in context.Suffixes
                            where a.displayname != null
                            select a).ToList<Suffix>();
            }
            return suffixes;
        }
        public static List<Suffix> GetSFGSuffixes() 
        {
            List<Suffix> suffixes = new List<Suffix>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                suffixes = (from a in context.Suffixes
                            where a.sfgcode != null
                            && a.displayname != null
                            select a).ToList<Suffix>();
            }
            return suffixes;
        }

        public static Suffix GetSuffixBySFGCode(string sfgcode)
        {
            Suffix suffix = new Suffix();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                suffix = (from a in context.Suffixes
                            where a.sfgcode == sfgcode
                            select a).SingleOrDefault();
            }
            return suffix;
        }
    }
}
