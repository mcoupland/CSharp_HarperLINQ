using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public partial class tbl_DrupalCartRequest
    {
        public void Save()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                tbl_DrupalCartRequest cart = (from a in context.tbl_DrupalCartRequests
                                                   where a.id == this.id
                                                   select a).SingleOrDefault();
                if (cart == null)
                {
                    cart = new tbl_DrupalCartRequest();
                    context.tbl_DrupalCartRequests.InsertOnSubmit(cart);
                }
                cart.username = this.username;
                cart.salutation = this.salutation;
                cart.firstname = this.firstname;
                cart.middleinitial = this.middleinitial;
                cart.lastname = this.lastname;
                cart.suffix = this.suffix;
                cart.protitle = this.protitle;
                cart.email = this.email;
                cart.optin = this.optin;
                cart.business = this.business;
                cart.add1 = this.add1;
                cart.add2 = this.add2;
                cart.add3 = this.add3;
                cart.city = this.city;
                cart.state = this.state;
                cart.zip = this.zip;
                cart.country = this.country;
                cart.phone = this.phone;
                cart.fax = this.fax;
                cart.altcity = this.altcity;
                cart.ccnum = this.ccnum;
                cart.ccmonth = this.ccmonth;
                cart.ccyear = this.ccyear;
                cart.amtpaid = this.amtpaid;
                cart.ccname = this.ccname;
                cart.ccadd = this.ccadd;
                cart.cccity = this.cccity;
                cart.ccstate = this.ccstate;
                cart.cczip = this.cczip;
                cart.pubcode = this.pubcode;
                cart.keycode = this.keycode;
                cart.sublen = this.sublen;
                cart.source = this.source;
                cart.customertype = this.customertype;
                cart.expiredate = this.expiredate;
                cart.startdate = this.startdate;
                cart.newsletters = this.newsletters;
                cart.errors = this.errors;
                cart.responsecode = this.responsecode;
                cart.mobilephone = this.mobilephone;
                cart.secondemail = this.secondemail;
                cart.methodofpayment = this.methodofpayment;
                cart.cccountry = this.cccountry;
                cart.ccaddress2 = this.ccaddress2;
                cart.screenname = this.screenname;
                cart.newmemberid = this.newmemberid;
                context.SubmitChanges();
            }
        }
    }
}
