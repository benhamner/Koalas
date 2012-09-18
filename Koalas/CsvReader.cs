using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Koalas {
    public class CsvReader : IEnumerator<List<String>>, IEnumerable<List<String>> {
        private readonly Stream _stream;
        private StreamReader _streamReader;
        private readonly int _quote;
        private readonly int _delimiter;
        private const int NewLine = '\n';
        private const int CarriageReturn = '\r';
        private CsvReaderState _csvReaderState = CsvReaderState.StartOfRow;
        public List<String> Row;
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private enum CsvReaderState {
            StartOfRow,
            StartOfField,
            InUnquotedField,
            InQuotedField,
            QuoteInQuotedField,
            EndOfField,
            EndOfRow,
            EndOfStream,
            Error
        }

        public CsvReader(Stream stream, char delimiter = ',', char quote = '"') {
            _stream = stream;
            _streamReader = new StreamReader(_stream);
            _delimiter = delimiter;
            _quote = quote;
        }

        public static CsvReader FromString(String data, char delimiter = ',', char quote = '"') {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            stream.Position = 0;
            return new CsvReader(stream, delimiter, quote);
        }

        bool IEnumerator.MoveNext() {
            var t = _streamReader.Read();
            if (t < 0)
                return false;

            Row = new List<string>();
            _stringBuilder.Clear();

            while (_csvReaderState != CsvReaderState.EndOfStream) {
                switch (_csvReaderState) {
                    case CsvReaderState.StartOfRow:
                        if (t == NewLine || t == CarriageReturn)
                            t = _streamReader.Read();
                        else if (t < 0)
                            _csvReaderState = CsvReaderState.EndOfStream;
                        else
                            _csvReaderState = CsvReaderState.StartOfField;
                        break;
                    case CsvReaderState.StartOfField:
                        if (t == _quote) {
                            t = _streamReader.Read();
                            _csvReaderState = CsvReaderState.InQuotedField;
                        }
                        else
                            _csvReaderState = CsvReaderState.InUnquotedField;
                        break;
                    case CsvReaderState.InUnquotedField:
                        if (t == _delimiter || t == NewLine || t == CarriageReturn || t < 0)
                            _csvReaderState = CsvReaderState.EndOfField;
                        else {
                            _stringBuilder.Append((char) t);
                            t = _streamReader.Read();
                        }
                        break;
                    case CsvReaderState.InQuotedField:
                        if (t == _quote) {
                            t = _streamReader.Read();
                            _csvReaderState = CsvReaderState.QuoteInQuotedField;
                        }
                        else if (t < 0)
                            _csvReaderState = CsvReaderState.Error;
                        else {
                            _stringBuilder.Append((char) t);
                            t = _streamReader.Read();
                        }
                        break;
                    case CsvReaderState.QuoteInQuotedField:
                        if (t == _quote) {
                            _stringBuilder.Append((char) t);
                            t = _streamReader.Read();
                            _csvReaderState = CsvReaderState.InQuotedField;
                        }
                        else if (t == _delimiter || t == CarriageReturn || t == NewLine || t < 0) {
                            _csvReaderState = CsvReaderState.EndOfField;
                        }
                        else {
                            _csvReaderState = CsvReaderState.Error;
                        }
                        break;
                    case CsvReaderState.EndOfField:
                        Row.Add(_stringBuilder.ToString());
                        _stringBuilder.Clear();
                        if (t == NewLine || t == CarriageReturn || t < 0)
                            _csvReaderState = CsvReaderState.EndOfRow;
                        else {
                            t = _streamReader.Read();
                            _csvReaderState = CsvReaderState.StartOfField;
                        }
                        break;
                    case CsvReaderState.EndOfRow:
                        _csvReaderState = t < 0 ? CsvReaderState.EndOfStream : CsvReaderState.StartOfRow;
                        return true;
                    case CsvReaderState.Error:
                        throw new Exception("CSV Parsing Error");
                }
            }
            return false;
        }

        public void Reset() {
            _stream.Position = 0;
            _streamReader = new StreamReader(_stream);
        }

        public object Current {
            get { return Row; }
        }

        List<string> IEnumerator<List<string>>.Current {
            get { return Row; }
        }

        public void Dispose() {
            Reset();
        }

        public IEnumerator<List<string>> GetEnumerator() {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}