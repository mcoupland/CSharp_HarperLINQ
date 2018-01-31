using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarperLINQ
{
    public partial class State
    {
        #region properties
        public tbl_AppEventLog LogEntry = new tbl_AppEventLog();
        #endregion

        public static List<string> GetRegions(string countryname)
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                List<string> response = new List<string>();
                if (countryname.ToUpper() == "U.S.A.")
                {
                    var states = (from a in context.States
                                  where a.staIsActive == true
                                  select a.staName);
                    foreach (var state in states)
                    {
                        response.Add(state);
                    }
                }
                else
                {
                    var regions = (from a in context.Nodes
                                   where a.nodName == countryname
                                   join b in context.Relations
                                   on a.nodID equals b.nodParentID
                                   join c in context.Nodes
                                   on b.nodChildID equals c.nodID
                                   select c.nodName);
                    foreach (var region in regions)
                    {
                        response.Add(region);
                    }
                }
                return response;
            }
        }
    }
}
