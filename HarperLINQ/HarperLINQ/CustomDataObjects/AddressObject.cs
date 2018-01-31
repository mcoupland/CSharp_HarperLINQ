using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    [System.Xml.Serialization.XmlInclude(typeof(tbl_AddressCustomer))]
    public partial class tbl_AddressCustomer
    {
        #region constructors
        public tbl_AddressCustomer(int id_in)
        {
            tbl_AddressCustomer address;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                address = (from a in context.tbl_AddressCustomers
                                               where a.addID == id_in
                                               select a).SingleOrDefault();
            }
            if (address != null)
            {
                SetData(address);
            }
        }
        #endregion

        public void Save()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                if (this.addID > 0)
                {
                    tbl_AddressCustomer address = (from a in context.tbl_AddressCustomers
                                             where a.addID == this.addID
                                             select a).SingleOrDefault();
                    GetData(address);
                    context.SubmitChanges();
                }
                else
                {
                    context.tbl_AddressCustomers.InsertOnSubmit(this);
                    context.SubmitChanges();
                }
            }
        }

        private void SetData(tbl_AddressCustomer address)
        {
            this.addID = address.addID;
            this.addAddress1 = address.addAddress1;
            this.addAddress2 = address.addAddress2;
            this.addAddress3 = address.addAddress3;
            this.addCity = address.addCity;
            this.addRegion = address.addRegion;
            this.addCountry = address.addCountry;
            this.addPostalCode = address.addPostalCode;
            this.addSource = address.addSource;
            this.addTempID = address.addTempID;
            this.addDateCreated = address.addDateCreated;
            this.addDateUpdated = address.addDateUpdated;
        }

        private void GetData(tbl_AddressCustomer address)
        {
            address.addID = this.addID;
            address.addAddress1 = this.addAddress1;
            address.addAddress2 = this.addAddress2;
            address.addAddress3 = this.addAddress3;
            address.addCity = this.addCity;
            address.addRegion = this.addRegion;
            address.addCountry = this.addCountry;
            address.addPostalCode = this.addPostalCode;
            address.addSource = this.addSource;
            address.addTempID = this.addTempID;
            address.addDateCreated = this.addDateCreated;
            address.addDateUpdated = this.addDateUpdated;
        }
    }
}
