namespace AdventOfCode.Y2024.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Mull It Over")]
class Solution : Solver {

    public object PartOne(string input) {
        var matches = Regex.Matches(input, @"\bmul\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)");
        
        return matches.Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)).Sum();
    }

    public object PartTwo(string input) {
        var matches = Regex.Matches(input, @"\b(do|don't)\(\)|\bmul\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)");
        var doCalc = true;
        var result = 0;

        foreach (Match match in matches) {
            if (match.Value.StartsWith("do(")) {
                doCalc = true;
            }
            else if (match.Value.StartsWith("don't(")) {
                doCalc = false;
            }
            else if (doCalc && match.Value.StartsWith("mul(")) {
                result += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }
        
        return result;
    }
}
