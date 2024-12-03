using System.Collections;

namespace AdventOfCode.Y2015.Day09;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("All in a Single Night")]
class Solution : Solver {
    public object PartOne(string input) {
        var graph = new CityGraph();
        foreach (var line in input.Split('\n')) {
            graph.ParseSaveCity(line);
        }
        
        Console.WriteLine(graph.DFS());
        return graph.DFS();
    }

    public object PartTwo(string input) {
        return 0;
    }
}

public class CityGraph {
    private readonly Dictionary<string, City> m_cities = new();

    public void ParseSaveCity(string input) {
        var tokens = input.Split(" ");

        if (!m_cities.TryGetValue(tokens[0], out var cityA)) {
            cityA = new City {
                Name = tokens[0],
            };
            m_cities[tokens[0]] = cityA;
        }

        if (!m_cities.TryGetValue(tokens[2], out var cityB)) {
            cityB = new City {
                Name = tokens[2],
            };
            m_cities[tokens[2]] = cityB;
        }

        var weight = int.Parse(tokens[4]);
        cityA.Paths[cityB] = weight;
        cityB.Paths[cityA] = weight;
    }
    
    public

    public int DFS(string start = "London") {
        var stack = new Stack<City>();
        stack.Push(m_cities[start]);
        m_cities[start].ShortestPathLength = 0;

        while (stack.Count != 0) {
            var current = stack.Pop();
            foreach (var currentPath in current.Paths.Where(currentPath =>
                         currentPath.Key.ShortestPathLength > current.ShortestPathLength + currentPath.Value)) {
                currentPath.Key.ShortestPathLength = current.ShortestPathLength + currentPath.Value;
                stack.Push(currentPath.Key);
            }
        }

        var values = m_cities.Values.ToList();
        values.Sort((city, city1) => city.ShortestPathLength < city1.ShortestPathLength ? -1 : 1);
        
        return values[1].ShortestPathLength;
    }
}

public class City {
    public int ShortestPathLength { get; set; } = int.MaxValue;
    public string Name { get; set; }
    public Dictionary<City, int> Paths { get; } = new();
    
    public Dictionary<City, int> ShortestPathLengths { get; } = new();
}
