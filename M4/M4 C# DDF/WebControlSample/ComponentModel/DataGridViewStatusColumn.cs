using System.Windows.Forms;
using System.ComponentModel;
using M4.Properties;

namespace M4.WebControlSample.ComponentModel
{
    public class DataGridViewStatusColumn : DataGridViewImageColumn
    {
        public DataGridViewStatusColumn()
        {
            CellTemplate = new DataGridViewStatusCell();
        }
    }

    public class DataGridViewStatusCell : DataGridViewImageCell
    {

        public DataGridViewStatusCell()
        {
            ValueType = typeof(bool);
        }

        protected override object GetFormattedValue(object value,
            int rowIndex,
            ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            bool statusVal = (bool)value;
            return statusVal ? Resources.development_51 : Resources.development_52;
        }
    }
}