using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class DataFrame
    {
        private List<List<String>> series;

        public DataFrame (List<List<String>> series)
        {
            this.series = series;
        }

        public List<String> this [int seriesIndex]
        {
            get {return series[seriesIndex];}
        }

        public static DataFrame FromCsvData(String data, char delimiter=',', char quoteChar='"')
        {
            var series = new List<List<string>>();
            var nColumns = GetColumnCount(data, delimiter, quoteChar);
            for (int i = 0; i < nColumns; i++ )
                series.Add(new List<string>());

            var sb = new StringBuilder();
            var column = 0;
            var inQuotedField = false;
            var previousCharacterQuote = false;
            foreach (char t in data)
            {
                if (inQuotedField)
                {
                    if (t==quoteChar)
                    {
                        inQuotedField = false;
                        previousCharacterQuote = true;
                    }
                    else
                    {
                        previousCharacterQuote = false;
                        sb.Append(t);
                    }
                }
                else
                {

                    if (t == delimiter)
                    {
                        series[column].Add(sb.ToString());
                        sb.Clear();
                        column++;
                    } else if (t == '\r' || t== '\n')
                    {
                        series[column].Add(sb.ToString());
                        sb.Clear();
                        column = 0;
                    } else if (t == quoteChar)
                    {
                        inQuotedField = true;
                        if (previousCharacterQuote)
                            sb.Append(t);
                        else
                            previousCharacterQuote = true;
                    } else
                    {
                        sb.Append(t);
                    }
                }
            }
            return new DataFrame(series);
        }

        public static int GetColumnCount(String data, char delimiter=',', char quoteChar='"')
        {
            var inQuotedField = false;
            var columnCount = 1;

            foreach (char t in data)
            {
                if (inQuotedField==false)
                {
                    if (t == delimiter)
                        columnCount++;
                    else if (t == '\n' || t == '\r')
                        break;
                    else if (t == quoteChar)
                        inQuotedField = true;
                } else
                {
                    if (t == quoteChar)
                        inQuotedField = false;
                }
            }

            return columnCount;
        }
    }
}