using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wells.Fargo.Application
{
    public class FileReader<T> : IEnumerable<T> where T : class, new()
    {
        private readonly string _fileName;
        private readonly string _delimiter;

        public FileReader(string fileName, string delimiter)
        {
            this._fileName = fileName;
            this._delimiter = delimiter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            using (StreamReader streamReader = new StreamReader(this._fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    T item = new T();
                    Type t = item.GetType();

                    string[] rowData = streamReader.ReadLine().Split(new String[] { this._delimiter }, StringSplitOptions.None);

                    var i = 0;
                    foreach (var propInfo in t.GetProperties())
                    {
                        propInfo.SetValue(item, rowData[i]);
                        i++;
                    }

                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)((IEnumerator<T>)this)).GetEnumerator();
        }
    }
}
