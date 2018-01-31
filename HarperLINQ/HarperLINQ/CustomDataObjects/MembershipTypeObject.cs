using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    [System.Xml.Serialization.XmlInclude(typeof(tbl_MembershipType))]
    public partial class tbl_MembershipType
    {
        public tbl_MembershipType(string mtyCode)
        {
            tbl_MembershipType membershiptype = new tbl_MembershipType();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                membershiptype = (from a in context.tbl_MembershipTypes
                        where a.mtyCode == mtyCode
                        select a).SingleOrDefault();
            }
            if (membershiptype != null)
            {
                this.mtyCode = membershiptype.mtyCode;
                this.mtyGUID = membershiptype.mtyGUID;
                this.mtyName = membershiptype.mtyName;
            }
        }
    }
}
