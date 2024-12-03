namespace AdventOfCode.Y2015.Day10;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Elves Look, Elves Say")]
class Solution : Solver {

    public object PartOne(string input) {
        var inputList = input.ToCharArray().Select(char.ToString).Select(int.Parse).ToList();

        for (int i = 0; i < 40; i++) {
            inputList = GenerateLookAndSayCycle(inputList);
        }
        
        return string.Join("", inputList).Length;
    }

    public object PartTwo(string input) {
        var inputList = input.ToCharArray().Select(char.ToString).Select(int.Parse).ToList();

        for (int i = 0; i < 50; i++) {
            inputList = GenerateLookAndSayCycle(inputList);
        }
        
        return string.Join("", inputList).Length;
    }

    public List<int> GenerateLookAndSayCycle(List<int> input) {
        var lookAndSay = new List<int>();
        var currentLookAndSay = 0;
        var counter = 0;
        
        foreach (var number in input) {
            if (currentLookAndSay != number) {
                if (currentLookAndSay != 0) {
                    lookAndSay.Add(counter);
                    lookAndSay.Add(currentLookAndSay);

                    counter = 0;
                }
                currentLookAndSay = number;
            }
            counter++;
        }
        
        lookAndSay.Add(counter);
        lookAndSay.Add(currentLookAndSay);
        
        return lookAndSay;
    }
}
