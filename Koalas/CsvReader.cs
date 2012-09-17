using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class CsvReader : IEnumerator<List<String>>, IEnumerable<List<String>>
    {
        private enum DataSourceType { String, File };

        private DataSourceType _dataSourceType;
        private String _dataSourceString;
        private TextReader _textReader;
        private readonly char _quoteChar;
        private readonly char _delimiter;
        private bool _inQuotedField = false;
        private bool _previousCharacterQuote = false;
        public List<String> Row; 
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private CsvReader(TextReader textReader, char delimiter=',', char quoteChar='"')
        {
            _textReader = textReader;
            _delimiter = delimiter;
            _quoteChar = quoteChar;
        } 

        public static CsvReader FromString(String data, char delimiter=',', char quoteChar='"')
        {
            return new CsvReader(new StringReader(data), delimiter, quoteChar)
                       {
                           _dataSourceType = DataSourceType.String,
                           _dataSourceString = data
                       };
        }

        public static CsvReader FromFile(String filename, char delimiter=',', char quoteChar='"')
        {
            throw new NotImplementedException();
        }

        bool IEnumerator.MoveNext()
        {
            if (_textReader.Peek() < 0)
                return false;

            Row = new List<string>();
            while (_textReader.Peek() > 0)
            {
                var t = (char) _textReader.Read();
                if (_inQuotedField)
                {
                    if (t == _quoteChar)
                    {
                        _inQuotedField = false;
                        _previousCharacterQuote = true;
                    }
                    else
                    {
                        _previousCharacterQuote = false;
                        _stringBuilder.Append(t);
                    }
                }
                else
                {

                    if (t == _delimiter)
                    {
                        Row.Add(_stringBuilder.ToString());
                        _stringBuilder.Clear();
                    }
                    else if (t == '\r' || t == '\n')
                    {
                        if (Row.Any() || _stringBuilder.Length > 0)
                        {
                            Row.Add(_stringBuilder.ToString());
                            _stringBuilder.Clear();
                            return true;
                        }
                        // Else this is a blank line, and we skip over it
                    }
                    else if (t == _quoteChar)
                    {
                        _inQuotedField = true;
                        if (_previousCharacterQuote)
                            _stringBuilder.Append(t);
                        else
                            _previousCharacterQuote = true;
                    }
                    else
                    {
                        _stringBuilder.Append(t);
                    }
                }
            }
            if (Row.Any() || _stringBuilder.Length > 0)
            {
                Row.Add(_stringBuilder.ToString());
                _stringBuilder.Clear();
                return true;
            }
            return false;
        }

        public void Reset()
        {
            switch (_dataSourceType)
            {
                case DataSourceType.String:
                    _textReader = new StringReader(_dataSourceString);
                    break;
                case DataSourceType.File:
                    throw new NotSupportedException();
            }
            _inQuotedField = false;
            _previousCharacterQuote = false;
        }

        public object Current
        {
            get { return Row; }
        }

        List<string> IEnumerator<List<string>>.Current
        {
            get { return Row; }
        }

        public void Dispose()
        {
            _textReader.Dispose();
        }

        public IEnumerator<List<string>> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
