using CallCenterReport.Models;

namespace CallCenterReport.Reports;

public static class MaxActiveSessionsService
{
    public static async Task GenerateMaxActiveSessionsReport(List<CallSession> sessions)
    {
        var maxActiveSessions = (await ProcessSessions(sessions)).Select(x => x.Result);

        OutputMaxActiveSessionToConsole(maxActiveSessions);
    }

    private static async Task<IEnumerable<Task<string>>> ProcessSessions(List<CallSession> sessions)
    {
        var sessionsBucket = SplitSessionsIntoBucketsByDay(sessions);

        var sessionTasks = new List<Task<string>>();

        foreach(var bucket in sessionsBucket)
        {
            sessionTasks.Add(Task.Run(() => FindMaxActiveSessions(bucket.Value)));
        }

        await Task.WhenAll(sessionTasks);

        return sessionTasks;
    }

    private static Dictionary<int, List<CallSession>> SplitSessionsIntoBucketsByDay(List<CallSession> sessions)
    {
        var sessionsByDay = new Dictionary<int, List<CallSession>>();

        foreach(var session in sessions)
        {
            if (sessionsByDay.ContainsKey(session.SessionStart.Day))
            {
                sessionsByDay[session.SessionStart.Day].Add(session);
            }
            else
            {
                sessionsByDay[session.SessionStart.Day] = new List<CallSession> { session };
            }
        }

        return sessionsByDay;
    }

    private static string FindMaxActiveSessions(List<CallSession> sessions)
    {
        var points = new List<Tuple<DateTime, int>>();

        foreach(var session in sessions)
        {
            points.Add(Tuple.Create(session.SessionStart, 1));
            points.Add(Tuple.Create(session.SessionEnd, -1));
        }

        points.Sort((a, b) => a.Item1 == b.Item1 
            ? a.Item2.CompareTo(b.Item2) 
            : a.Item1.CompareTo(b.Item1));

        int maxActiveSessions = 0;
        int currentActiveSessions = 0;
        foreach(var point in points)
        {
            currentActiveSessions += point.Item2;

            if(currentActiveSessions > maxActiveSessions)
            {
                maxActiveSessions = currentActiveSessions;
            }
        }

        return $"{sessions.First().SessionStart.ToString("dd.MM.yyyy")} {maxActiveSessions}";
    }

    private static void OutputMaxActiveSessionToConsole(IEnumerable<string> maxActiveSessions)
    {
        foreach(var activeSessionResult in maxActiveSessions.OrderBy(x => x))
        {
            Console.WriteLine(activeSessionResult);
        }
    }
}