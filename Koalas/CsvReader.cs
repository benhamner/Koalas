using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Koalas {
    public class CsvReader : IEnumerable<List<String>> {
        private readonly Stream _stream;
        private StreamReader _streamReader;
        private const int NewLine = '\n';
        private const int CarriageReturn = '\r';
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        public readonly CsvSchema Schema;

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

        public CsvReader(Stream stream, CsvSchema schema) {
            _stream = stream;
            _streamReader = new StreamReader(_stream);
            Schema = schema;
        }

        public static CsvReader FromString(String data) {
            return new CsvReader(StringToStream(data), new CsvSchema());
        }

        public static CsvReader FromString(String data, CsvSchema schema) {
            return new CsvReader(StringToStream(data), schema);
        }

        public static Stream StringToStream(String data) {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public IEnumerator<List<string>> GetEnumerator() {
            var t = _streamReader.Read();
            if (t < 0)
                yield break;

            var row = new List<String> ();
            var csvReaderState = CsvReaderState.StartOfRow;
            _stringBuilder.Clear();

            while (csvReaderState != CsvReaderState.EndOfStream)
            {
                switch (csvReaderState)
                {
                    case CsvReaderState.StartOfRow:
                        if (t == NewLine || t == CarriageReturn) {
                            t = _streamReader.Read();
                            row = new List<String>();
                        }
                        else if (t < 0)
                            csvReaderState = CsvReaderState.EndOfStream;
                        else
                            csvReaderState = CsvReaderState.StartOfField;
                        break;
                    case CsvReaderState.StartOfField:
                        if (t == Schema.Quote)
                        {
                            t = _streamReader.Read();
                            csvReaderState = CsvReaderState.InQuotedField;
                        }
                        else
                            csvReaderState = CsvReaderState.InUnquotedField;
                        break;
                    case CsvReaderState.InUnquotedField:
                        if (t == Schema.Delimiter || t == NewLine || t == CarriageReturn || t < 0)
                            csvReaderState = CsvReaderState.EndOfField;
                        else
                        {
                            _stringBuilder.Append((char)t);
                            t = _streamReader.Read();
                        }
                        break;
                    case CsvReaderState.InQuotedField:
                        if (t == Schema.Quote)
                        {
                            t = _streamReader.Read();
                            csvReaderState = CsvReaderState.QuoteInQuotedField;
                        }
                        else if (t < 0)
                            csvReaderState = CsvReaderState.Error;
                        else
                        {
                            _stringBuilder.Append((char)t);
                            t = _streamReader.Read();
                        }
                        break;
                    case CsvReaderState.QuoteInQuotedField:
                        if (t == Schema.Quote)
                        {
                            _stringBuilder.Append((char)t);
                            t = _streamReader.Read();
                            csvReaderState = CsvReaderState.InQuotedField;
                        }
                        else if (t == Schema.Delimiter || t == CarriageReturn || t == NewLine || t < 0)
                        {
                            csvReaderState = CsvReaderState.EndOfField;
                        }
                        else
                        {
                            csvReaderState = CsvReaderState.Error;
                        }
                        break;
                    case CsvReaderState.EndOfField:
                        row.Add(_stringBuilder.ToString());
                        _stringBuilder.Clear();
                        if (t == NewLine || t == CarriageReturn || t < 0)
                            csvReaderState = CsvReaderState.EndOfRow;
                        else
                        {
                            t = _streamReader.Read();
                            csvReaderState = CsvReaderState.StartOfField;
                        }
                        break;
                    case CsvReaderState.EndOfRow:
                        csvReaderState = t < 0 ? CsvReaderState.EndOfStream : CsvReaderState.StartOfRow;
                        yield return row;
                        break;
                    case CsvReaderState.Error:
                        throw new Exception("CSV Parsing Error");
                }
            }
            _stream.Position = 0;
            _streamReader = new StreamReader(_stream);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}