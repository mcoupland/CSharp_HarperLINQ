using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class ReferralOffer
    {
        public tbl_AppEventLog LogEntry = new tbl_AppEventLog();

        public ReferralOffer(string keycode_in, string pubcode_in)
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    ReferralOffer offer = (from a in context.ReferralOffers
                                         where a.keycode == keycode_in
                                         && a.pubcode == pubcode_in
                                         && a.isactive == true
                                         select a).SingleOrDefault();
                    if (offer != null)
                    {
                        SetData(offer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntry = new tbl_AppEventLog(string.Empty, 
                    "BusinessLogic", 
                    "DataLoadException",
                    "Error loading tbl_ReferralOffer object", 
                    string.Format("keycode_in is '{0}', pubcode_in is '{1}'", new object[] {keycode_in, pubcode_in}),
                    ex.Message, 
                    string.Empty, 
                    "Constructor: tbl_ReferralOffer(string keycode_in, string pubcode_in) ");
                LogEntry.Save();
                throw new DataLoadException(ex.Message);
            }
        }
        
        private void SetData(ReferralOffer offer)
        {
            this.id = offer.id;
            this.isactive = offer.isactive;
            this.keycode = offer.keycode;
            this.pubcode = offer.pubcode;
            this.offerexpiresmonths = offer.offerexpiresmonths;
            this.triallengthinmonths = offer.triallengthinmonths;
            this.reminderemailbcc = offer.reminderemailbcc;
            this.reminderemailcopy = offer.reminderemailcopy;
            this.reminderemailfromaddress = offer.reminderemailfromaddress;
            this.reminderemailishtml = offer.reminderemailishtml;
            this.reminderemailsmtp = offer.reminderemailsmtp;
            this.reminderemailsubject = offer.reminderemailsubject;
        }

        public void Save()
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    context.ReferralOffers.InsertOnSubmit(this);
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
