using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Coding4Fun.Phone.Controls.Toolkit.Common
{
    public class TimeSpanFormat
    {
        public static string Format(TimeSpan time, string format)
        {
            var returnValue = new StringBuilder(format);
            var groupFormats = Regex.Matches(format, @"{0:[^}]*}");

            var formatLists = groupFormats.Cast<Match>().ToList();
            formatLists.Reverse();
            
            foreach (Match formats in formatLists)
            {
                var sb = new StringBuilder(formats.Value);

                var smallFormatRegex = Regex.Matches(formats.Value, "[d]{1,2}|[h]{1,2}|[m]{1,2}|[s]{1,2}|[f]{1,7}|[F]{1,7}");
                var smallFormatLists = smallFormatRegex.Cast<Match>().ToList();
                smallFormatLists.Reverse();

                foreach (Match match in smallFormatLists)
                {
                    switch (match.Value[0])
                    {
                        case 'F':
                            sb.Replace(
                                match.Value, 
                                (time.Milliseconds/1000.0).ToString(match.Value.Replace("F", "0")),
                                match.Index,
                                match.Length);
                            break;
                        case 'f':
                            sb.Replace(
                                match.Value, 
                                (time.Milliseconds/1000).ToString(match.Value.Replace("f", "0")),
                                match.Index, 
                                match.Length);
                            break;
                        case 'd':
                            sb.Replace(match.Value, time.Days.ToString("00"), match.Index, match.Length);
                            break;
                        case 'h':
                            sb.Replace(match.Value, time.Hours.ToString("00"), match.Index, match.Length);
                            break;
                        case 'm':
                            sb.Replace(match.Value, time.Minutes.ToString("00"), match.Index, match.Length);
                            break;
                        case 's':
                            sb.Replace(match.Value, time.Seconds.ToString("00"), match.Index, match.Length);
                            break;
                    }
                }

                sb.Remove(0, 3);
                sb.Remove(sb.Length - 1, 1);

                returnValue.Replace(formats.Value, sb.ToString(), formats.Index, formats.Length);
            }

            return returnValue.ToString();
        }
    }
}
