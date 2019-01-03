using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System;
using System.IO;

namespace ID3.src.ID3.v2
{
    public class TreeGraph
    {
        public TreeGraph(Tree tree)
        {
            this.tree = tree;

            graph = new AdjacencyGraph<Leaf, TaggedEdge<Leaf, string>>();
        }

        public void BuildGraph()
        {
            buildGraph(tree.Root);
        }

        public void DrawGraph(string outputFileName)
        {
            var graphviz =
                new GraphvizAlgorithm<Leaf, TaggedEdge<Leaf, string>>(graph);

            graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Box;
            graphviz.FormatVertex +=
                new FormatVertexEventHandler<Leaf>(graphviz_FormatVertex);
            graphviz.FormatEdge += (sender, e) => {
                e.EdgeFormatter.Label.Value = e.Edge.Tag;
            };
            graphviz.Generate(new FileDotEngine(), outputFileName);
        }

        private void graphviz_FormatVertex(object sender,
                                           FormatVertexEventArgs<Leaf> e)
        {
            // decision
            if (e.Vertex.Classifier != null 
                //&& e.Vertex.Classifier.Entropy == 0
                )
            {
                e.VertexFormatter.Label = e.Vertex.Classifier.AttributeName;

                return;
            }

            e.VertexFormatter.Shape = GraphvizVertexShape.Diamond;

            if (e.Vertex.Parent.Negatives == 0)
                e.VertexFormatter.Label = "Yes";
            else
                e.VertexFormatter.Label = "No";
        }

        private void buildGraph(Leaf parentVertex)
        {
            foreach (var childVertex in parentVertex.Children)
            {
                var edge = new TaggedEdge<Leaf, string>(
                    parentVertex,
                    childVertex,
                    childVertex.Parent.Name);

                graph.AddVerticesAndEdge(edge);

                buildGraph(childVertex);
            }
        }

        private Tree tree;
        private AdjacencyGraph<Leaf, TaggedEdge<Leaf, string>> graph;
    }

    /// <summary>
    /// Default dot engine implementation, writes dot code to disk
    /// </summary>
    public sealed class FileDotEngine : IDotEngine
    {
        public string Run(GraphvizImageType imageType,
                          string dot,
                          string outputFileName)
        {
            string output = outputFileName; // +".dot";

            using (var sw = new StreamWriter(output))
            {
                sw.Write(dot);
                sw.Close();
            }

            return output;
        }
    }
}
