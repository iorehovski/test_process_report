using CallCenterReport.Models;

namespace CallCenterReport.Reports;

public static class OperatorStatesService
{
    public static void GenerateOperatorStatesReport(List<CallSession> sessions)
    {
        var operatorStates = ProcessOperatorStates(sessions);

        OutputOperatorsStatesToConsole(operatorStates);
    }

    private static Dictionary<string, SortedDictionary<string, int>> ProcessOperatorStates(List<CallSession> sessions)
    {
        var operatorToStateToMinutes = new Dictionary<string, SortedDictionary<string, int>>();

        foreach (var session in sessions)
        {
            if (operatorToStateToMinutes.ContainsKey(session.Operator))
            {
                if (operatorToStateToMinutes[session.Operator].ContainsKey(session.State))
                {
                    operatorToStateToMinutes[session.Operator][session.State] += session.Duration;
                }
                else
                {
                    operatorToStateToMinutes[session.Operator][session.State] = session.Duration;
                }
            }
            else
            {
                operatorToStateToMinutes[session.Operator] = new SortedDictionary<string, int>
                    { { session.State, session.Duration } };
            }
        }

        return operatorToStateToMinutes;
    }

    private static void OutputOperatorsStatesToConsole(
        Dictionary<string, SortedDictionary<string, int>> operatorToStateToMinutes)
    {
        foreach (var operatorValue in operatorToStateToMinutes)
        {
            Console.Write($"{operatorValue.Key} ");
            foreach (var operatorStateToMinutes in operatorValue.Value)
            {
                Console.Write(
                    $"{Math.Round(operatorStateToMinutes.Value / 60.0f, 2)} мин. в состоянии {operatorStateToMinutes.Key} ");
            }

            Console.Write("\n");
        }
    }
}