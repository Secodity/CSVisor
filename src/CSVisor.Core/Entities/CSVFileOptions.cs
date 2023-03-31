using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVisor.Core.Entities
{
    public class CSVFileOptions
    {
        #region - ctor -
        public CSVFileOptions(CSVFile file)
        {
            File = file;
        }
        #endregion

        #region - properties -
        public CSVFile File { get; set; }
        public uint UniqueIdentifyColumn { get; set; }
        public Type UniqueIdentifyColumnType { get; set; }
        public IList<Tuple<uint, Type, bool>> SortingColumns { get; set; } = new List<Tuple<uint, Type, bool>>();
        #endregion
    }
}
