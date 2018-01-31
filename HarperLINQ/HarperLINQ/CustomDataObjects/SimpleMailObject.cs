using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace HarperLINQ
{
    public partial class SimpleMail
    {

        public bool isHtml;
        public string smtpAddress;

        public SimpleMail(string subject, string fromaddress, string toaddress, string cc, string bcc, string body, bool ishtml, string smtp)
        {
            this.subject = subject;
            this.fromemail = fromaddress;
            this.toemail = toaddress;
            this.body = body;
            this.ccemail = cc;
            this.bccemail = bcc;
            this.datecreated = DateTime.Now;
            this.ishtml = ishtml;
            this.isHtml = ishtml;
            this.smtpAddress = smtp; 
        }
        public void Save()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                context.SimpleMails.InsertOnSubmit(this);
                context.SubmitChanges();
            }
        }
    }
}
