namespace AdventOfCode.Y2015.Day20;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Infinite Elves and Infinite Houses")]
class Solution : Solver {

    public object PartOne(string input) {
        var target = long.Parse(input);
        var result = 0L;
        var count = 0;
        
        while (result * 10 < target) {
            count++;
            result = CalculateSieve(count);
        }
        
        return count;
    }

    private int CalculateSieve(int n, int limit = -1) {
        var sum = 0;
        var iterations = 1;
        for (var i = 1; i <= Math.Sqrt(n); i++)
        {
            if (limit != -1 && iterations >= limit)
                break;

            if (n % i != 0)
                continue;

            sum += i;
            iterations++;
            if (n / i == i)
                continue;

            sum += n / i;
            iterations++;
        }
        return sum;
    }

    public object PartTwo(string input) {
        var target = long.Parse(input);
        var result = 0L;
        var count = 0;
        
        while (result * 11 < target) {
            count++;
            result = CalculateSieve(count, 50);
        }
        
        return count;
    }
}
