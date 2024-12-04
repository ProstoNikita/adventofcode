namespace AdventOfCode.Y2015.Day16;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Aunt Sue")]
class Solution : Solver {
    public object PartOne(string input) {
        var parameters = new Dictionary<string, int> {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        return input.Split("\n")
            .Select(sueInfo => new Sue(sueInfo))
            .Select(sue => new {
                sue,
                rightAunt = sue.Parameters.All(parameter => parameters[parameter.Key] == parameter.Value)
            })
            .Where(t => t.rightAunt)
            .Select(t => t.sue.Number).FirstOrDefault();
    }

    public object PartTwo(string input) {
        var parameters = new Dictionary<string, int[]> {
            { "children", [3, 3] },
            { "cats", [8, 100] },
            { "samoyeds", [2, 2] },
            { "pomeranians", [0, 2] },
            { "akitas", [0, 0] },
            { "vizslas", [0, 0] },
            { "goldfish", [0, 4] },
            { "trees", [4, 100] },
            { "cars", [2, 2] },
            { "perfumes", [1, 1] }
        };

        return input
            .Split("\n")
            .Select(sueInfo => new Sue(sueInfo))
            .Select(sue => new {
                sue,
                rightAunt = sue.Parameters.All(
                    parameter => parameters[parameter.Key][0] <= parameter.Value &&
                                 parameter.Value <= parameters[parameter.Key][1])
            })
            .Where(t => t.rightAunt)
            .Select(t => t.sue.Number).FirstOrDefault();
    }
}

public record Sue {
    public int Number { get; }
    public Dictionary<string, int> Parameters { get; } = new();

    public Sue(string info) {
        var tokens = info.Split(" ");
        Number = int.Parse(tokens[1].Replace(":", ""));
        Parameters.Add(tokens[2].Replace(":", ""), int.Parse(tokens[3].Replace(",", "")));
        Parameters.Add(tokens[4].Replace(":", ""), int.Parse(tokens[5].Replace(",", "")));
        Parameters.Add(tokens[6].Replace(":", ""), int.Parse(tokens[7].Replace(",", "")));
    }
}
