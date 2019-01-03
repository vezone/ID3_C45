using System;

namespace ID3.src.ID3.v2
{
    public class Node
    {
        public Node(string name) { Name = name; }

        public string Name      { get; }
        public int    Positives { get; set; }
        public int    Negatives { get; set; }
        public int    Sum       => Positives + Negatives;
        public double Entropy   => Additional.Entropy(Positives, Negatives);
    }
}
