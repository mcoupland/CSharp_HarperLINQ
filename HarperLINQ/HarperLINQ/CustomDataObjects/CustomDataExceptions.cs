using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarperLINQ
{
    public class DataException : Exception
    {
        new public string Message;
    }

    /// <summary>
    /// Thrown when an exception occurs while the data is being loaded into the object from the database
    /// </summary>
    public class DataLoadException : DataException
    {
        public DataLoadException(string message)
        {
            this.Message = message;
        }
    }


    /// <summary>
    /// Thrown when an exception occurs while the data is being loaded into the object from user supplied parameters
    /// </summary>
    public class ObjectInitException : DataException
    {
        public ObjectInitException(string message)
        {
            this.Message = message;
        }
    }


    /// <summary>
    /// Thrown when an exception occurs while the object is being saved to the database
    /// </summary>
    public class ObjectSaveException : DataException
    {
        public ObjectSaveException(string message)
        {
            this.Message = message;
        }
    }
}
