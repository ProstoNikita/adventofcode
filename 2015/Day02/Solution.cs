namespace AdventOfCode.Y2015.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("I Was Told There Would Be No Math")]
class Solution : Solver {
    public object PartOne(string input) {
        return Solve(input);
    }

    public object PartTwo(string input) {
        return Solve2(input);
    }

    private int Solve(string input) {
        return input.Trim().Split("\n").Select(x => x.Split("x").Select(int.Parse)).Select(SolveSingular).Aggregate((x, y) => x + y);
    }

    private int Solve2(string input) {
        return input.Trim().Split("\n").Select(x => x.Split("x").Select(int.Parse)).Select(SolveSingular2).Aggregate((x, y) => x + y);
    }

    private int SolveSingular(IEnumerable<int> input) {
        var list = input.ToList();
        list.Sort();
        return 3 * list[0] * list[1] + 2 * list[2] * list[1] + 2 * list[0] * list[2];
    }

    private int SolveSingular2(IEnumerable<int> input) {
        var list = input.ToList();
        list.Sort();
        return 2 * list[0] + 2 * list[1] + list[0] * list[1] * list[2];
    }
}
