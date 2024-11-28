namespace AdventOfCode.Y2015.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Perfectly Spherical Houses in a Vacuum")]
class Solution : Solver {

    public object PartOne(string input) {
        return new MapChecker().Calculate(input, MapChecker.Map).Count;
    }

    public object PartTwo(string input) {
        return new MapChecker().Calculate2(input).Count;
    }
}

public class MapChecker {
    public static HashSet<(int, int)> Map = [];
    public static HashSet<(int, int)> Map2 = [];

    public HashSet<(int, int)> Calculate(string input, HashSet<(int, int)> map) {
        var directions = input.ToCharArray();
        map.Add((0, 0));
        var pos = (0, 0);
        foreach (var direction in directions) {
            switch (direction) {
                case '^':
                    pos.Item2++;
                    break;
                case 'v':
                    pos.Item2--;
                    break;
                case '>':
                    pos.Item1++;
                    break;
                case '<':
                    pos.Item1--;
                    break;
            }
            map.Add((pos.Item1, pos.Item2));
        }
        
        return map;
    }

    public HashSet<(int, int)> Calculate2(string input) {
        Calculate(new string(input.Where((c,i) => i % 2 == 0).ToArray()), Map);
        Calculate(new string(input.Where((c,i) => i % 2 == 1).ToArray()), Map2);
        Map.IntersectWith(Map2);

        return Map;
    }
    
    
}
