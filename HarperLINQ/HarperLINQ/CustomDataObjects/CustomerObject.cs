using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public enum CustomerResponseCode 
    { 
        SUCCESS = 0, 
        INVALID_PASSWORD = 101,
        NO_SUCH_USER_NAME = 102, 
        EXPIRED_SUB = 103, 
        NO_SUB = 104,
        DUPLICATE_USER_NAME = 105, 
        CANNOT_DECRYPT_STORED_PWD = 106,
        CANNOT_DECRYPT_INPUT = 107,
        DUPLICATE_EMAIL_ADDRESS = 108,
        CANNOT_CREATE_CUSCUSTNUM = 109,

        CANNOT_CONVERT_EXPIREDATE = 201,
        CANNOT_CONVERT_STARTDATE = 202
    }

    [System.Xml.Serialization.XmlInclude(typeof(tbl_Customer))]
    public partial class tbl_Customer
    {
        public long SfgId;
        public string MembershipType;
        public DateTime MembershipExpires;

        #region constructors
        public tbl_Customer(long cusid_or_sfgid, bool issfgid)
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                if (issfgid)
                {
                    this.SfgId = cusid_or_sfgid;
                    this.cusID = (from a in context.SFG_CustomerNumbers
                                  where a.SFGCustNum == this.SfgId.ToString()
                                  select a.cusID).SingleOrDefault();
                }
                else
                {
                    this.cusID = (int)cusid_or_sfgid;
                }
                tbl_Customer customer = (from a in context.tbl_Customers
                                         where a.cusID == this.cusID
                                         select a).SingleOrDefault();
                if (customer != null)
                {
                    SetData(customer);
                }
                var sfgid = (from a in context.SFG_CustomerNumbers
                             where a.cusID == this.cusID
                             select a.SFGCustNum).SingleOrDefault();
                if (sfgid != null)
                {
                    string s = sfgid.ToString();
                    this.SfgId = long.Parse(s);
                }
                try
                {
                    var membership = (from a in context.tbl_NetMemberships
                                      where a.cusID == this.cusID
                                      select a).Take(1).ToArray<tbl_NetMembership>()[0];
                    if (membership != null)
                    {
                        this.MembershipType = membership.mtyCode;
                        this.MembershipExpires = membership.nmbDateEnd;
                    }
                }
                catch { }
            }
        }
        public tbl_Customer(string username_or_emailaddress, bool isusername)
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                tbl_Customer customer;
                if (isusername)
                {
                    this.cusUserName = username_or_emailaddress;
                    customer = (from a in context.tbl_Customers
                                where a.cusUserName == this.cusUserName
                                select a).SingleOrDefault();
                }
                else
                {
                    this.cusEmail = username_or_emailaddress;
                    customer = (from a in context.tbl_Customers
                                where a.cusEmail == this.cusEmail
                                select a).SingleOrDefault();
                }
                if (customer != null)
                {
                    SetData(customer);
                } 
                var sfgid = (from a in context.SFG_CustomerNumbers
                               where a.cusID == this.cusID
                               select a.SFGCustNum).SingleOrDefault();
                if (sfgid != null)
                {
                    string s = sfgid.ToString();
                    this.SfgId = long.Parse(s);
                }
            }       
        }
        public tbl_Customer(string username_or_emailaddress, bool isusername, bool is_encrypted)
        {
            string u = username_or_emailaddress;
            if (is_encrypted)
            {
                u = HarperCRYPTO.Cryptography.Decrypt256FromHEX(u);
            }
            SetData(new tbl_Customer(u, isusername));
        }
        #endregion

        private void SetData(tbl_Customer customer)
        {
            this.cusID = customer.cusID;
            this.cusCustNum = customer.cusCustNum;
            this.cusQuickFillCustNum = customer.cusQuickFillCustNum;
            this.cusSecNum = customer.cusSecNum;
            this.cusCustType = customer.cusCustType;
            this.cusFirstName = customer.cusFirstName;
            this.cusLastName = customer.cusLastName;
            this.cusPAFirstName = customer.cusPAFirstName;
            this.cusPALastName = customer.cusPALastName;
            this.cusPAPhone1 = customer.cusPAPhone1;
            this.cusPAPhone1Ext = customer.cusPAPhone1Ext;
            this.cusCompany = customer.cusCompany;
            this.addID = customer.addID;
            this.addSeasonID = customer.addSeasonID;
            this.cusSeasonStart = customer.cusSeasonStart;
            this.cusSeasonEnd = customer.cusSeasonEnd;
            this.addAltAdd1ID = customer.addAltAdd1ID;
            this.cusPrefix = customer.cusPrefix;
            this.cusSuffix = customer.cusSuffix;
            this.cusTitle = customer.cusTitle;
            this.cusDepartment = customer.cusDepartment;
            this.cusPhone1 = customer.cusPhone1;
            this.cusPhone1Ext = customer.cusPhone1Ext;
            this.cusPhone2 = customer.cusPhone2Ext;
            this.cusPhone2Ext = customer.cusPhone2Ext;
            this.cusMobile = customer.cusMobile;
            this.cusFax = customer.cusFax;
            this.cusEmail = customer.cusEmail;
            this.cusUserName = customer.cusUserName;
            this.cusIsCharterMem = customer.cusIsCharterMem;
            this.cusNextQClubCardDate = customer.cusNextQClubCardDate;
            this.cusSource = customer.cusSource;
            this.cusTempID = customer.cusTempID;
            this.cusSecQClubCardName = customer.cusSecQClubCardName;
            this.cusUserLastUpdated = customer.cusUserLastUpdated;
            this.cusDateCreated = customer.cusDateCreated;
            this.cusDateUpdated = customer.cusDateUpdated;
            this.cusIsDeleted = customer.cusIsDeleted;
            this.cusPriFirstName = customer.cusPriFirstName;
            this.cusPriLastName = customer.cusPriLastName;
            this.cusNickname = customer.cusNickname;
            this.cusSex = customer.cusSex;
            this.cusDOB = customer.cusDOB;
            this.cusDisplayName = customer.cusDisplayName;
            this.cusGUID = customer.cusGUID;
            this.cusHasDisplayName = customer.cusHasDisplayName;
            this.cusDisplayNameUpdated = customer.cusDisplayNameUpdated;
            this.cusAffiliation = customer.cusAffiliation;
            this.cusMemberSince = customer.cusMemberSince;
            this.cusAppStatus = customer.cusAppStatus;
            this.cusComplimentsOf = customer.cusComplimentsOf;
            this.cusTrialCardName = customer.cusTrialCardName;
            this.cusEncryptedPassword = customer.cusEncryptedPassword;
            this.cusPasswordSalt = customer.cusPasswordSalt;
            this.cusPassword = customer.cusPassword;
            this.SfgId = customer.SfgId;
            this.cusSecondEmail = customer.cusSecondEmail;
            this.cusKeyCode = customer.cusKeyCode;
            this.csoCode = customer.csoCode;
        }          
        private void GetData(tbl_Customer customer)
        {
            customer.cusID = this.cusID;
            customer.cusCustNum = this.cusCustNum;
            customer.cusQuickFillCustNum = this.cusQuickFillCustNum;
            customer.cusSecNum = this.cusSecNum;
            customer.cusCustType = this.cusCustType;
            customer.cusFirstName = this.cusFirstName;
            customer.cusLastName = this.cusLastName;
            customer.cusPAFirstName = this.cusPAFirstName;
            customer.cusPALastName = this.cusPALastName;
            customer.cusPAPhone1 = this.cusPAPhone1;
            customer.cusPAPhone1Ext = this.cusPAPhone1Ext;
            customer.cusCompany = this.cusCompany;
            customer.addID = this.addID;
            customer.addSeasonID = this.addSeasonID;
            customer.cusSeasonStart = this.cusSeasonStart;
            customer.cusSeasonEnd = this.cusSeasonEnd;
            customer.addAltAdd1ID = this.addAltAdd1ID;
            customer.cusPrefix = this.cusPrefix;
            customer.cusSuffix = this.cusSuffix;
            customer.cusTitle = this.cusTitle;
            customer.cusDepartment = this.cusDepartment;
            customer.cusPhone1 = this.cusPhone1;
            customer.cusPhone1Ext = this.cusPhone1Ext;
            customer.cusPhone2 = this.cusPhone2Ext;
            customer.cusPhone2Ext = this.cusPhone2Ext;
            customer.cusMobile = this.cusMobile;
            customer.cusFax = this.cusFax;
            customer.cusEmail = this.cusEmail;
            customer.cusUserName = this.cusUserName;
            customer.cusIsCharterMem = this.cusIsCharterMem;
            customer.cusNextQClubCardDate = this.cusNextQClubCardDate;
            customer.cusSource = this.cusSource;
            customer.cusTempID = this.cusTempID;
            customer.cusSecQClubCardName = this.cusSecQClubCardName;
            customer.cusUserLastUpdated = this.cusUserLastUpdated;
            customer.cusDateCreated = this.cusDateCreated;
            customer.cusDateUpdated = this.cusDateUpdated;
            customer.cusIsDeleted = this.cusIsDeleted;
            customer.cusPriFirstName = this.cusPriFirstName;
            customer.cusPriLastName = this.cusPriLastName;
            customer.cusNickname = this.cusNickname;
            customer.cusSex = this.cusSex;
            customer.cusDOB = this.cusDOB;
            customer.cusDisplayName = this.cusDisplayName;
            customer.cusGUID = this.cusGUID;
            customer.cusHasDisplayName = this.cusHasDisplayName;
            customer.cusDisplayNameUpdated = this.cusDisplayNameUpdated;
            customer.cusAffiliation = this.cusAffiliation;
            customer.cusMemberSince = this.cusMemberSince;
            customer.cusAppStatus = this.cusAppStatus;
            customer.cusComplimentsOf = this.cusComplimentsOf;
            customer.cusTrialCardName = this.cusTrialCardName;
            customer.cusEncryptedPassword = this.cusEncryptedPassword;
            customer.cusPasswordSalt = this.cusPasswordSalt;
            customer.cusPassword = this.cusPassword;
            customer.SfgId = this.SfgId;
            customer.cusSecondEmail = this.cusSecondEmail;
            customer.cusKeyCode = this.cusKeyCode;
            customer.csoCode = this.csoCode;
        }        
        public static object[] Login(string enc_username, string enc_password)
        {
            tbl_Customer customer = new tbl_Customer();
            tbl_AddressCustomer address = new tbl_AddressCustomer();
            tbl_NetMembership current_netmembership = new tbl_NetMembership();            
            CustomerResponseCode response_code = Authenticate(enc_username, enc_password);
            if (response_code == CustomerResponseCode.SUCCESS)
            {
                customer = new tbl_Customer(enc_username, true, true);
                address = new tbl_AddressCustomer(customer.addID);
                current_netmembership = tbl_NetMembership.GetCurrentNetMembership(customer.cusID);
                if (current_netmembership == null)
                {
                    response_code = CustomerResponseCode.NO_SUB;
                }
            }
            return new object[] { response_code, customer, address, current_netmembership };
        }
        public static CustomerResponseCode Authenticate(string enc_username, string enc_password)
        {
            bool result = true;
            CustomerResponseCode response_code = CustomerResponseCode.SUCCESS;

            #region decrypt input
            string u = string.Empty;
            string p = string.Empty;
            try
            {
                u = HarperCRYPTO.Cryptography.Decrypt256FromHEX(enc_username);
                p = HarperCRYPTO.Cryptography.Decrypt256FromHEX(enc_password);
            }
            catch
            {
                result = false;
                response_code = CustomerResponseCode.CANNOT_DECRYPT_INPUT;
            }
            #endregion

            if (result)
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    #region get customer by username
                    tbl_Customer customer = new tbl_Customer();
                    try
                    {
                        customer = (from a in context.tbl_Customers
                                    where a.cusUserName == u
                                    && a.cusIsDeleted == false
                                    select a).SingleOrDefault();

                        if (customer == null)
                        {
                            result = false;
                            response_code = CustomerResponseCode.NO_SUCH_USER_NAME;
                        }
                    }
                    catch
                    {
                        result = false;
                        response_code = CustomerResponseCode.DUPLICATE_USER_NAME;
                    }
                    #endregion

                    if (result)
                    {
                        #region decrypt password
                        string storedpwd = string.Empty;
                        try
                        {
                            storedpwd = HarperCRYPTO.Cryptography.Decrypt256FromHEX(customer.cusPassword);
                        }
                        catch
                        {
                            result = false;
                            response_code = CustomerResponseCode.CANNOT_DECRYPT_STORED_PWD;
                        }
                        #endregion

                        if (result)
                        {
                            #region compare passwords
                            if (storedpwd != p)
                            {
                                result = false;
                                response_code = CustomerResponseCode.INVALID_PASSWORD;
                            }
                            #endregion
                        }
                    }
                }
            }
            return response_code;
        }
        public static object[] CreateCustomer(string address1, string address2, string address3, 
            string city, string region, string country, string postal, string source, 
            string password, string customertype, string salutation, string firstname, 
            string middleinitial, string lastname, string suffix, string emailaddress, 
            string username, string newmemberid, string pubcode, string expiredate, 
            string startdate, string screenname, string mobilephone, string secondemail, string keycode)
        {
            CustomerResponseCode responsecode = 0;
            tbl_Customer Customer = new tbl_Customer();
            tbl_NetMembership NetMembership = new tbl_NetMembership();
            tbl_AddressCustomer Address = new tbl_AddressCustomer();

            #region convert string input to correct types
            DateTime dt_expiredate = new DateTime();
            DateTime dt_startdate = new DateTime();

            if (!DateTime.TryParse(expiredate, out dt_expiredate))
            {
                responsecode = CustomerResponseCode.CANNOT_CONVERT_EXPIREDATE;
            }
            if (!DateTime.TryParse(startdate, out dt_startdate))
            {
                responsecode = CustomerResponseCode.CANNOT_CONVERT_STARTDATE;
            }             
            #endregion

            if (responsecode == 0)
            {
                #region check user name availability
                switch (tbl_Customer.CheckUserName(username))
                {
                    case 0:
                        responsecode = 0;
                        break;
                    case 1:
                        responsecode = CustomerResponseCode.DUPLICATE_USER_NAME;
                        break;
                    case 2:
                        responsecode = CustomerResponseCode.DUPLICATE_EMAIL_ADDRESS;
                        break;
                    case 3:
                        responsecode = CustomerResponseCode.DUPLICATE_USER_NAME;
                        break;
                }
                #endregion
                if (responsecode == 0)
                {
                    using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                    {
                        #region get cuscustnum
                        long newcustnum = 0;
                        try
                        {
                            long lastcustomer = (from a in context.tbl_Customers select a.cusCustNum).Select(x => Convert.ToInt64(x)).Max();
                            newcustnum = lastcustomer + 1;
                        }
                        catch
                        {
                            responsecode = CustomerResponseCode.CANNOT_CREATE_CUSCUSTNUM;
                        }
                        #endregion

                        if (responsecode == 0)
                        {
                            #region address data at AH
                            Address = new tbl_AddressCustomer();
                            Address.addAddress1 = address1;
                            Address.addAddress2 = address2;
                            Address.addAddress3 = address3;
                            Address.addCity = city;
                            Address.addCountry = country;
                            Address.addDateCreated = DateTime.Now;
                            Address.addDateUpdated = DateTime.Now;
                            Address.addPostalCode = postal;
                            Address.addRegion = string.IsNullOrEmpty(region) ? "" : region;
                            Address.addSource = string.Empty;
                            context.tbl_AddressCustomers.InsertOnSubmit(Address);
                            context.SubmitChanges();
                            #endregion

                            #region customer data at AH
                            Customer.cusEncryptedPassword = HarperCRYPTO.Cryptography.EncryptData(password);
                            Customer.cusPassword = HarperCRYPTO.Cryptography.Encrypt256(password);
                            Customer.cusPasswordSalt = HarperCRYPTO.Cryptography.Salt;
                            Customer.addID = Address.addID;
                            Customer.cusCustNum = newcustnum.ToString();
                            Customer.cusCustType = customertype;
                            Customer.cusFirstName = firstname;
                            Customer.cusLastName = lastname;
                            Customer.cusPriFirstName = firstname;
                            Customer.cusPriLastName = lastname;
                            Customer.cusEmail = emailaddress;
                            Customer.cusUserName = username;
                            Customer.cusIsCharterMem = false;
                            Customer.cusDateCreated = DateTime.Now;
                            Customer.cusDateUpdated = DateTime.Now;
                            Customer.cusIsDeleted = false;
                            Customer.cusSex = 'U';
                            Customer.cusGUID = Guid.NewGuid();
                            Customer.cusDisplayName = screenname;
                            Customer.cusMobile = mobilephone;
                            Customer.cusHasDisplayName = true;
                            Customer.cusSecondEmail = secondemail;
                            Customer.cusMemberSince = DateTime.Now;
                            Customer.cusSource = source;
                            Customer.cusKeyCode = keycode;
                            Customer.csoCode = source;
                            long.TryParse(newmemberid, out Customer.SfgId);
                            context.tbl_Customers.InsertOnSubmit(Customer);
                            context.SubmitChanges();
                            #endregion

                            #region sfg customer number data at AH
                            HarperLINQ.SFG_CustomerNumber SfgData = new HarperLINQ.SFG_CustomerNumber();
                            SfgData.cusID = Customer.cusID;
                            SfgData.SFGCustNum = newmemberid;
                            context.SFG_CustomerNumbers.InsertOnSubmit(SfgData);
                            context.SubmitChanges();
                            #endregion

                            #region net membership data at AH
                            NetMembership = new tbl_NetMembership();
                            NetMembership.cusID = Customer.cusID;
                            NetMembership.mtyCode = HarperLINQ.SFG_ProdCode.GetFromExtCode(pubcode).IntCode;
                            NetMembership.nmbDateCreated = DateTime.Now;
                            NetMembership.nmbDateEnd = dt_expiredate;
                            NetMembership.nmbDateStart = dt_startdate;
                            context.tbl_NetMemberships.InsertOnSubmit(NetMembership);
                            context.SubmitChanges();
                            #endregion

                        }
                    }
                }
            }
            return new object[] { responsecode, Customer, Address, NetMembership };
        }
        
        /// <summary>
        /// Legacy method before second email address and keycode were added
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="address3"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <param name="country"></param>
        /// <param name="postal"></param>
        /// <param name="source"></param>
        /// <param name="password"></param>
        /// <param name="customertype"></param>
        /// <param name="salutation"></param>
        /// <param name="firstname"></param>
        /// <param name="middleinitial"></param>
        /// <param name="lastname"></param>
        /// <param name="suffix"></param>
        /// <param name="emailaddress"></param>
        /// <param name="username"></param>
        /// <param name="newmemberid"></param>
        /// <param name="pubcode"></param>
        /// <param name="expiredate"></param>
        /// <param name="startdate"></param>
        /// <param name="screenname"></param>
        /// <param name="mobilephone"></param>
        /// <returns></returns>
        public static object[] CreateCustomer(string address1, string address2, string address3, 
            string city, string region, string country, string postal, string source, string password, 
            string customertype, string salutation, string firstname, string middleinitial, string lastname, 
            string suffix, string emailaddress, string username, string newmemberid, string pubcode, 
            string expiredate, string startdate, string screenname, string mobilephone)
        {
            return CreateCustomer(address1, address2, address3, city, region, country, postal, source, password, customertype, salutation, firstname, middleinitial, lastname, suffix, emailaddress, username, newmemberid, pubcode, expiredate, startdate, screenname, mobilephone, string.Empty, string.Empty);
        }

        public static List<string> GetUserNames()
        {
            List<string> u = new List<string>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                u = (from a in context.tbl_Customers
                     where a.cusIsDeleted == false
                     select a.cusUserName).Distinct().ToList<string>();
                u.AddRange((from b in context.tbl_Customers
                            where b.cusIsDeleted == false
                            select b.cusDisplayName).Distinct().ToList<string>());
                u.AddRange((from c in context.tbl_Customers
                            where c.cusIsDeleted == false
                            select c.cusEmail).Distinct().ToList<string>());
            }
            u.Sort();
            List<string> usernames = u.Distinct().ToList<string>();
            return usernames;
        }

        public static List<tbl_Customer> GetAllCustomers()
        {
            List<tbl_Customer> u = new List<tbl_Customer>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                u = (from a in context.tbl_Customers
                     where a.cusIsDeleted == false
                     && a.cusUserName != null
                     && a.cusEncryptedPassword != null
                     select a).ToList<tbl_Customer>();                
            }
            return u;
        }

        public static bool IsUserNameAvailable(string username, int? cusid)
        {
            int u = 0;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                if (cusid.HasValue)
                {
                    u = (from a in context.tbl_Customers
                         where a.cusIsDeleted == false
                         && (a.cusUserName == username
                         || a.cusEmail == username
                         || a.cusDisplayName == username)
                         && a.cusID != cusid.Value
                         select a).Count();
                }
                else
                {
                    u = (from a in context.tbl_Customers
                         where a.cusIsDeleted == false
                         && (a.cusUserName == username
                         || a.cusEmail == username
                         || a.cusDisplayName == username)
                         select a).Count();
                }
            }
            return u > 0 ? false : true;
        }

        public static bool IsUserNameAvailable(string username)
        {
            return tbl_Customer.IsUserNameAvailable(username, null);
        }

        public static int CheckUserName(string username)
        {
            return tbl_Customer.CheckUserName(username, null);
        }
        public static int CheckUserName(string username, int? cusid)
        {
            int u = 0;
            int response = 0;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                if (cusid.HasValue)
                {
                    u = (from a in context.tbl_Customers
                         where a.cusIsDeleted == false
                         && a.cusUserName == username
                         && a.cusID != cusid
                         select a).Count();
                }
                else
                {
                    u = (from a in context.tbl_Customers
                         where a.cusIsDeleted == false
                         && a.cusUserName == username
                         select a).Count();
                }
                if(u > 0)
                {
                    return 1;//duplicate username
                }
                else
                {
                    if (cusid.HasValue)
                    {
                        u = (from a in context.tbl_Customers
                             where a.cusIsDeleted == false
                             && a.cusEmail == username
                             && a.cusID != cusid
                             select a).Count();
                    }
                    else
                    {
                        u = (from a in context.tbl_Customers
                             where a.cusIsDeleted == false
                             && a.cusEmail == username
                             select a).Count();
                    }
                    if(u > 0)
                    {
                        return 2; //duplicate email
                    }
                    else
                    {
                        if (cusid.HasValue)
                        {
                            u = (from a in context.tbl_Customers
                                 where a.cusIsDeleted == false
                                 && a.cusDisplayName == username
                                 && a.cusID != cusid
                                 select a).Count();
                        }
                        else
                        {
                            u = (from a in context.tbl_Customers
                                 where a.cusIsDeleted == false
                                 && a.cusDisplayName == username
                                 select a).Count();                            
                        }
                        if (u > 0)
                        {
                            return 3; //duplicate screenname
                        }
                    }
                }
            }
            return response;
        }        
        
        public void Save()
        {
            try
            {                
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    tbl_CustomerSource source = (from a in context.tbl_CustomerSources 
                                                 where a.csoCode == this.csoCode 
                                                 select a).SingleOrDefault();
                    this.csoCode = source == null ? null : source.csoCode;
                    if (this.cusID > 0)
                    {
                        tbl_Customer customer = (from a in context.tbl_Customers
                                                 where a.cusID == this.cusID
                                                 select a).SingleOrDefault();                        
                        GetData(customer);
                        context.SubmitChanges();
                    }
                    else
                    {
                        context.tbl_Customers.InsertOnSubmit(this);
                        context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ObjectSaveException(ex.Message);
            }
        }


        public static List<int> GetAllCusIds()
        {
            List<int> customers = new List<int>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             where a.cusIsDeleted == false
                             //&& a.cusPassword != null
                             && a.cusUserName != null
                             select a.cusID).ToList<int>();

            }
            return customers;

        }

        public static List<tbl_Customer> GetCustomersWithPwdSfgidMembership()
        {
            List<tbl_Customer> customers = new List<tbl_Customer>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                             join c in context.tbl_NetMemberships on a.cusID equals c.cusID
                             where a.cusPassword != null
                             select a).ToList<tbl_Customer>();

            }
            return customers;
        }
        public static List<int> GetCustomersWithoutNewPwd()
        {
            List<int> customers = new List<int>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                             join c in context.tbl_NetMemberships on a.cusID equals c.cusID
                             where (a.cusPassword == null || a.cusPassword.Trim() == string.Empty)
                             && a.cusEncryptedPassword != null
                             && a.cusIsDeleted == false
                             select a.cusID).ToList<int>();

            }
            return customers;
        }
        public static List<tbl_Customer> GetCustomersWithoutMembership()
        {
            List<tbl_Customer> customers = new List<tbl_Customer>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                             into ab
                             from c in ab.DefaultIfEmpty()
                             where c == null //no sfgid
                             && a.cusPassword != null
                             select a).ToList<tbl_Customer>();

            }
            return customers;
        }
        public static List<tbl_Customer> GetCustomersWithoutSfgidMembership()
        {
            List<tbl_Customer> customers = new List<tbl_Customer>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                             into ab
                             from c in ab.DefaultIfEmpty()
                             join d in context.tbl_NetMemberships on c.cusID equals d.cusID
                             into cd
                             from e in cd.DefaultIfEmpty()
                             where c == null //no sfgid
                             && e == null //no membership
                             && a.cusPassword != null
                             select a).ToList<tbl_Customer>();

            }
            return customers;
        }
        public static List<tbl_Customer> GetCustomersWithoutPwdSfgidMembership()
        {
            List<tbl_Customer> customers = new List<tbl_Customer>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                customers = (from a in context.tbl_Customers
                             join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                             into ab
                             from c in ab.DefaultIfEmpty()
                             join d in context.tbl_NetMemberships on c.cusID equals d.cusID
                             into cd
                             from e in cd.DefaultIfEmpty()
                             where c == null //no sfgid
                             && e == null //no membership
                             && a.cusPassword == null
                             select a).ToList<tbl_Customer>();

            }
            return customers;
        }
        
        public static int CountCustomersWithPwdSfgidMembership()
        {
            int count;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                count = (from a in context.tbl_Customers
                         join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                         join c in context.tbl_NetMemberships on a.cusID equals c.cusID
                         where a.cusPassword != null
                         select a).Count();

            }
            return count;
        }
        public static int CountCustomersWithoutMembership()
        {
            int count;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                count = (from a in context.tbl_Customers
                         join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                         into ab
                         from c in ab.DefaultIfEmpty()
                         where c == null //no sfgid
                         && a.cusPassword != null
                         select a).Count();

            }
            return count;
        }
        public static int CountCustomersWithoutSfgidMembership()
        {
            int count;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                count = (from a in context.tbl_Customers
                         join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                         into ab
                         from c in ab.DefaultIfEmpty()
                         join d in context.tbl_NetMemberships on c.cusID equals d.cusID
                         into cd
                         from e in cd.DefaultIfEmpty()
                         where c == null //no sfgid
                         && e == null //no membership
                         && a.cusPassword != null
                         select a).Count();

            }
            return count;
        }
        public static int CountCustomersWithoutPwdSfgidMembership()
        {
            int count;
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                count = (from a in context.tbl_Customers
                         join b in context.SFG_CustomerNumbers on a.cusID equals b.cusID
                         into ab
                         from c in ab.DefaultIfEmpty()
                         join d in context.tbl_NetMemberships on c.cusID equals d.cusID
                         into cd
                         from e in cd.DefaultIfEmpty()
                         where c == null //no sfgid
                         && e == null //no membership
                         && a.cusPassword == null
                         select a).Count();

            }
            return count;
        }



        //needed for C360
        public void DeleteUser()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                tbl_Customer me = (from a in context.tbl_Customers
                               where a.cusID == this.cusID
                               select a).Single();
                me.cusIsDeleted = true;
                me.cusArchiveEmail = me.cusEmail;
                me.cusArchiveUsername = me.cusUserName;
                me.cusArchiveDisplayName = me.cusDisplayName;
                me.cusEmail = string.Empty;
                me.cusUserName = string.Empty;
                me.cusDisplayName = string.Empty;
                context.SubmitChanges();
            }
        }
        public void UnDeleteUser()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                bool exists = false;
                string errormsg = string.Empty;

                #region check email
                int existing = (from a in context.tbl_Customers
                                where a.cusIsDeleted == false
                                && a.cusID != this.cusID
                                && a.cusEmail == this.cusArchiveEmail
                                select a).Count();
                if (existing > 0)
                {
                    exists = true;
                    errormsg = "Email in use.";
                }
                #endregion

                #region check user name
                existing = 0;
                existing = (from a in context.tbl_Customers
                            where a.cusIsDeleted == false
                            && a.cusID != this.cusID
                            && a.cusUserName == this.cusArchiveUsername
                            select a).Count();
                if (existing > 0)
                {
                    exists = true;
                    errormsg += "User Name in use.";
                }
                #endregion

                #region check screen name
                existing = 0;
                existing = (from a in context.tbl_Customers
                            where a.cusIsDeleted == false
                            && a.cusID != this.cusID
                            && a.cusDisplayName == this.cusArchiveDisplayName
                            select a).Count();
                if (existing > 0)
                {
                    exists = true;
                    errormsg += "Screen Name in use.";
                }
                #endregion

                if (exists)
                {
                    throw new Exception(string.Format("Unable to un-delete user, {0}", errormsg));
                }
                else
                {
                    tbl_Customer me = (from a in context.tbl_Customers
                                   where a.cusID == this.cusID
                                   select a).Single();
                    me.cusIsDeleted = false;
                    me.cusEmail = me.cusArchiveEmail;
                    me.cusUserName = me.cusArchiveUsername;
                    me.cusDisplayName = me.cusArchiveDisplayName;
                    me.cusArchiveEmail = string.Empty;
                    me.cusArchiveUsername = string.Empty;
                    me.cusArchiveDisplayName = string.Empty;
                    context.SubmitChanges();
                }
            }
        }
        public void SaveSfgId()
        {
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                try
                {
                    SFG_CustomerNumber sfg = (from a in context.SFG_CustomerNumbers
                                              where a.cusID == this.cusID
                                              select a).Single();
                    sfg.SFGCustNum = this.SfgId.ToString();
                    context.SubmitChanges();
                }
                catch { }
            }
        }
    }
}
