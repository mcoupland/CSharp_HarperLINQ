using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using HarperCRYPTO;

namespace HarperLINQ
{
    public partial class Referral
    {
        #region properties
        public tbl_AppEventLog LogEntry = new tbl_AppEventLog();
        public string membername;
        #endregion

        #region constructors
        public Referral(int id_in)
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    Referral referral = (from a in context.Referrals
                                         where a.id == id_in
                                         select a).SingleOrDefault();
                    if (referral != null)
                    {
                        SetData(referral);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntry = new tbl_AppEventLog(string.Empty, "BusinessLogic", "DataLoadException",
                    "Error loading tbl_Referral object", string.Format("id_in is '{0}'", id_in),
                    ex.Message, string.Empty, "Constructor: tbl_Referral(int id_in) ");
                LogEntry.Save();
                throw new DataLoadException(ex.Message);
            }
        }
        public Referral(string email_in)
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    Referral referral = (from a in context.Referrals
                                         where a.friendemail == email_in
                                         select a).SingleOrDefault();
                    if (referral != null)
                    {
                        SetData(referral);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntry = new tbl_AppEventLog(string.Empty, "BusinessLogic", "DataLoadException",
                    "Error loading tbl_Referral object", string.Format("email_in is '{0}'", email_in),
                    ex.Message, string.Empty, "Constructor: tbl_Referral(string email_in) ");
                LogEntry.Save();
                throw new DataLoadException(ex.Message);
            }
        }

        public static List<Referral> GetNeedsReminderList()
        {
            List<Referral> referrals = new List<Referral>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                referrals = (from a in context.Referrals
                             where a.friendid == null
                             && a.reminderemailid == null
                             && a.dateredeemed == null
                             && a.testrecord == false
                             && a.datecreated <= DateTime.Now.AddDays(-10) //this is wrong... seems it will email same folks over and over
                             && a.dateexpires >= DateTime.Now.AddDays(7)
                             orderby a.id
                             select a).ToList<Referral>();
            }
            return referrals;
        }

        public static List<Referral> GetNonRedeemedList()
        {
            List<Referral> referrals = new List<Referral>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                referrals = (from a in context.Referrals
                             where a.friendid == null
                             && a.testrecord == false
                             orderby a.id
                             select a).ToList<Referral>();
            }
            return referrals;
        }

        //public Referral(int memberid_in, string friendname_in, string friendemail_in, 
        //    string keycode_in, string pubcode_in, bool ccmember_in, string messagefrommember_in)
        public Referral(int cusid_in, string membername_in, string memberemail_in, string keycode_in,
                string pubcode_in, string friendname_in, string friendemailaddress_in,
                bool ccmember_in, int triallengthinmonths_in, int offerexpiresmonths)
        {
            try
            {
                this.membername = membername_in;
                this.datecreated = DateTime.Now;
                this.memberid = cusid_in;
                this.ccmember = ccmember_in;
                this.friendname = friendname_in;
                this.friendemail = friendemailaddress_in;
                this.keycode = keycode_in;
                this.pubcode = pubcode_in;
                this.dateexpires = DateTime.Now.AddMonths(offerexpiresmonths);//This is the date the offer expires not the date the subscription expires
                this.subscriptionlength = triallengthinmonths_in;
            }
            catch (Exception ex)
            {
                LogEntry = new tbl_AppEventLog(string.Empty, "BusinessLogic", "ObjectInitException",
                    "Error loading Referral object", string.Format("donorid_in is '{0}'", cusid_in),
                    ex.Message, string.Empty, "Constructor: Referral(string cusid, string membername, string memberemail, string keycode, string pubcode, string friendname, string friendemailaddress, bool ccmember, string personalmessage, int triallengthinmonths) ");
                LogEntry.Save();
                throw new ObjectInitException(ex.Message);
            }
        }
        #endregion

        public string GetReferralUserCreatedEmailBody()
        {
            string sfg_number = string.Empty;
            if (this.friendid == null
                || this.friendid <= 0
                || string.IsNullOrEmpty(this.friendemail))
            {
                throw new Exception(string.Format("Cannot get \"Referral User Created\" email, data is invalid for referral id {0}.", this.id));
            }

            #region get sfg number
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                try
                {
                    sfg_number = (from a in context.SFG_CustomerNumbers
                              where a.cusID == this.friendid
                              select a.SFGCustNum).Single();
                }
                catch
                {
                    throw new Exception(string.Format("Cannot get \"Referral User Created\" email, no SFG number available for referral id {0}.", this.id));
                }
            }
            #endregion

            #region email html
            string body = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN""
    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<base target=""_blank"" />
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>Welcome to the Andrew Harper Community!</title>
</head>
<body style=""font-family: Verdana,Arial,Helvetica,sans-serif; line-height: 1.5; background-color:#EEEEEE"" >
<table cellspacing=""0"" border=""0"" align=""center"" cellpadding=""0"" width=""630"">
  <tr>
    <td align=""center"" style=""color:#000000; font-family:Verdana, sans-serif; font-size: 7pt; line-height: 150%; padding-bottom:5px;""><strong>Andrew Harper Invitation</strong><br />
      Please add <a href=""mailto:membership@andrewharper.com"" style=""text-decoration: none; color: #36c;"">membership@andrewharper.com</a> to your address book to ensure that our emails reach your inbox. </td>
  </tr>
