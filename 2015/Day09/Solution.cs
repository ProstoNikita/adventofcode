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

        return graph.CalculateMinCost();
    }

    public object PartTwo(string input) {
        var graph = new CityGraph();
        foreach (var line in input.Split('\n')) {
            graph.ParseSaveCity(line);
        }

        return graph.CalculateMaxCost();
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

    public int CalculateMinCost() {
        var perm = GetPermutations(m_cities.Values.ToList(), m_cities.Values.Count);

        var min = int.MaxValue;
        var noPath = false;
        
        foreach (var list in perm) {
            int curr = 0;
            for (int i = 0; i < list.Count - 1; i++) {
                if (list[i].Paths.TryGetValue(list[i + 1], out var value)) {
                    if (curr + value > min) {
                        noPath = true;
                        break;
                    }

                    curr += value;
                } else {
                    noPath = true;
                    break;
                }
            }

            if (noPath) {
                noPath = false;
            } else {
                min = Math.Min(min, curr);
            }
        }

        return min;
    }
    
    public int CalculateMaxCost() {
        var perm = GetPermutations(m_cities.Values.ToList(), m_cities.Values.Count);

        var max = 0;
        var noPath = false;
        
        foreach (var list in perm) {
            int curr = 0;
            for (int i = 0; i < list.Count - 1; i++) {
                if (list[i].Paths.TryGetValue(list[i + 1], out var value)) {
                    curr += value;
                } else {
                    noPath = true;
                    break;
                }
            }

            if (noPath) {
                noPath = false;
            } else {
                max = Math.Max(max, curr);
            }
        }

        return max;
    }

    static IEnumerable<List<T>> GetPermutations<T>(List<T> list, int length) {
        if (length == 1) return list.Select(t => new List<T> { t });
        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new List<T> { t2 }).ToList());
    }
}

public class City {
    public string Name { get; set; }
    public Dictionary<City, int> Paths { get; } = new();
}
