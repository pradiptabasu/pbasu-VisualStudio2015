using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testConsoleApplication1.HSFEntityWebServices;
using testConsoleApplication1.HSFWebServices;
using System.Web.Services.Protocols;

namespace testConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String database="";
            String entity = "";
            try
            {
                Console.WriteLine("Hello World!");
                // Create general web service link.
                HSFWebService hsfWebService = new HSFWebService();
                HSFEntityWebService hsfEntityWebService = new HSFEntityWebService();

                //call with hsfEntityWebService1 to forward to proxy proxyTrace012 to capture request xml
                HSFEntityWebService hsfEntityWebService1 = new HSFEntityWebService("abc");
                // Create session with user/password.
                String sessionID = hsfWebService.CreateSession("admin", "Welcome1");
                if (sessionID == "") throw new Exception("Can not create a valid session.");
                Console.WriteLine("Session ID : " + sessionID);

                hsfWebService.OpenServer(sessionID, "HSFServer");
                string[] databases = hsfWebService.EnumDatabases(sessionID);
                if (databases.Length < 1) throw new Exception("There are no databases in the current server.");

                // Open database.
                if (database == null || database == "")
                    database = databases[0];
                Console.WriteLine("Database : " + database);
                hsfWebService.OpenDatabase(sessionID, database);


                System.Data.DataSet entities = hsfWebService.EnumEntities(sessionID, "", "All");
                if (entities == null || entities.Tables["Entities"] == null) throw new Exception("There are no entities in this database");
                System.Data.DataRow[] entityRows = entities.Tables["Entities"].Select();
                if (entityRows.Length < 1) throw new Exception("There are no entities in this database");
                entity = (string)entityRows[1]["ID"];
                Console.WriteLine("Entity : " + entity);


                // Open entity.
                hsfEntityWebService.OpenEntity(sessionID, entity, false);

                // Mark entity as opened.
                Boolean entityOpened = true;

                // Get list of time periods.
                System.Data.DataSet timePeriods = hsfEntityWebService.EnumTimePeriods(sessionID);
                if (timePeriods == null || timePeriods.Tables["TimePeriods"] == null) throw new Exception("There are no time periods in this entity");
                System.Data.DataRow[] timePeriodRows = timePeriods.Tables["TimePeriods"].Select();
                if (timePeriodRows.Length < 1) throw new Exception("here are no time periods in this entity");
                int timeCols = timePeriodRows.Length;

                System.Data.DataSet scenarios = hsfEntityWebService.EnumScenarios(sessionID);
                if (scenarios == null || scenarios.Tables["Scenarios"] == null) throw new Exception("There are no scenarios in this entity");
                System.Data.DataRow[] scenarioRows = scenarios.Tables["Scenarios"].Select();
                if (scenarioRows.Length < 1) throw new Exception("here are no scenarios in this entity");

                System.Data.DataSet accounts = hsfEntityWebService.EnumAccounts(sessionID);
                if (accounts == null || accounts.Tables["Accounts"] == null) throw new Exception("There are no accounts in this entity");
                System.Data.DataTable accountTable = accounts.Tables["Accounts"];
                System.Data.DataRow[] accountRows = accountTable.Select();
                if (accountRows.Length < 1) throw new Exception("here are no accounts in this entity");

                System.Data.DataSet dataCellsOut = null;
                // Create data set for GetCellData call.
                System.Data.DataSet dataCellsIn = new System.Data.DataSet();
                System.Data.DataTable dataCellsInTable = dataCellsIn.Tables.Add("DataCells");

                // Add columns to the table.
                dataCellsInTable.Columns.Add("Entity", typeof(string));
                dataCellsInTable.Columns.Add("Account", typeof(string));
                dataCellsInTable.Columns.Add("Time", typeof(string));
                dataCellsInTable.Columns.Add("Scenario", typeof(string));
                dataCellsInTable.Columns.Add("Measure", typeof(string));
                dataCellsInTable.Columns.Add("CustomMembers", typeof(string));

                for (int j = 0; j < timeCols; j++)
                {
                    System.Data.DataRow dataRow = dataCellsInTable.NewRow();
                    dataRow["Entity"] = entity;
                    dataRow["Account"] = "100.00.000";
                    dataRow["Measure"] = "Output";
                    dataRow["Scenario"] = "Base";
                    dataRow["Time"] = timePeriodRows[j]["ID"];
                    dataRow["CustomMembers"] = "";
                    dataCellsInTable.Rows.Add(dataRow);
                }


                Console.WriteLine("Request Payload");
                dataCellsIn.WriteXml("E:\\VisualStudio2015Workspace\\CSharp_HSF_ConsoleApplication\\GetEntityDataCells.xml", System.Data.XmlWriteMode.WriteSchema);

                Console.WriteLine("Press any key to proceed.");
                Console.ReadKey();

                // Call GetDataCells.
                dataCellsOut = hsfEntityWebService.GetEntityDataCells(sessionID, dataCellsIn);
                if (dataCellsOut == null) throw new Exception("DataCells data set not found.");

                System.Data.DataTable dataCellsOutTable = dataCellsOut.Tables["DataCells"];
                System.Data.DataRow[] valueRows = dataCellsOutTable.Select();
                System.Data.DataRow[] valueRows2 = valueRows;
                Console.WriteLine(valueRows.Length);
                for (int i = 0; i < valueRows2.Length; i++)
                {
                    System.Data.DataRow dataRow = valueRows2[i];
                    String text = "<DataCell ";
                    text += "Account=\"";
                    text += dataRow["Account"];
                    text += "\" Scenario=\"";
                    text += dataRow["Scenario"];
                    text += "\" Time=\"";
                    text += dataRow["Time"];
                    text += "\" Measure=\"";
                    text += dataRow["Measure"];
                    text += "\" Value=\"";
                    text += dataRow["Value"];
                    Console.WriteLine(text);
                }

                Console.WriteLine("calling setentitydatacells");
                Console.WriteLine("Press any key to proceed.");
                Console.ReadKey();

                System.Data.DataSet dataCellsOutSet = null;
                System.Data.DataSet dataCellsInSet = new System.Data.DataSet();
                System.Data.DataTable dataCellsInTableSet = dataCellsInSet.Tables.Add("DataCells");

                // Add columns to the table.
                dataCellsInTableSet.Columns.Add("Entity", typeof(string));
                dataCellsInTableSet.Columns.Add("Account", typeof(string));
                dataCellsInTableSet.Columns.Add("Time", typeof(string));
                dataCellsInTableSet.Columns.Add("Scenario", typeof(string));
                dataCellsInTableSet.Columns.Add("Measure", typeof(string));
                dataCellsInTableSet.Columns.Add("Value", typeof(string));
                dataCellsInTableSet.Columns.Add("CustomMembers", typeof(string));

                for (int j = 0; j < 10; j++)
                {
                    System.Data.DataRow dataRowSet = dataCellsInTableSet.NewRow();
                    dataRowSet["Entity"] = entity;
                    dataRowSet["Account"] = "100.00.000";
                    dataRowSet["Measure"] = "Output";
                    dataRowSet["Scenario"] = "Base";
                    dataRowSet["Time"] = timePeriodRows[j]["ID"];
                    dataRowSet["Value"] = "100";
                    dataRowSet["CustomMembers"] = "";
                    dataCellsInTableSet.Rows.Add(dataRowSet);
                }

                dataCellsInSet.WriteXml("E:\\VisualStudio2015Workspace\\CSharp_HSF_ConsoleApplication\\SetEntityDataCells.xml", System.Data.XmlWriteMode.WriteSchema);
                // Call SetDataCells.

                //call with hsfEntityWebService1 to forward to proxy proxyTrace012 to capture request xml
                //dataCellsOutSet = hsfEntityWebService1.SetEntityDataCells(sessionID, dataCellsInSet,"csharpConsoleClient");

               // dataCellsOutSet = hsfEntityWebService.SetEntityDataCells(sessionID, dataCellsInSet, "csharpConsoleClient");
               // if (dataCellsOutSet == null) throw new Exception("DataCells data set not found.");

               // System.Data.DataTable dataCellsOutTableSet = dataCellsOutSet.Tables["DataCells"];
               // if (dataCellsOutTableSet == null) throw new Exception("DataCells table not found.");

                System.Data.DataSet dataCellsOutAccountList = hsfEntityWebService.EnumAccounts(sessionID);
                dataCellsOutAccountList.WriteXml("E:\\VisualStudio2015Workspace\\CSharp_HSF_ConsoleApplication\\EnumAccountsResponse.xml", System.Data.XmlWriteMode.WriteSchema);



            }
            catch (Exception e)
            {
                Console.WriteLine("*******************************************************");
                Console.WriteLine("IN EXCEPTION");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Press any key to proceed.");
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