</table>
<table width=""630"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""border: 3px solid #EEEEEE;"" bgcolor=""#ffffff"">
  <tr>
    <td><table cellspacing=""0"" cellpadding=""0"" width=""100%"">
        <tr valign=""top"" align=""center"">
          <td style=""padding-top:10px;""><a href=""http://www.andrewharper.com/""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/ah_logo_redeem.jpg""
                            alt=""Membership Invitation from Your Friend"" style=""border:none;""/></a></td>
        </tr>
      </table></td>
  </tr>
  <tr align=""center"">
    <td valign=""top"" style=""padding-bottom: 25px;""><img alt=""Log in now and start traveling!"" src=""http://www.andrewharper.com/ImageStore/Images/referral/RedemptionEmail_header.jpg""
                            border=""0""  /></td>
  </tr>
  <tr>
    <td><table style=""font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 12pt; line-height: 1.5;"">
        <tr>
          <td width=""30px""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/clear.gif"" />			        </td>
          <td valign=""top""><p>Dear [friendname],</p>
            <p>Welcome to the Andrew Harper Community! <b>Your membership number is [friendmemberid].</b></p>
            <p> We look forward to providing you with access to Andrew Harper's legendary travel
              advice month after month. To take advantage of everything your membership had to
              offer, be sure to log in at <a href=""https://www.andrewharper.com/user"" style=""text-decoration:underline; color: #3366CC;"">AndrewHarper.com/user</a> to: </p>
            
            <ul style=""margin-top: 10px; margin-bottom: 10px; list-style-type: disc;"">
              <li><a href=""http://www.andrewharper.com/advanced-search"" style=""text-decoration:underline; color: #3366CC;"">Search</a> Mr. Harper's reviews of more than 1,000 properties worldwide</li>
              <li><a href=""http://www.andrewharper.com/hideaway-report-archive"" style=""text-decoration:underline; color: #3366CC;"">Explore</a> the expanded, digital
                version of the <i>Hideaway Report</i></li>
              <li>Book hotels online at your convenience</li>
              <li><a href=""http://www.andrewharper.com/specialoffers"" style=""text-decoration:underline; color: #3366CC;"">Find</a> exclusive members-only special
                offers at incredible properties</li>
              <li ><a href=""mailto:reservations@andrewharper.com"" style=""text-decoration:underline; color: #3366CC;"">Contact</a> our in-house travel agency
                for customized itineraries and complimentary assistance in planning your next trip</li>
            </ul>
            <p>Thank you again for your order. We look forward to bringing you the world!</p>
            <hr />
            <p style=""color: #e34609; margin: 0px; padding: 0px; font-weight: bold;"">Questions?</p>
            <b>View our <a href=""http://www.andrewharper.com/faq"" style=""text-decoration:underline; color: #3366CC;"">Frequently Asked Questions</a> or<br />
            contact Andrew Harper Member Services.</b><br />
            <div>
              <div style=""float: left; width: 100px; text-transform: uppercase;""> Email </div>
              <a href=""mailto:Membership@AndrewHarper.com"" style=""text-decoration:underline; color: #3366CC;"">Membership@AndrewHarper.com</a><br />
              <div style=""float: left; width: 100px; text-transform: uppercase;""> Phone </div>
              (866) 831-4314 USA, +1 (512) 904-7342 International <br />
              <div style=""float: left; width: 100px; text-transform: uppercase;""> Hours </div>
              Monday-Friday, 8 a.m.-6 p.m. (CST) </div></td>
          <td width=""30px""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/clear.gif"" />			        </td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 0px 0px; color:#000000; font-family: Verdana, sans-serif;""><img src=""http://www.andrewharper.com/ImageStore/Images/enews/2011/module-divider-up.png"" width=""600"" height=""70"" alt="""" class=""block"" /></td>
  </tr>
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 15px 0px; color:#000000; font-family: Verdana, sans-serif;""><a href=""http://www.andrewharper.com""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/harper_icon.gif"" alt=""Andrew Harper"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://www.facebook.com/pages/Andrew-Harper/70674486854?sid=7bb23ae8292668ee9c3ba769e85d4c4c&amp;ref=search"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_facebook.gif"" alt=""Andrew Harper Facebook"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://twitter.com/HarperTravel"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_twitter.gif"" alt=""Andrew Harper Twitter"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://www.thingsyoushouldknowblog.com/?feed=rss2"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_rss_blog.gif"" alt=""view the blog"" width=""35"" height=""34"" border=""0"" style=""border:none"" /></a>&nbsp;</td>
  </tr>
