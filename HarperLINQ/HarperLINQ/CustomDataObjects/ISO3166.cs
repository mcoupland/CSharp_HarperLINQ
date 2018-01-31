using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HarperLINQ
{
    public enum GeographicType { Country, Level1_Subdivision, Level2_Subdivision, Unknown }
    public enum IdentifierType { Id, Country_Code_Alpha2, Country_Code_Alpha3, Country_Code_Numeric, Level1_Subdivision_Code, Level2_Subdivision_Code, Name, DisplayName }
    public class IdentifierNotUniqueException:Exception
    {
        private object id;
        private IdentifierType idtype;
        public object Id { get { return id; } }
        public IdentifierType IdType{get{return idtype;}}
        public new string Message
        {
	        get {return string.Format("Identifier \"{0}\" of type \"{1}\" returned more than one object", new object[]{Id, IdType.ToString()});}
        }
        public IdentifierNotUniqueException(object identifier, IdentifierType identifiertype)
        {
            id = identifier;
            idtype = identifiertype;
        }
    }
    public class ObjectNotFoundException : Exception
    {
        private object id;
        private IdentifierType idtype;
        public object Id { get { return id; } }
        public IdentifierType IdType{get{return idtype;}}
        public new string Message
        {
	        get {return string.Format("No objects found with identifier \"{0}\" of type \"{1}\"", new object[]{Id, IdType.ToString()});}
        }
        public ObjectNotFoundException(object identifier, IdentifierType identifiertype)
        {
            id = identifier;
            idtype = identifiertype;
        }
    }

    public partial class ISO3166
    {
        public string SafeDisplayName
        {
            get { return DisplayName == null ? Name : DisplayName; }
        }

        private GeographicType _geotype;
        public GeographicType GeoType
        {
            get 
            {
                if (ParentId != null && !string.IsNullOrEmpty(Level2_Subdivision))
                {
                    _geotype = GeographicType.Level2_Subdivision;
                }
                else if (ParentId != null && !string.IsNullOrEmpty(Level1_Subdivision))
                {
                    _geotype = GeographicType.Level1_Subdivision;
                }
                else if (ParentId == null && (!string.IsNullOrEmpty(Alpha2) || !string.IsNullOrEmpty(Alpha3) || Numeric.HasValue))
                {
                    _geotype = GeographicType.Country;
                }
                else
                {
                    _geotype = GeographicType.Unknown;
                }
                return _geotype; 
            }
            set { _geotype = value; }
        }
        
        public ISO3166(object identifier, IdentifierType identifiertype)
        {           
            try
            {
                using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
                {
                    ISO3166 iso = null;
                    switch (identifiertype)
                    {
                        case IdentifierType.Id:
                            iso = (from a in context.ISO3166s where a.Id == (int)identifier && a.ParentId == null select a).SingleOrDefault();
                            break;
                        case IdentifierType.Country_Code_Alpha2:
                            iso = (from a in context.ISO3166s where a.Alpha2 == (string)identifier && a.ParentId == null select a).SingleOrDefault();
                            break;
                        case IdentifierType.Country_Code_Alpha3:
                            iso = (from a in context.ISO3166s where a.Alpha3 == (string)identifier && a.ParentId == null select a).SingleOrDefault();
                            break;
                        case IdentifierType.Country_Code_Numeric:
                            iso = (from a in context.ISO3166s where a.Numeric == (int)identifier && a.ParentId == null select a).SingleOrDefault();
                            break;
                        case IdentifierType.Level1_Subdivision_Code:
                            iso = (from a in context.ISO3166s where a.Level1_Subdivision == (string)identifier select a).SingleOrDefault();
                            break;
                        case IdentifierType.Level2_Subdivision_Code:
                            iso = (from a in context.ISO3166s where a.Level2_Subdivision == (string)identifier select a).SingleOrDefault();
                            break;
                        case IdentifierType.Name:
                            iso = (from a in context.ISO3166s where a.Name == (string)identifier select a).SingleOrDefault();
                            break;
                        case IdentifierType.DisplayName:
                            iso = (from a in context.ISO3166s where a.DisplayName == (string)identifier select a).SingleOrDefault();
                            break;
                    }
                    if (iso == null)
                    {
                        throw new ObjectNotFoundException(identifier, identifiertype);
                    }
                    else
                    {
                        this.Id = iso.Id;
                        this.Alpha2 = iso.Alpha2;
                        this.Alpha3 = iso.Alpha3;
                        this.Numeric = iso.Numeric;
                        this.Level1_Subdivision = iso.Level1_Subdivision;
                        this.Level2_Subdivision = iso.Level2_Subdivision;
                        this.Name = iso.Name;
                        this.ParentId = iso.ParentId;
                        this.SFGCode = iso.SFGCode;
                        this.DisplayName = iso.DisplayName;
                    }
                }
            }
            catch
            {
                throw new IdentifierNotUniqueException(identifier, identifiertype);
            }
        }

        public static List<ISO3166> GetCountries()
        {
            List<ISO3166> response = new List<ISO3166>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                response = (from a in context.ISO3166s
                            where a.ParentId == null
                            orderby a.DisplayName, a.Name
                            select a).ToList();
            }
            return response;
        }

        public static List<ISO3166> GetSFGSafeCountries()
        {
            List<ISO3166> response = new List<ISO3166>();
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                response = (from a in context.ISO3166s
                            where a.ParentId == null
                            && a.SFGCode != null
                            orderby a.DisplayName, a.Name
                            select a).ToList();
            }
            return response;
        }
        //public List<ISO3166> GetAll() { }
        //public List<ISO3166> GetAllOfType(GeographicType geographictype) { }
        public static List<ISO3166> GetRegions(string ParentAlpha2) 
        {
            List<ISO3166> response = new List<ISO3166>(); 
            using (AHT_MainDataContext context = new AHT_MainDataContext(ConfigurationManager.ConnectionStrings["AHT_MainConnectionString"].ConnectionString))
            {
                response = (from a in context.ISO3166s
                            join b in context.ISO3166s
                            on a.ParentId equals b.Id
                            where b.Alpha2 == ParentAlpha2
                            orderby a.DisplayWeight, a.DisplayName, a.Name
                            select a).ToList();
            }
            return response;
        }    
    }
}
