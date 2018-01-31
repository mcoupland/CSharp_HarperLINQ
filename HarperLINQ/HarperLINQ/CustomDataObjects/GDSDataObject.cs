using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class GDSData
    {
        #region properties
        public tbl_AppEventLog LogEntry = new tbl_AppEventLog();
        #endregion

        #region constructors
        public GDSData(int id_in)
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    GDSData gdsdata = (from a in context.GDSDatas
                                       where a.ID == id_in
                                       select a).SingleOrDefault();
                    SetData(gdsdata);
                }
            }
            catch (Exception ex)
            {
                LogEntry = new tbl_AppEventLog(string.Empty, "Sideline GDS Report", "DataLoadException",
                    "Error loading tbl_GDSData object", string.Format("id_in is '{0}'", id_in),
                    ex.Message, string.Empty, "Constructor: GDSData(int id_in)");
                LogEntry.Save();
                throw new DataLoadException(ex.Message);
            }
        }
        #endregion

        private void SetData(GDSData gdsdata)
        {
            this.BkiId = gdsdata.BkiId;
            this.IsInApollo = gdsdata.IsInApollo;
            this.CityCode = gdsdata.CityCode;
            this.PropertyNumber = gdsdata.PropertyNumber;
            this.ChainCode = gdsdata.ChainCode;
            this.Rates2011 = gdsdata.Rates2011;
            this.Rates2012 = gdsdata.Rates2012;
            this.Rates2013 = gdsdata.Rates2013;
        }
        public void Save()
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    context.GDSDatas.InsertOnSubmit(this);
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
