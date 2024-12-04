namespace AdventOfCode.Y2015.Day13;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Knights of the Dinner Table")]
class Solution : Solver {

    public object PartOne(string input) {
        var table = new Table();
        foreach (var command in input.Split('\n')) {
            var tokens = command.Split(' ');
            var value = int.Parse(tokens[3]);
            if (tokens[2] == "lose") {
                value *= -1;
            }
            table.AddValue(tokens[0], tokens[10].Replace(".", ""), value);
        }
        return table.CalculateBestSitting();
    }

    public object PartTwo(string input) {
        var table = new Table();
        foreach (var command in input.Split('\n')) {
            var tokens = command.Split(' ');
            var value = int.Parse(tokens[3]);
            if (tokens[2] == "lose") {
                value *= -1;
            }
            table.AddValue(tokens[0], tokens[10].Replace(".", ""), value);
        }
        table.AddMyself("Nikita");
        return table.CalculateBestSitting();
    }
}

public class Table {
    private readonly int[,] m_tableValues = new int[9,9];
    private readonly List<string> m_names = [];

    public void AddValue(string name, string name2, int value) {
        if (!m_names.Contains(name)) {
            m_names.Add(name);
        }
        var position1 = m_names.IndexOf(name);

        if (!m_names.Contains(name2)) {
            m_names.Add(name2);
        }
        var position2 = m_names.IndexOf(name2);
        
        m_tableValues[position1, position2] += value;
    }

    public void AddMyself(string name) {
        m_names.Add(name);
    }

    public int CalculateBestSitting() {
        var permutations = GetPermutations(m_names.Select((x, i) => i).ToList(), m_names.Count);
        var max = 0;

        foreach (var permutation in permutations) {
            permutation.Add(permutation[0]);
            var curr = 0;
            for (int i = 0; i < permutation.Count - 1; i++) {
                curr += m_tableValues[permutation[i], permutation[i + 1]] + m_tableValues[permutation[i + 1], permutation[i]];
            }
            max = Math.Max(max, curr);
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
