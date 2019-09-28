using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Byte array used in all BLOB objects
    /// </summary>
    public interface IBytesKey : IKey
    {
        /// <summary>
        /// Bytes
        /// </summary>
        byte[] Bytes { get; set; }
    }
}