</table>
<table cellspacing=""0"" border=""0"" align=""center"" style=""padding-botom:0px"" cellpadding=""0"" width=""630"">
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 28px 0px; color:#000000; font-family: Verdana, sans-serif;"">&nbsp;<br />
      &copy; Andrew Harper LLC. | (800) 375-4685 or (630) 734-4610 | 1601 Rio Grande, Suite 410, Austin, TX 78701 <br/>
      View our <a href=""http://www.andrewharper.com/privacy-policy"" target=""_blank"" style=""color: #36c;"">Privacy Statement</a> | <a href=""http://www.andrewharper.com/contact-us"" target=""_blank"" style=""color: #36c;"">Contact Us</a> | <a href=""http://www.andrewharper.com/terms-conditions"" target=""_blank"" style=""color: #36c;"">Terms &amp; Conditions</a></td>
  </tr>
</table>
</body>
</html>
";
            #endregion

            return body.Replace("[friendname]", this.friendname)
                .Replace("[friendmemberid]", sfg_number);
        }

        public string GetReferralEmailBody()
        {
            string link = string.Format("{0}/Referral/Redeem.aspx?ReferralId={1}", 
                new object[]{ 
                    ConfigurationManager.AppSettings["server"], 
                    System.Web.HttpUtility.UrlEncode(HarperCRYPTO.Cryptography.EncryptData(this.id.ToString()))
                });

            #region email html
            string body = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN""
    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<base target=""_blank"" />
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>You Have Been Invited!</title>
</head>
<body style=""font-family: Verdana,Arial,Helvetica,sans-serif; line-height: 1.5; background-color:#EEEEEE "" >
<table cellspacing=""0"" border=""0"" align=""center"" cellpadding=""0"" width=""630"">
  <tr>
    <td align=""center"" style=""color:#000000; font-family:Verdana, sans-serif; font-size: 7pt; line-height: 150%; padding-bottom:5px;""><strong>Andrew Harper Invitation</strong><br />
      Please add <a href=""mailto:membership@andrewharper.com"" style=""text-decoration: none; color: #36c;"">membership@andrewharper.com</a> to your address book to ensure that our emails reach your inbox. </td>
  </tr>
</table>
<table bgcolor=""#ffffff"" width=""630"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""border: 3px solid #EEEEEE;"">
  <tr valign=""top"" align=""center"">
    <td style=""padding-top:10px; padding-bottom:10px;""><a href=""http://www.andrewharper.com/""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/ah_logo_referred.jpg"" alt=""Membership Invitation from [membername]"" style=""border:none;""/></a></td>
  </tr>
  <tr valign=""top"" align=""center"">
    <td style=""padding-bottom: 25px;""><img alt=""Welcome to Andrew Harper"" src=""http://www.andrewharper.com/ImageStore/Images/referral/ReferredEmail_header.jpg""
                            border=""0""  /></a></td>
  </tr>
  <tr>
    <td style=""padding:0px;""><table cellpadding=""0"" style=""font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 12pt; line-height: 1.5;"">
        <tr>
          <td width=""30px""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/clear.gif"" /> </td>
          <td valign=""top""><table>
              <tr>
                <td style=""font-family: Georgia; font-size: 1.2em; line-height: 1; color: #000000; font-weight: normal; line-height: normal;""><p><em>You have been invited to join Andrew Harper’s elite travel service by [membername].</em></p>
                  <p><span style=""font-size:17pt;"">We invite you to begin your journey with a complimentary six month Premier Online membership.</span></p>
                  <a href=""[link]""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/activate-button.gif"" alt=""Click to Activate"" border=""0"" /></a><br/></td>
              </tr>
              <tr>
                <td style=""font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 12pt; line-height: 1.5;"">
                <p>Dear [friendname],</p>
                  <p><b>[membername]</b> and <b>Andrew Harper</b> would like to extend you a <b>complimentary,
                    six month Premier Online Membership.</b></p>
                  <p> Traveling incognito and paying full rate, Andrew Harper circles the globe in search
                    of those qualities that separate the truly enchanting from the merely excellent.
                    To learn more about Andrew Harper, <a href=""http://www.andrewharper.com"" style=""text-decoration:underline; color: #3366CC;"">click here</a>.</p>
                  <p> Enjoy access to all negotiated member benefits at more than 500 Harper-recommended
                    hotels and resorts worldwide! Members receive an average of $600* in benefits from
                    one stay. Member benefits may include:</p>
                  <ul style=""margin-top: 10px; margin-bottom: 10px; list-style-type: disc;"">
                    <li><b>Hotel or resorts credits</b> (such as dining,
                      spa, activities)</li>
                    <li><b>Room upgrades</b> (when available)</li>
                    <li><b>Breakfasts for two, daily</b></li>
                    <li><b>Preferred rates</b></li>
                  </ul>
                  <p> <i>*Based on double occupancy and stays of two nights or longer. Exact benefits and
                    value vary by property.</i></p>
                  <p> <a href=""[link]"" style=""text-decoration:underline; color: #3366CC;"">Click here</a> to activate your complimentary membership and start enjoying
                    everything Andrew Harper membership has to offer!</p>
                  <hr />
                  <p style=""color: #e34609; margin: 0px; padding: 0px; font-weight: bold;"">Questions?</p>
                  <b>View our <a href=""http://www.andrewharper.com/faq"" style=""text-decoration:underline; color: #3366CC;"">Frequently Asked Questions</a> or<br />
                  contact Andrew Harper Member Services.</b><br />
                  <div>
                    <div style=""float: left; width: 100px; text-transform: uppercase;""> Email </div>
                    <a href=""mailto:Membership@AndrewHarper.com"" style=""text-decoration:underline; color: #3366CC;"">Membership@AndrewHarper.com</a><br />
                    <div style=""float: left; width: 100px; text-transform: uppercase;""> Phone </div>
                    (866) 831-4314 USA,+1(512)904-7342 International <br />
                    <div style=""float: left; width: 100px; text-transform: uppercase;""> Hours </div>
                    Monday-Friday, 8 a.m.-6 p.m. (CST) </div></td>
              </tr>
            </table></td>
          <td width=""30px""><img src=""http://www.andrewharper.com/ImageStore/Images/referral/clear.gif"" /></td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 0px 0px; color:#000000; font-family: Verdana, sans-serif;""><img src=""http://www.andrewharper.com/ImageStore/Images/enews/2011/module-divider-up.png"" width=""600"" height=""70"" alt="""" class=""block"" /></td>
  </tr>
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 15px 0px; color:#000000; font-family: Verdana, sans-serif;""><a href=""http://www.andrewharper.com""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/harper_icon.gif"" alt=""Andrew Harper"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://www.facebook.com/pages/Andrew-Harper/70674486854?sid=7bb23ae8292668ee9c3ba769e85d4c4c&amp;ref=search"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_facebook.gif"" alt=""Andrew Harper Facebook"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://twitter.com/HarperTravel"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_twitter.gif"" alt=""Andrew Harper Twitter"" width=""35"" height=""34"" border=""0"" style=""border:medium none"" /></a>&nbsp; <a href=""http://www.thingsyoushouldknowblog.com/?feed=rss2"" target=""_blank""><img src=""http://www.andrewharper.com/ImageStore/Images/icons/icon_rss_blog.gif"" alt=""view the blog"" width=""35"" height=""34"" border=""0"" style=""border:none"" /></a>&nbsp;</td>
  </tr>
</table>
<table cellspacing=""0"" border=""0"" align=""center"" style=""padding-botom:0px"" cellpadding=""0"" width=""630"">
  <tr>
    <td align=""center"" style=""font-size: 7pt; line-height: 150%; padding: 0px 0px 28px 0px; color:#000000; font-family: Verdana, sans-serif;"">&nbsp;<br />
      &copy; Andrew Harper LLC. | (800) 375-4685 or (630) 734-4610 | 1601 Rio Grande, Suite 410, Austin, TX 78701 <br/>
      View our <a href=""http://www.andrewharper.com/privacy-policy"" target=""_blank"" style=""color: #36c;"">Privacy Statement</a> | <a href=""http://www.andrewharper.com/contact-us"" target=""_blank"" style=""color: #36c;"">Contact Us</a> | <a href=""http://www.andrewharper.com/terms-conditions"" target=""_blank"" style=""color: #36c;"">Terms &amp; Conditions</a></td>
  </tr>
</table>
</body>
</html>
";
            #endregion

            return body.Replace("[membername]", this.membername)
                .Replace("[friendname]", this.friendname)
                .Replace("[link]", link);
        }
        
        public void Save()
        {
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    if (this.id > 0)
                    {
                        Referral referral = (from a in context.Referrals
                                                 where a.id == this.id
                                                 select a).SingleOrDefault();
                        GetData(referral);
                    }
                    else
                    {
                        context.Referrals.InsertOnSubmit(this);
                    }
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ObjectSaveException(ex.Message);
            }
        }

        private void SetData(Referral referral)
        {
            this.id = referral.id;
            this.datecreated = referral.datecreated;
            this.memberid = referral.memberid;
            this.friendname = referral.friendname;
            this.friendemail = referral.friendemail;
            this.keycode = referral.keycode;
            this.pubcode = referral.pubcode;
            this.dateredeemed = referral.dateredeemed;
            this.friendid = referral.friendid;
            this.ccmember = referral.ccmember;
            this.messagefrommember = referral.messagefrommember;
            this.subscriptionlength = referral.subscriptionlength;
            this._dateexpires = referral._dateexpires;
            this.reminderemailid = referral.reminderemailid;
            this.testrecord = referral.testrecord;
        }

        private void GetData(Referral referral)
        {
            referral.id = this.id;
            referral.datecreated = this.datecreated;
            referral.memberid = this.memberid;
            referral.ccmember = this.ccmember;
            referral.messagefrommember = this.messagefrommember;
            referral.friendname = this.friendname;
            referral.friendemail = this.friendemail;
            referral.keycode = this.keycode;
            referral.pubcode = this.pubcode;
            referral.dateredeemed = this.dateredeemed;
            referral.friendid = this.friendid;
            referral.dateexpires = this.dateexpires;
            referral.subscriptionlength = this.subscriptionlength;
            referral.testrecord = this.testrecord;
            referral.reminderemailid = this.reminderemailid;
        }
    }
}
