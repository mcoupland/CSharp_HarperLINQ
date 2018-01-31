using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class tbl_WebEventLog
    {
        #region properties
        #endregion

        #region constructors
        public tbl_WebEventLog(string user, string event_name, string description)
        {
            try
            {
                this.welUserName = user;
                this.welDateCreated = DateTime.Now;
                this.welEvent = event_name;
                this.welEventDesc = description;
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
                    context.tbl_WebEventLogs.InsertOnSubmit(this);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ObjectSaveException(ex.Message);
            }
        }
    }
}
