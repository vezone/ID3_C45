using System.Collections.Generic;
using System.Linq;

namespace ID3.src.ID3.v2
{
    public class Classifier : System.IComparable<Classifier>
    {
        private readonly string[] m_AttributeColumn;
        private readonly string[] m_Answers; 

        public Classifier(string[] attributeColumn, 
                          string[] answers)              
        {
            AttributeName = attributeColumn[0];
            Entropy       = GetEntropy(answers);

            m_Answers         = answers;
            m_AttributeColumn = attributeColumn;
        }


        public string AttributeName { get; }
        public double Entropy { get; }
        public List<Node> Nodes { get; set; }
        public double? Gain
        {
            get
            {
                if (Nodes == null)
                    return null;

                double gain = Entropy;
                
                int globalNodeSum = 0;

                foreach (var attribute in Nodes)
                {
                    globalNodeSum += attribute.Sum;
                }

                foreach (var attribute in Nodes)
                {
                    gain -= (((double)attribute.Sum) / globalNodeSum)
                        * attribute.Entropy;
                    //Log($"{Name}:\n" +
                    //    $"{((double)attribute.Sum)} / {globalNodeSum} * {attribute.Entropy}" 
                    //    + "=" +
                    //   ((((double)attribute.Sum) / globalNodeSum)
                    //    * attribute.Entropy));
                }

                return gain;
            }
        }

        public double? GainRatio
        {
            get
            {
                if (Nodes == null)
                    return null;

                double splitinfo = GetSplitInfo();

                return System.Math.Abs((double)(Gain / splitinfo));
            }
        }

        public int CompareTo(Classifier other)
        {
            if (ConfigInfo.Readed[5] == "C45")
            {
                if (GainRatio > other.GainRatio)
                    return 1;
                else if (GainRatio < other.GainRatio)
                    return -1;
                else
                    return 0;
            }
            else
            {
                if (Gain > other.Gain)
                    return 1;
                else if (Gain < other.Gain)
                    return -1;
                else
                    return 0;
            }
        }
        
        public void BuildNodes()
        {
            Nodes = new List<Node>();

            for (int i = 1; i < m_AttributeColumn.Length; i++)
            {
                string attributeValue = m_AttributeColumn[i];
                string answer         = m_Answers[i];

                Node attributeValueNode = Nodes
                    .Where(x => x.Name == attributeValue)
                    .FirstOrDefault();

                if (attributeValueNode == null)
                {
                    attributeValueNode = new Node(attributeValue);
                    Nodes.Add(attributeValueNode);
                }

                if (answer == "Yes")
                    attributeValueNode.Positives++;
                else
                    attributeValueNode.Negatives++;
            }
        }

        private double GetEntropy(string[] answers)
        {
            int positives = 0;
            int negatives = 0;

            for (int i = 1; i < answers.Length; i++)
            {
                if (answers[i].Contains("Yes"))
                    positives++;
                else
                    negatives++;
            }
            return Additional.Entropy(positives, negatives);
        }

        private double GetSplitInfo()
        {
            double splitinfo = 0.0, 
                globalNodeSum = 0.0;

            foreach (var attribute in Nodes)
            {
                globalNodeSum += attribute.Sum;
            }

            double summary = 0.0;

            foreach (var attribute in Nodes)
            {
                summary = ((double)attribute.Sum) / globalNodeSum;
                splitinfo += summary * System.Math.Log(summary, 2);
            }

            return splitinfo;
        }

    }
}
