using System.Globalization;
using CallCenterReport.Models;

namespace CallCenterReport.DataProviders;

public static class CsvReader
{
    public static List<CallSession> GetSessionsFrom(string filePath)
    {
        var sessions = new List<CallSession>();
        try
        {
            using StreamReader reader = new StreamReader(filePath);

            string csvLine;
            reader.ReadLine(); // skip first line
            while ((csvLine = reader.ReadLine()) != null)
            {
                var session = MapFileStringToCallSession(csvLine);

                sessions.AddRange(session);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return sessions;
    }

    private static IEnumerable<CallSession> MapFileStringToCallSession(string fileLine)
    {
        string[] values = fileLine.Split(';', ',');

        var sessionStart = DateTime.ParseExact(values[0], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        var sessionEnd = DateTime.ParseExact(values[1], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        // If session takes 2 days
        if (sessionEnd.Day == sessionStart.Day + 1)
        {
            return new[]
            {
                MapLineItemsToCallSession(sessionStart, sessionStart.Date.AddDays(1).AddTicks(-1)),
                MapLineItemsToCallSession(sessionStart.Date.AddDays(1), sessionEnd, 0)
            };
        }

        return new[]
        {
            MapLineItemsToCallSession(sessionStart, sessionEnd)
        };

        CallSession MapLineItemsToCallSession(DateTime sessionStart, DateTime sessionEnd, int? duration = null) => new()
        {
            SessionStart = sessionStart,
            SessionEnd = sessionEnd,
            Project = values[2],
            Operator = values[3],
            State = values[4],
            Duration = duration ?? int.Parse(values[5])
        };
    }
}