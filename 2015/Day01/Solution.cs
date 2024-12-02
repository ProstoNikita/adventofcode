namespace AdventOfCode.Y2015.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Not Quite Lisp")]
class Solution : Solver {

    public object PartOne(string input) {
        return new Counter().CountLisp(input.ToCharArray()).Count;
    }

    public object PartTwo(string input) {
        return new Counter().CountLispNeg(input.ToCharArray());
    }
}

public class Counter {
    public int Count { get; set; } = 0;
    public int Floor { get; set; } = 0;
    public bool IsNegative => Count == -1;

    public Counter CountLisp(char[] input) {
        foreach (var chara in input) {
            switch (chara)
            {
                case ')':
                    Count--;
                    break;
                case '(':
                    Count++;
                    break;
            }
        }
        
        return this;
    }
    
    public int CountLispNeg(char[] input) {
        foreach (var chara in input) {
            Floor++;
            switch (chara)
            {
                case ')':
                    Count--;
                    break;
                case '(':
                    Count++;
                    break;
            }

            if (IsNegative) { return Floor; }
        }
        
        return Floor;
    }
}
