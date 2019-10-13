using System;
using System.Data;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Data reader that handles nulls
    /// </summary>
    public class DataReaderSafe : IDataReader, IDisposable
    {
        private IDataReader _dataReader;
        private bool disposed;

        /// <summary>
        /// Get a reference to the underlying data reader
        /// object that actually contains the data from
        /// the data source.
        /// </summary>
        protected IDataReader DataReader
        {
            get
            {
                return _dataReader;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DR"></param>
        /// <remarks></remarks>
        public DataReaderSafe(IDataReader DR)
        {
            _dataReader = DR;
        }

        /// <summary>
        /// Gets date from database
        /// </summary>
        /// <param name="MyDate"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime GetDate(string MyDate)
        {
            DateTime returnValue = default(DateTime);
            returnValue = GetDateTime(MyDate);
            if (returnValue <= default(DateTime))
                returnValue = default(DateTime);
            return returnValue;
        }

        /// <summary>
        /// Defaults 0 to -1
        /// This is for columns that incorrectly use 0 as 'none'
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int GetInt32Default(string FieldName)
        {
            int returnValue;
            returnValue = GetInt32(FieldName);
            if (returnValue < 1)
                returnValue = default(int);
            return returnValue;
        }

        /// <summary>
        /// Gets a string value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns empty string for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public string GetString(string name)
        {
            return GetString(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a string value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns empty string for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetString(int i)
        {
            if (_dataReader.IsDBNull(i))
                return String.Empty;
            else
                return _dataReader.GetString(i);
        }

        /// <summary>
        /// Gets a value of type from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public object GetValue(string name)
        {
            return GetValue(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a value of type from the datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual object GetValue(int i)
        {
            if (_dataReader.IsDBNull(i))
                return null;
            else
                return _dataReader.GetValue(i);
        }

        /// <summary>
        /// Gets an integer from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public int GetInt32(string name)
        {
            return GetInt32(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets an integer from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual int GetInt32(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetInt32(i);
        }

        /// <summary>
        /// Gets a double from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public double GetDouble(string name)
        {
            return GetDouble(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a double from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual double GetDouble(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetDouble(i);
        }

        /// <summary>
        /// Gets a Guid value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns Guid.Empty for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public Guid GetGuid(string name)
        {
            return GetGuid(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Guid value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns Guid.Empty for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual Guid GetGuid(int i)
        {
            if (_dataReader.IsDBNull(i))
                return Guid.Empty;
            else
                return _dataReader.GetGuid(i);
        }

        /// <summary>
        /// Gets a boolean value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns <see langword="false" /> for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public bool GetBoolean(string name)
        {
            return GetBoolean(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a boolean value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns <see langword="false" /> for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual bool GetBoolean(int i)
        {
            if (_dataReader.IsDBNull(i))
                return false;
            else
                return _dataReader.GetBoolean(i);
        }

        /// <summary>
        /// Gets a byte value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public byte GetByte(string name)
        {
            return GetByte(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a byte value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual byte GetByte(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetByte(i);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            return GetBytes(_dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets a char value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns Char.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public char GetChar(string name)
        {
            return GetChar(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a char value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns Char.MinValue for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual char GetChar(int i)
        {
            if (_dataReader.IsDBNull(i))
            {
                return char.MinValue;
            }
            else
            {
                var myChar = new char[0];
                _dataReader.GetChars(i, 0, myChar, 0, 1);
                return myChar[0];
            }
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long GetChars(string name, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return GetChars(_dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetData method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public IDataReader GetData(string name)
        {
            return GetData(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetData method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual IDataReader GetData(int i)
        {
            return _dataReader.GetData(i);
        }

        /// <summary>
        /// Invokes the GetDataTypeName method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public string GetDataTypeName(string name)
        {
            return GetDataTypeName(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetDataTypeName method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetDataTypeName(int i)
        {
            return _dataReader.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets a date value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns DateTime.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public virtual DateTime GetDateTime(string name)
        {
            return GetDateTime(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a date value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns DateTime.MinValue for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual DateTime GetDateTime(int i)
        {
            if (_dataReader.IsDBNull(i))
                return DateTime.MinValue;
            else
                return _dataReader.GetDateTime(i);
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public decimal GetDecimal(string name)
        {
            return GetDecimal(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual decimal GetDecimal(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetDecimal(i);
        }

        /// <summary>
        /// Invokes the GetFieldType method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public Type GetFieldType(string name)
        {
            return GetFieldType(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetFieldType method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual Type GetFieldType(int i)
        {
            return _dataReader.GetFieldType(i);
        }

        /// <summary>
        /// Gets a Single value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public float GetFloat(string name)
        {
            return GetFloat(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Single value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual float GetFloat(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetFloat(i);
        }

        /// <summary>
        /// Gets a Short value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public short GetInt16(string name)
        {
            return GetInt16(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Short value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual short GetInt16(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetInt16(i);
        }

        /// <summary>
        /// Gets a Long value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public long GetInt64(string name)
        {
            return GetInt64(_dataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Long value from the datareader.
        /// </summary>
        /// <remarks>
        /// returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual long GetInt64(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            else
                return _dataReader.GetInt64(i);
        }

        /// <summary>
        /// Invokes the GetName method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetName(int i)
        {
            return _dataReader.GetName(i);
        }

        /// <summary>
        /// Gets an ordinal value from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public int GetOrdinal(string name)
        {
            return _dataReader.GetOrdinal(name);
        }

        /// <summary>
        /// Invokes the GetValues method of the underlying datareader.
        /// </summary>
        /// <param name="values">An array of System.Object to
        /// copy the values into.</param>
        public int GetValues(object[] values)
        {
            return _dataReader.GetValues(values);
        }

        /// <summary>
        /// Invokes the GetSchemaTable method of the underlying datareader.
        /// </summary>
        public DataTable GetSchemaTable()
        {
            return _dataReader.GetSchemaTable();
        }

        /// <summary>
        /// Reads the next row of data from the datareader.
        /// </summary>
        public bool Read()
        {
            return _dataReader.Read();
        }

        /// <summary>
        /// Moves to the next result set in the datareader.
        /// </summary>
        public bool NextResult()
        {
            return _dataReader.NextResult();
        }

        /// <summary>
        /// Closes the datareader.
        /// </summary>
        public void Close()
        {
            _dataReader.Close();
        }

        /// <summary>
        /// returns the depth property value from the datareader.
        /// </summary>
        public int Depth
        {
            get { return _dataReader.Depth; }
        }

        /// <summary>
        /// returns the FieldCount property from the datareader.
        /// </summary>
        public int FieldCount
        {
            get { return _dataReader.FieldCount; }
        }

        /// <summary>
        /// returns the IsClosed property value from the datareader.
        /// </summary>
        public bool IsClosed
        {
            get { return _dataReader.IsClosed; }
        }

        /// <summary>
        /// Invokes the IsDBNull method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual bool IsDBNull(int i)
        {
            return _dataReader.IsDBNull(i);
        }

        /// <summary>
        /// Invokes the IsDBNull method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public virtual bool IsDBNull(string name)
        {
            var index = GetOrdinal(name);
            return IsDBNull(index);
        }

        /// <summary>
        /// returns a value from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public object this[string name]
        {
            get
            {
                var val = _dataReader[name];
                if (DBNull.Value.Equals(val))
                    return null;
                else
                    return val;
            }
        }

        /// <summary>
        /// returns a value from the datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public object this[int i]
        {
            get
            {
                if (_dataReader.IsDBNull(i))
                    return null;
                else
                    return _dataReader[i];
            }
        }

        /// <summary>
        /// returns the RecordsAffected property value from the underlying datareader.
        /// </summary>
        public int RecordsAffected
        {
            get { return _dataReader.RecordsAffected; }
        }
        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Inheritance disposal
        /// </summary>
        /// <param name="disposing">Disposing state flag</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dataReader.Dispose();
                }
                disposed = true;
            }
        }
    }
}