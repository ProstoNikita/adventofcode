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
        return input.Split('\n')
            .Select(x => x.Split(' ').Select(int.Parse).ToList())
            .Count(x => IsReportSafe(x));
    }

    public object PartTwo(string input) {
        return input.Split('\n')
            .Select(x => x.Split(' ').Select(int.Parse).ToList())
            .Count(x => IsReportSafe(x, 1));
    }

    private static bool IsReportSafe(List<int> report, int errorTolerance = 0) {
        var result = false;
        if (errorTolerance > 0) {
            for (int i = 0; i < report.Count; i++) {
                result |= IsReportSafe(RemoveOneElement(report, i), --errorTolerance);
            }
            return result;
        }
        
        var reduce = new List<(int, int)>();
        for (int i = 0; i < report.Count - 1; i++) {
            var reduction = report[i] - report[i + 1];
            reduce.Add((Math.Abs(reduction), Math.Sign(reduction)));
        }

        for (int i = 0; i < reduce.Count - 1; i++) {
            if (reduce[i].Item2 == 0 || reduce[i + 1].Item2 == 0) {
                return false;
            }

            if (reduce[i].Item1 > 3) {
                return false;
            }

            if (reduce[i + 1].Item1 > 3) {
                return false;
            }

            if (reduce[i].Item2 != reduce[i + 1].Item2) {
                return false;
            }
            
        }

        return true;

        // for (int i = 0; i < reduce.Count; i++) {
        //     if (reduce[i].Item1 > 3 || reduce[i].Item1 == 0) {
        //         if (errorTolerance == 0) return false;
        //         return IsReportSafe(RemoveOneElement(report, i), errorTolerance - 1) ||
        //                IsReportSafe(RemoveOneElement(report, i + 1), errorTolerance - 1);
        //     }
        //
        //     if (i + 1 >= reduce.Count) {
        //         continue;
        //     }
        //
        //     if (reduce[i].Item2 == reduce[i + 1].Item2 && reduce[i + 1].Item2 != 0) {
        //         continue;
        //     }
        //
        //     if (errorTolerance == 0) return false;
        //     return IsReportSafe(RemoveOneElement(report, i), errorTolerance - 1) ||
        //            IsReportSafe(RemoveOneElement(report, i + 1), errorTolerance - 1);
        // }
        //
        // return true;
    }

    private static List<int> RemoveOneElement(List<int> list, int index) =>
        list.Take(index).Concat(list.Skip(index + 1)).ToList();
}
