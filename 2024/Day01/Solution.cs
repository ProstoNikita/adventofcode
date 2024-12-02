namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Historian Hysteria")]
class Solution : Solver {

    public object PartOne(string input) {
        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var numbers in input.Split('\n')) {
            var nums = numbers.Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).Select(int.Parse).ToList();
            list1.Add(nums[0]);
            list2.Add(nums[1]);
        }
        
        list1.Sort();
        list2.Sort();

        return list1.Select((t, i) => Math.Abs(t - list2[i])).Sum();
    }

    public object PartTwo(string input) {
        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var numbers in input.Split('\n')) {
            var nums = numbers.Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).Select(int.Parse).ToList();
            list1.Add(nums[0]);
            list2.Add(nums[1]);
        }
        
        list1.Sort();
        list2.Sort();

        return list1.Select(value => value * list2.FindAll(i => i == value).Count).Sum();
    }
}
