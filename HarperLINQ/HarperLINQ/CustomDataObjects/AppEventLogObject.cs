using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class tbl_AppEventLog
    {
        #region properties
        
        #endregion

        #region constructors
        public tbl_AppEventLog(string user, string app_name, string event_name, string message1, 
                        string message2, string message3, string severity, string section)
        {
            try
            {
                this.aelAppName = app_name;
                this.aelDateCreated = DateTime.Now;
                this.aelEvent = event_name;
                this.aelMessage1 = message1;
                this.aelMessage2 = message2;
                this.aelMessage3 = message3;
                this.aelSection = section;
                this.aelSeverity = severity;
                this.aelUserName = user;
            }
            catch(Exception ex)
            {
                throw new ObjectInitException(ex.Message);
            }
        }
        #endregion 
        
        public void Save()
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    context.tbl_AppEventLogs.InsertOnSubmit(this);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ObjectSaveException(ex.Message);
            }
        }
        public static List<string> GetAppNames() 
        {
            List<string> appnames = new List<string>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                appnames = (from a in context.tbl_AppEventLogs select a.aelAppName).Distinct<string>().ToList<string>();
            }
            appnames.Sort();
            return appnames;
        }
        public static List<string> GetSeverities()
        {
            List<string> severities = new List<string>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                severities = (from a in context.tbl_AppEventLogs select a.aelSeverity).Distinct<string>().ToList<string>();
            }
            severities.Sort();
            return severities;
        }
        public static List<string> GetEventNames()
        {
            List<string> eventnames = new List<string>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                eventnames = (from a in context.tbl_AppEventLogs select a.aelEvent).Distinct<string>().ToList<string>();
            }
            eventnames.Sort();
            return eventnames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="limit">If zero, defaults to 100</param>
        /// <returns></returns>
        public static List<tbl_AppEventLog> GetByUserName(string username, int limit)
        {
            limit = limit == 0 ? 100 : limit;
            List<tbl_AppEventLog> logs = new List<tbl_AppEventLog>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                logs = (from a in context.tbl_AppEventLogs where a.aelUserName == username select a).OrderByDescending(x => x.aelID).Take(limit).ToList<tbl_AppEventLog>();
            }
            return logs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appname"></param>
        /// <param name="limit">If zero, defaults to 100</param>
        /// <returns></returns>
        public static List<tbl_AppEventLog> GetByAppName(string appname, int limit)
        {
            limit = limit == 0 ? 100 : limit;
            List<tbl_AppEventLog> logs = new List<tbl_AppEventLog>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                logs = (from a in context.tbl_AppEventLogs where a.aelAppName == appname select a).OrderByDescending(x => x.aelID).Take(limit).ToList<tbl_AppEventLog>();
            }
            return logs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="limit">If zero, defaults to 100</param>
        /// <returns></returns>
        public static List<tbl_AppEventLog> GetBySeverity(string severity, int limit)
        {
            limit = limit == 0 ? 100 : limit;
            List<tbl_AppEventLog> logs = new List<tbl_AppEventLog>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                logs = (from a in context.tbl_AppEventLogs where a.aelSeverity == severity select a).OrderByDescending(x => x.aelID).Take(limit).ToList<tbl_AppEventLog>();
            }
            return logs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="limit">If zero, defaults to 100</param>
        /// <returns></returns>
        public static List<tbl_AppEventLog> GetByEventName(string eventname, int limit)
        {
            limit = limit == 0 ? 100 : limit;
            List<tbl_AppEventLog> logs = new List<tbl_AppEventLog>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                logs = (from a in context.tbl_AppEventLogs where a.aelEvent == eventname select a).OrderByDescending(x => x.aelID).Take(limit).ToList<tbl_AppEventLog>();
            }
            return logs;
        }
    }
}
