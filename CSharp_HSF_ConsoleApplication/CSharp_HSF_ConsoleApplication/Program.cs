using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_HSF_ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pg = new Program();

            Console.WriteLine("Hello World!");

            // Keep the console window open in debug mode.
            
            DataSet dataSet = new DataSet();
            //dataSet.ReadXml("E:\\VisualStudio2015Workspace\\CSharp_HSF_ConsoleApplication\\TestXML.xml", XmlReadMode.IgnoreSchema);
            dataSet.ReadXml("E:\\VisualStudio2015Workspace\\CSharp_HSF_ConsoleApplication\\TestXML.xml");
            pg.GetEntityDataCells("session-12121asasa32121b213m2b1m3",dataSet);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            
        }

        // Hyperion.HSF.WebServices.GatewayWrapper.Gateway
        public void GetEntityDataCells(string sessionID, DataSet dataCellLocators)
        {
            DataSet dataSet = null;
            DataTable dataTable = dataCellLocators.Tables["DataCells"];
            string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n";
            text += "<GatewayInput SessionID=\"";
            text += sessionID;
            text += "\">\n<Servers>\n<Server Name=\"";
            text = (text ?? "");
            text += "\">\n<Databases>\n<Database Name=\"";
            text = (text ?? "");
            text += "\">\n<Entities>\n<Entity Name=\"";
            text = (text ?? "");
            text += "\">\n<DataCells>\n<GetDataCells />\n";
            
            DataRow[] array = dataTable.Select(null, null, DataViewRowState.CurrentRows);
            DataRow[] array2 = array;
            Console.WriteLine(array.Length);
            for (int i = 0; i < array2.Length; i++)
            {
                DataRow dataRow = array2[i];
                text += "<DataCell ";
                text += "Account=\"";
                text += dataRow["Account"];
                text += "\" Scenario=\"";
                text += dataRow["Scenario"];
                text += "\" Time=\"";
                text += dataRow["Time"];
                text += "\" Measure=\"";
                text += dataRow["Measure"];
                string text2 = (string)dataRow["CustomMembers"];
                if (text2 != "" && text2.Length > 2 && text2[0] == '[' && text2[text2.Length - 1] == ']')
                {
                    string[] array3 = text2.Substring(1, text2.Length - 2).Split(new string[]
                    {
                "]["
                    }, StringSplitOptions.None);
                    int num = 0;
                    string[] array4 = array3;
                    for (int j = 0; j < array4.Length; j++)
                    {
                        string str = array4[j];
                        object obj = text;
                        text = string.Concat(new object[]
                        {
                    obj,
                    "\" CustomMember",
                    num++,
                    "=\""
                        });
                        text += str;
                    }
                }
                text += "\" />\n";
            }
            text += "</DataCells>\n</Entity>\n</Entities>\n";
            text += "</Database>\n</Databases>\n</Server>\n</Servers>\n</GatewayInput>";
            Console.WriteLine(text);
        }

    }
}
