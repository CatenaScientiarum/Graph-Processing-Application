public class Graph
{
    private List<Vertex> vertices;
    private List<Edge> edges;

    public List<Vertex> Vertices
    {
        get { return vertices; }
    }

    public List<Edge> Edges
    {
        get { return edges; }
    }


    public Graph()
    {
        vertices = new List<Vertex>();
        edges = new List<Edge>();
    }

    public void AddVertex(Vertex vertex)
    {
        vertices.Add(vertex);
    }


    public void AddEdge(Edge edge)
    {
        edges.Add(edge);
    }

    public void RemoveLoops()
    {
        edges.RemoveAll(edge => edge.From == edge.To);
    }


    public int CountCentralVertices()
    {

        Dictionary<Vertex, int> eccentricities = new Dictionary<Vertex, int>();
        foreach (Vertex v in vertices)
        {
            int maxDistance = 0;
            foreach (Vertex u in vertices)
            {
                if (u != v)
                {
                    int distance = GetShortestPath(v, u).Count;
                    maxDistance = Math.Max(maxDistance, distance);
                }
            }
            eccentricities[v] = maxDistance;
        }
        int minEccentricity = eccentricities.Values.Min();

        int centralVerticesCount = eccentricities.Count(kv => kv.Value == minEccentricity);

        return centralVerticesCount;
    }
    public List<Vertex> GetShortestPath(Vertex start, Vertex end)
    {
        Dictionary<Vertex, Vertex> previous = new Dictionary<Vertex, Vertex>();
        Dictionary<Vertex, int> distances = new Dictionary<Vertex, int>();
        List<Vertex> nodes = new List<Vertex>();

        List<Vertex> path = null;

        foreach (var vertex in vertices)
        {
            if (vertex == start)
            {
                distances[vertex] = 0;
            }
            else
            {
                distances[vertex] = int.MaxValue;
            }

            nodes.Add(vertex);
        }

        while (nodes.Count != 0)
        {
            nodes.Sort((x, y) => distances[x] - distances[y]);

            var smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest == end)
            {
                path = new List<Vertex>();
                while (previous.ContainsKey(smallest))
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }

                break;
            }

            if (distances[smallest] == int.MaxValue)
            {
                break;
            }

            foreach (var neighbor in GetNeighbors(smallest))
            {
                var alt = distances[smallest] + GetDistance(smallest, neighbor);
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = smallest;
                }
            }
        }

        return path;
    }

    public List<Vertex> GetNeighbors(Vertex node)
    {
        List<Vertex> neighbors = new List<Vertex>();

        foreach (Edge edge in edges)
        {
            if (edge.From == node)
            {
                neighbors.Add(edge.To);
            }
            else if (edge.To == node)
            {
                neighbors.Add(edge.From);
            }
        }

        return neighbors;
    }


    public int GetDistance(Vertex nodeA, Vertex nodeB)
    {
        foreach (Edge edge in edges)
        {
            if ((edge.From == nodeA && edge.To == nodeB) || (edge.From == nodeB && edge.To == nodeA))
            {
                return 1;
            }
        }
        return int.MaxValue;
    }


}
public class Vertex
{
    public int Id { get; set; }

    public Vertex(int id)
    {
        Id = id;
    }
}
public class Edge
{
    public Vertex From { get; set; }
    public Vertex To { get; set; }

    public Edge(Vertex from, Vertex to)
    {
        From = from;
        To = to;
    }

}


public class Program
{
    public static Graph InputGraphFromConsole()
    {
        Graph graph = new Graph();

        Console.WriteLine("Введіть кількість вершин:");
        int verticesCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < verticesCount; i++)
        {
            graph.AddVertex(new Vertex(i));
        }

        Console.WriteLine("Введіть кількість ребер:");
        int edgesCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < edgesCount; i++)
        {
            Console.WriteLine($"Введіть вершини для ребра {i + 1} (формат: id1 id2):");
            string[] verticesIds = Console.ReadLine().Split(' ');
            Vertex from = graph.Vertices[int.Parse(verticesIds[0])];
            Vertex to = graph.Vertices[int.Parse(verticesIds[1])];
            graph.AddEdge(new Edge(from, to));
        }

        return graph;
    }

    public static Graph InputGraphFromFile(string filename)
    {
        if (!System.IO.File.Exists(filename))
        {
            Console.WriteLine($"Файл {filename} не знайдено. Будь ласка, перевірте назву файлу та спробуйте знову.");
            return null;
        }
        Graph graph = new Graph();

        string[] lines = System.IO.File.ReadAllLines(filename);

        int verticesCount = int.Parse(lines[0]);

        for (int i = 0; i < verticesCount; i++)
        {
            graph.AddVertex(new Vertex(i));
        }

        int edgesCount = int.Parse(lines[1]);

        for (int i = 0; i < edgesCount; i++)
        {
            string[] verticesIds = lines[i + 2].Split(' ');
            Vertex from = graph.Vertices[int.Parse(verticesIds[0])];
            Vertex to = graph.Vertices[int.Parse(verticesIds[1])];
            graph.AddEdge(new Edge(from, to));
        }

        return graph;
    }


    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Введіть 1 для введення графа з клавіатури або 2 для введення графа з файлу:");
        int inputOption = int.Parse(Console.ReadLine());

        Graph graph1;
        Graph graph2;

        if (inputOption == 1)
        {
            Console.WriteLine("Введіть дані для першого графа:");
            graph1 = InputGraphFromConsole();
            Console.WriteLine("Введіть дані для другого графа:");
            graph2 = InputGraphFromConsole();
        }
        else
        {
            Console.WriteLine("Введіть ім'я файлу для першого графа:");
            string filename1 = Console.ReadLine();
            graph1 = InputGraphFromFile(filename1);
            if (graph1 == null)
            {
                return;
            }

            Console.WriteLine("Введіть ім'я файлу для другого графа:");
            string filename2 = Console.ReadLine();
            graph2 = InputGraphFromFile(filename2);
            if (graph2 == null)
            {
                return;
            }
        }

        graph1.RemoveLoops();
        graph2.RemoveLoops();

        int centralVerticesCount1 = graph1.CountCentralVertices();
        int centralVerticesCount2 = graph2.CountCentralVertices();

        if (centralVerticesCount1 == centralVerticesCount2)
        {
            Console.WriteLine("Графи еквівалентні за кількістю центральних вершин.");
        }
        else
        {
            Console.WriteLine("Графи не еквівалентні за кількістю центральних вершин.");
        }

    }

}