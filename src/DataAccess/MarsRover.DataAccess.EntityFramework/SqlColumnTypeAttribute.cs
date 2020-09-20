using System;

namespace Hb.MarsRover.DataAccess.EntityFramework
{
    public sealed class SqlColumnTypeAttribute : Attribute
    {
        public string ColumnType { get; private set; }

        public SqlColumnTypeAttribute(string columnType)
        {
            ColumnType = columnType;
        }
    }
}