using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class Country
    {
        #region properties
        public tbl_AppEventLog LogEntry = new tbl_AppEventLog();
        #endregion

        public static List<Country> GetCountries()
        {
            List<Country> response = new List<Country>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                var countries = (from a in context.Countries
                                 select a);
                foreach (var country in countries)
                {
                    response.Add(country);
                }
            }
            return response;
        }
    }
}
