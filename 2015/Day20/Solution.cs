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
        // var result = 0L;
        // var count = 0;
        //
        // while (result * 10 < target) {
        //     count++;
        //     result = CalculateSieve(count);
        // }
        //
        // return count;
        return StepByStepFind(target, 10);
    }

    //For some reason Limit doesn't work, doing different stuff
    private int CalculateSieve(int n) {
        var sum = 0;

        for (var i = 1; i <= Math.Sqrt(n); i++)
        {
            if (n % i != 0)
                continue;

            sum += i;
            if (n / i == i)
                continue;

            sum += n / i;
        }
        return sum;
    }

    private int StepByStepFind(long target, int presentPrice, int limit = 100000000) {
        var houses = new int[1000000];

        for (int i = 0; i < houses.Length; i++) {
            var margin = i;
            var step = 0;

            while (margin < houses.Length && step < limit) {
                houses[margin] += presentPrice * i;
                margin += i;
                step++;
            }
        }

        foreach (var presents in houses) {
            if (presents > target) {
                return houses.ToList().IndexOf(presents);
            }
        }
        return -1;
    }

    public object PartTwo(string input) {
        var target = long.Parse(input);
        
        return StepByStepFind(target, 11, 50);
    }
}
