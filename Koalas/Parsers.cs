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
        private readonly TextReader _textReader;
        private readonly char _quoteChar;
        private readonly char _delimiter;
        private bool _inQuotedField = false;
        private bool _previousCharacterQuote = false;
        public List<String> Row; 
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public CsvReader(TextReader textReader, char delimiter=',', char quoteChar='"')
        {
            this._textReader = textReader;
            this._delimiter = delimiter;
            this._quoteChar = quoteChar;
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
            throw new NotSupportedException();
        }

        public object Current
        {
            get { throw new NotImplementedException(); }
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
