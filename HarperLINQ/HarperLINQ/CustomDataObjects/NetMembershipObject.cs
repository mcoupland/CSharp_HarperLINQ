using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    [System.Xml.Serialization.XmlInclude(typeof(tbl_NetMembership))]
    public partial class tbl_NetMembership
    {
        public string Name;
        public string PublicationCode;

        public void Save()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                tbl_NetMembership netmembership = (from a in context.tbl_NetMemberships
                                                   where a.cusID == this.cusID
                                                   && a.mtyCode == this.mtyCode
                                                   select a).SingleOrDefault();
                if (netmembership == null)
                {
                    netmembership = new tbl_NetMembership();
                    context.tbl_NetMemberships.InsertOnSubmit(netmembership);
                }
                netmembership.cusID = this.cusID;
                netmembership.mtyCode = this.mtyCode;
                netmembership.nmbDateCreated = this.nmbDateCreated;
                netmembership.nmbDateEnd = this.nmbDateEnd;
                netmembership.nmbDateStart = this.nmbDateStart;
                netmembership.nmbRenewalCode = this.nmbRenewalCode;
                context.SubmitChanges();
            }
        }

        public static List<tbl_NetMembership> GetNetMemberships(int cusid)
        {
            List<tbl_NetMembership> subs = new List<tbl_NetMembership>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                subs = (from a in context.tbl_NetMemberships
                        where a.cusID == cusid
                        orderby a.nmbDateStart descending
                        select a).ToList();
            }
            return subs;
        }

        public static tbl_NetMembership GetCurrentNetMembership(int cusid)
        {
            DateTime TodayPlusGracePeriod = DateTime.Now.AddMonths(-1);
            List<tbl_NetMembership> subs = tbl_NetMembership.GetNetMemberships(cusid).Where(x => x.nmbDateEnd >= TodayPlusGracePeriod).OrderByDescending(x => x.nmbDateEnd).ToList<tbl_NetMembership>();
            if(subs.Count <= 0)
            {
                return null;
            }
            subs[0].AddProperties();
            return subs[0];
        }

        public static tbl_NetMembership GetLatestNetMembership(int cusid)
        {
            List<tbl_NetMembership> subs = tbl_NetMembership.GetNetMemberships(cusid).OrderByDescending(x => x.nmbDateEnd).ToList<tbl_NetMembership>();
            if (subs.Count <= 0)
            {
                return null;
            }
            subs[0].AddProperties();
            return subs[0];
        }

        public void AddProperties()
        {
            tbl_MembershipType membershiptype = new tbl_MembershipType(this.mtyCode);
            this.Name = membershiptype.mtyName;
            SFG_ProdCode code = SFG_ProdCode.GetFromIntCode(this.mtyCode);
            this.PublicationCode = code.ExtCode;
        }

    }
}
