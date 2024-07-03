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

                sessions.Add(session);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return sessions;
    }

    private static CallSession MapFileStringToCallSession(string fileLine)
    {
        string[] values = fileLine.Split(';', ',');

        return new CallSession
        {
            SessionStart = DateTime.ParseExact(values[0], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            SessionEnd = DateTime.ParseExact(values[1], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            Project = values[2],
            Operator = values[3],
            State = values[4],
            Duration = int.Parse(values[5])
        };
    }
}