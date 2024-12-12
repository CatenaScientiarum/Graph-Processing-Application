Graph Processing Application
This project implements a simple graph processing system using C#. It provides functionality to manage graphs, including adding vertices and edges, finding the shortest paths, counting central vertices, and removing loops.

Libraries and Frameworks Used
C#: The application is built using C# and .NET runtime.
System.IO: Used for reading and writing files.
System.Collections.Generic: Provides collections like List<T>, Dictionary<TKey, TValue>, and Queue<T> for managing graph data structures.
Features
Add vertices and edges to a graph.
Remove loops (edges that connect a vertex to itself).
Calculate the number of central vertices in a graph (based on eccentricity).
Find the shortest path between two vertices using Dijkstra’s algorithm.
Input graphs from the console or from a file.

Example Input File Format
The input file should contain data in the following format:

4
4
0 1
1 2
2 3
3 0

In this example:

The graph has 4 vertices.
It has 4 edges: (0,1), (1,2), (2,3), (3,0).
License
This project is licensed under the MIT License.