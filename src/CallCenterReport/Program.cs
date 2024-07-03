using System.Text;
using CallCenterReport.Reports;
using CallCenterReport.DataProviders;

Console.OutputEncoding = Encoding.UTF8;

var filePath = Environment.GetCommandLineArgs()[1];

var sessions = CsvReader.GetSessionsFrom(filePath);

await MaxActiveSessionsService.GenerateMaxActiveSessionsReport(sessions);
OperatorStatesService.GenerateOperatorStatesReport(sessions);
