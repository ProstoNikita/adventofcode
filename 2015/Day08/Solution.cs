using System.Net;

namespace AdventOfCode.Y2015.Day08;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Matchsticks")]
class Solution : Solver {
    public object PartOne(string input) {
        var lines = input.Split("\n");
        return lines.Sum(Calculate);
    }

    public object PartTwo(string input) {
        var lines = input.Split('\n');
        return lines.Sum(Calculate2);
    }

    public int Calculate(string input) {
        var byteArray = Encoding.UTF8.GetBytes(input).AsSpan();
        var result = 0;
        var i = 0;
        while (byteArray.Length - 1 > i) {
            if (byteArray[i] == (byte)'\"') {
                i += 1;
                continue;
            }

            if (byteArray[i] == '\\') {
                result += 1;
                switch (byteArray[i + 1]) {
                    case (byte)'\\' or (byte)'\"':
                        i += 2;
                        break;
                    case (byte)'x':
                        i += 4;
                        break;
                    default:
                        i += 1;
                        break;
                }
            } else {
                i += 1;
                result++;
            }
        }

        return byteArray.Length - result;
    }

    public int Calculate2(string input) {
        var byteArray = Encoding.UTF8.GetBytes(input).AsSpan();
        var result = 0;
        var i = 0;
        while (byteArray.Length > i) {
            if (byteArray[i] == (byte)'\"' || byteArray[i] == (byte)'\\') {
                result += 2;
            } else {
                result += 1;
            }

            i++;
        }

        // Console.WriteLine("{0} - {1}", result, byteArray.Length);

        return result + 2 - byteArray.Length;
    }
}
