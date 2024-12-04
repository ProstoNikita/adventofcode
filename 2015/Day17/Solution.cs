namespace AdventOfCode.Y2015.Day17;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("No Such Thing as Too Much")]
class Solution : Solver {
    public List<int> Containers = [];

    public object PartOne(string input) {
        Containers = input.Split('\n').Select(int.Parse).ToList();
        var combinations = new List<int>();
        GenerateCombinations(0, 0, [], combinations);
        return combinations.Count;
    }

    public object PartTwo(string input) {
        Containers = input.Split('\n').Select(int.Parse).ToList();
        var combinations = new List<int>();
        GenerateCombinations(0, 0, [], combinations);
        var result = combinations.GroupBy(x => x, x => x).OrderBy(x => x.Key).First().Count();
        return result;
    }

    public void GenerateCombinations(int acc, int i1, List<int> containers, List<int> combination) {
        switch (acc)
        {
            case > 150:
                return;
            case 150:
                combination.Add(containers.Count);
                return;
        }

        for (var i = i1; i < Containers.Count; i++) {
            containers.Add(i);
            acc += Containers[i];
            GenerateCombinations(acc, i + 1, containers, combination);
            acc -= Containers[i];
            containers.RemoveAt(containers.Count - 1);
        }
    }
}
