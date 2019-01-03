using System.Collections.Generic;

namespace ID3.src.ID3.v2
{

    public static class IO
    {
        public static void WriteFile(string path, string data)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding(1251).GetBytes(data);
            using (System.IO.FileStream fileStream = new System.IO.FileStream(
                    path,
                    System.IO.FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public static string ReadFile(string path)
            => new System.IO.StreamReader(path, System.Text.Encoding.Default).ReadToEnd();

        public static string GenerateInput(int numberOfData)
        {
            string data = "",
                   type = "", gunType = "", answer = "",
                   angle = "", distance = "", score = "";

            System.Random rand = new System.Random();

            data += "Type GunType Angle Distance Score Answer" + "\r\n";

            for (int i = 1; i < numberOfData; i++)
            {
                type = "Стрелок" + rand.Next(1, 6);
                gunType = rand.Next(0, 10) > 5 ? "Нарезной" : "Гладкий";
                angle = rand.Next(1, 1000) < 300 ? "<60" :
                        rand.Next(1, 1000) < 600 ? "60<=x<85" : ">85";//rand.Next(50, 95);
                distance = rand.Next(1, 1000) < 300 ? "100" :
                           rand.Next(1, 1000) < 600 ? "150" : "200";
                score = rand.Next(1, 1000) < 300 ? "<600" :
                        rand.Next(1, 1000) < 600 ? "600<=x<=700" : ">700";//rand.Next(500, 800);

                if (score == "<600")
                {
                    answer = "No";
                }
                else if (
                        (score == ">700" && distance == "100")
                            ||
                        (score == ">700" && distance == "150")
                            ||
                        (gunType == "Нарезной" && distance == "100")
                            ||
                        (gunType == "Нарезной" && distance == "150")
                            ||
                        (gunType == "Нарезной" && angle == ">85" && distance != "200")
                            ||
                        (gunType == "Нарезной" && angle == "<60" && distance != "200")
                            ||
                        (gunType == "Нарезной" && angle == "60<=x<85" && distance != "200")
                            ||
                        (gunType == "Гладкий" && distance == "100")
                            ||
                        (gunType == "Гладкий" && angle == ">85")

                        )
                {
                    answer = "Yes";
                }
                else if (
                         (gunType == "Нарезной" && distance == "150")
                            ||
                         (gunType == "Нарезной" && distance == "200")
                            ||
                         (gunType == "Гладкий" && angle == "60<=x<85")
                            ||
                         (gunType == "Гладкий" && angle == "<60")
                            ||
                         (angle == "60<=x<85" && distance == "150")
                            ||
                         (angle == "60<=x<85" && distance == "200")
                            ||
                         (angle != "<60" && distance == "200")
                        )
                {
                    answer = "No";
                }
                else
                {
                    System.Console.WriteLine("неучтенный");
                    answer = "Yes";
                }
                data += $"{type} {gunType} {angle} {distance} {score} {answer}" + ((i != numberOfData - 1) ? "\r\n" : "");
            }

            return data;
        }
    }

    public class Input
    {
        public string[][] m_Data { get; set; }

        public Input() { }
        
        public Input(string str)
        {
            str = str.Replace("\r\n", "|");
            string[] strArray = str.Split(new char[] { '|' });
            m_Data = new string[strArray.Length][];
            
            for (int i = 0; i < strArray.Length; i++)
            {
                m_Data[i] = strArray[i].Split(' ');
            }
        }

        public string[] GetAttributeColumn(int attributeNumber)
        {
            string[] result = new string[m_Data.Length];

            for (int i = 0; i < m_Data.Length; i++)
                result[i] = m_Data[i][attributeNumber];

            return result;
        }
        
        public Input GetSubset(string attribute,
                               string attributeValue)
        {
            int attributeNumber;

            for (attributeNumber = 1;
                 attributeNumber < m_Data[0].Length - 1;
                 attributeNumber++)
            {
                if (m_Data[0][attributeNumber] == attribute)
                    break;
            }

            var data = new List<string[]>();
            data.Add(m_Data[0]);

            for (int i = 1; i < m_Data.Length; i++)
            {
                if (m_Data[i][attributeNumber] == attributeValue)
                    data.Add(m_Data[i]);
            }

            var subset = new Input();
            subset.m_Data = data.ToArray();

            return subset;
        }

    }

    public static class ConfigInfo
    {
        //File = 0; Mode = 1; NumberOfIterations = 2; 
        //CreateNewFile = 3; CreateXLFile = 4;
        static public string[] Readed { get; set; }

        public static void SetData(string input)
        {
            int index = 0, parametrsCounter = 0;
            while ((index = input.IndexOf(":", index)) != -1)
            {
                ++parametrsCounter;
                ++index;
            }
            
            Readed = new string[parametrsCounter];
            int beginIndex = 0;
            int endIndex   = 0;

            for (int p = 0; p < parametrsCounter; p++)
            {
                beginIndex = input.IndexOf(":", beginIndex)+1;
                endIndex   = input.IndexOf("\r", beginIndex);

                for (int i = beginIndex; i < endIndex; i++)
                {
                    Readed[p] += input[i];
                }
            }

        }

        public static string String()
        {
            string toString = "";
            for (int i = 0; i < Readed.Length; i++)
            {
                toString += Readed[i] + " ";
            }

            return $"{toString}";
        }


    }
}
