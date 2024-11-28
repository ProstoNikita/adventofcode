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
        return new MapChecker().Calculate(input, 0).Count;
    }

    public object PartTwo(string input) {
        return new MapChecker().Calculate2(input).Count;
    }
}

public class MapChecker {
    private readonly HashSet<(int, int)> m_map = [];
    private readonly HashSet<(int, int)> m_map2 = [];

    public HashSet<(int, int)> Calculate(string input, int mapId) {
        var map = mapId == 0 ? m_map : m_map2;
        
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
        var string1 = new string(input.Where((_, i) => i % 2 == 0).ToArray());
        var string2 = new string(input.Where((_, i) => i % 2 == 1).ToArray());
        
        Calculate(string1, 0);
        Calculate(string2, 1);

        m_map.UnionWith(m_map2);
        
        return m_map;
    }
    
    
}
