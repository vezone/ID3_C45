#define Run

using ID3.src.ID3.v2;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

using static ID3.src.ID3.v2.Additional;

namespace ID3
{
    class EntryPoint
    {

        static void Main(string[] args)
        {
            //System.Diagnostics.Process.Start(@"delete.bat");

            string outputFileName = @"graph.dot";

            string inputData = IO.ReadFile(@"config\config.txt");
            ConfigInfo.SetData(inputData);
            Log(ConfigInfo.String());

            string path = "";
            if (ConfigInfo.Readed[3] == "true")
            {
                path = @"..\..\..\Additional\" + $"data{ConfigInfo.Readed[4]}.txt";
                IO.WriteFile(path, IO.GenerateInput(
                        System.Int32.Parse(ConfigInfo.Readed[4])));
            }
            else
            {
                path = @"..\..\..\Additional\" + ConfigInfo.Readed[0];
            }

            var input = new Input(IO.ReadFile(path));

            if (ConfigInfo.Readed[4] == "true")
            {
                XL.Create(input.m_Data);
            }

            var tree = new Tree();
            if (ConfigInfo.Readed[1] == "Recursive")
            {
                Thread thread = new Thread(() => tree.Build(input), 150 * 1024 * 1024);
                thread.Start();
                thread.Join();
            }
            else
            {
                tree.BuildID3(input, System.Int32.Parse(ConfigInfo.Readed[2]));
            }
            
            var graph = new TreeGraph(tree);
            graph.BuildGraph();
            graph.DrawGraph(outputFileName);

            if (System.IO.File.Exists("ID3.png"))
            {
                System.Diagnostics.Process.Start(@"get_image_2.bat");
            }
            else
            {
                System.Diagnostics.Process.Start(@"get_image_1.bat");
            }

            System.Console.Read();
        }
    }
}
