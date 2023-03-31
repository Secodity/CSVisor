using CSVisor.Core.Exceptions;
using CSVisor.Core.Extender;
using EasyUtilities;
using System.Collections.Generic;

namespace CSVisor.Core.Entities
{
    public class CSVFile
    {
        #region - ctor -
        private CSVFile()
        {
            Lines = new List<IReadOnlyList<string>>();
        }
        public CSVFile(string filePath) : this()
        {
            if (filePath.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullOrEmptyOrWhitespaceException(nameof(filePath));
            
            if(!File.Exists(filePath))
                throw new FileNotFoundException(nameof(filePath));

            FileInfo fileInfo = new(filePath);
            if (!fileInfo.Extension.Equals(".csv"))
                throw new WrongFileFormatException(fileInfo.Extension, "csv", filePath);

            __ReadCSVFile(filePath);
        }
        #endregion

        #region - properties -

        #region [Lines]
        public IList<IReadOnlyList<string>> Lines { get; set; }
        #endregion

        #endregion

        #region - methods -

        #region - private methods -

        #region [__ReadCSVFile]
        private void __ReadCSVFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            char separator = ',';
            bool separatorLineExists = false;
            if (lines[0].StartsWith("sep="))
            {
                separator = lines[0].Split("=").Last().First();
                separatorLineExists = true;
            }
            foreach(var line in separatorLineExists ? lines.Skip(1) : lines)
            {
                Lines.Add(line.Split(separator));
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
