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
        public IList<Tuple<uint, eSortDirection>> GroupingSortingColumns { get; set; } = new List<Tuple<uint, eSortDirection>>();
        public IList<Tuple<uint, eSortDirection>> StateSortingColumns { get; set; } = new List<Tuple<uint, eSortDirection>>();
        #endregion
    }
}
