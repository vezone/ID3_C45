using System.Collections.Generic;

using static ID3.src.ID3.v2.Additional;

namespace ID3.src.ID3.v2
{
    public class Tree
    {
        public Leaf Root { get; set; }
    

        public void Build(Input i)
        {
            Root = Build(i, null);
        }
        
        private Leaf Build(Input input, Node parentLink)
        {
            var leaf = new Leaf { Parent = parentLink };

            if (parentLink != null && parentLink.Entropy == 0)
            {
                Log("God damn!");
                return leaf;
            }

            leaf.Input = input;
            leaf.SetClassifier();

            foreach (Node node in leaf.Classifier.Nodes)
            {
                Input subset = input
                               .GetSubset(leaf.Classifier.AttributeName,
                                          node.Name);
                Leaf childLeaf = Build(subset, node);
                leaf.Children.Add(childLeaf);
            }

            return leaf;
        }

        public void BuildID3(Input input, int numberOfIterations)
        {
            Root = new Leaf { Parent = null};
            Root.Input = input;
            Root.SetChildren();

            List<Leaf> children1 = new List<Leaf>();
            List<Leaf> children2 = new List<Leaf>();

            foreach (var child in Root.Children)
                children1.Add(child);
            
            int NumberOfAnswers = 0;
            List<int> LayersAnswers = new List<int>(10);

            while (children1.Count > 0 &&
                   LayersAnswers.Count < numberOfIterations)
            {
                foreach (var child in children1)
                {
                    if (!child.IsAnswer)
                    {
                        child.SetChildren();
                        foreach (var child2 in child.Children)
                        {
                            children2.Add(child2);
                        }
                    }
                    else
                    {
                        ++NumberOfAnswers;
                    }
                }

                LayersAnswers.Add(NumberOfAnswers);
                NumberOfAnswers = 0;
                Log($"iterations: {LayersAnswers.Count}");

                children1.Clear();
                children1 = new List<Leaf>(children2);
                children2.Clear();
            }

            for (int i = 0; i < LayersAnswers.Count;i++)
            {
                Log($"Number of answers in layer {i}: {LayersAnswers[i]}");
            }

        }
        

    }
}
