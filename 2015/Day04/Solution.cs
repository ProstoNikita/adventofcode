using System.Security.Cryptography;

namespace AdventOfCode.Y2015.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("The Ideal Stocking Stuffer")]
class Solution : Solver {

    public object PartOne(string input) {
        return Lowest(input, "00000");
    }

    public object PartTwo(string input) {
        return Lowest(input, "000000");
    }

    public int Lowest(string input, string startsWith) {
        var num = 0;
        while (true) {
            if (BitConverter.ToString(MD5.HashData(Encoding.Default.GetBytes(input + num))).Replace("-", "").StartsWith(startsWith)) {
                return num;
            }

            num++;
        }
    }
}
