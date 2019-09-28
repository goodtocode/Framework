using GoodToCode.Extensions;
using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Status of the entity current state. Can be multiple values to reduce collisions and maintain independent behavior on a per-value basis.
    /// </summary>
    /// <remarks></remarks>
    public struct RecordStates
    {
        /// <summary>
        /// Normal behavior: Allows all querying and changes.
        /// </summary>
        public static Guid Default = Defaults.Guid;

        /// <summary>
        /// ReadOnly/Locked: Do not allow to be changed. Ignore and log any change request. Alert calling app that record is read only (can be changed back to default to be altered later, not historical.)
        /// </summary>
        public static Guid ReadOnly = new Guid("F3B57E0D-9213-425C-B86B-405E46EB37AA");

        /// <summary>
        /// Record now historical. This record can never be updated, and will now be excluded out of all re-calculations (becomes a line item to feed historical counts.)
        /// </summary>
        public static Guid Historical = new Guid("5A5DAEB7-235A-4E00-9AAB-99C1D96ED5B5");

        /// <summary>
        /// Deleted: This record is deleted and to be considered non-existent, even in historical re-calculations (will make historical counts shift.)
        /// </summary>
        public static Guid Deleted = new Guid("081C6A5B-0817-4161-A3AD-AD7924BEA874");
    }
}
