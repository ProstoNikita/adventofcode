namespace AdventOfCode.Y2024.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Red-Nosed Reports")]
class Solution : Solver {
    public object PartOne(string input) {
        return input.Split('\n').Select(x => x.Split(' ').Select(int.Parse))
            .Select(x => CheckIfReportSafe(x, 0))
            .Count(x => x);
    }

    public object PartTwo(string input) {
        return input.Split('\n').Select(x => x.Split(' ').Select(int.Parse))
            .Select(x => CheckIfReportSafe(x, 1))
            .Count(x => x);
    }

    private static bool CheckIfReportSafe(IEnumerable<int> report, int errorTolerance = 0) {
        var enumerable = report.ToList();
        var errors = new bool[enumerable.Count];
        
        var direction = Math.Sign(enumerable[0] - enumerable[1]);
        for (var i = 1; i < enumerable.Count - 1; i++) {
            if (direction != 0) {
                break;
            }
            
            errors[i - 1] = true;
            if (errors.Count(x => x) > errorTolerance) {
                return false;
            }
            direction = Math.Sign(enumerable[i] - enumerable[i + 1]);
        }
        
        for (var i = 0; i < enumerable.Count - 1; i++) {
            var diff = enumerable[i] - enumerable[i + 1];
            
            if (Math.Abs(diff) > 3) {
                errors[i] = true;
            }

            if (Math.Sign(diff) != direction) {
                errors[i] = true;
            }

            if (errors.Count(x => x) > errorTolerance) {
                return false;
            }
        }

        return true;
    }
}
