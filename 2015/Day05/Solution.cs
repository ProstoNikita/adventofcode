namespace AdventOfCode.Y2015.Day05;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Doesn't He Have Intern-Elves For This?")]
class Solution : Solver {

    public object PartOne(string input) {
        return new StringChecker().IsStringNiceCount(input);
    }

    public object PartTwo(string input) {
        return new StringChecker().IsStringNiceCount2(input);
    }
}

public class StringChecker {
    public bool IsStringNice(string input) {
        return Regex.IsMatch(input, @"^(?=.*([aeiou].*?){3})(?=.*([a-z])\2)(?!.*(?:ab|cd|pq|xy)).*$");
    }

    public int IsStringNiceCount(string input) {
        return input.Split("\n").Count(IsStringNice);
    }

    public bool IsStringNice2(string input) {
        return Regex.IsMatch(input, @"^(?=.*([a-z]{2}).*\1)(?=.*([a-z]).\2).*$");
    }

    public int IsStringNiceCount2(string input) {
        return input.Split("\n").Count(IsStringNice2);
    }
}
