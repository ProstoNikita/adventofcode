namespace AdventOfCode.Y2015.Day11;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Corporate Policy")]
class Solution : Solver {
    public object PartOne(string input) {
        var password = ToIntPass(input);

        do {
            password = IncrementPassword(password);
        } while (!ValidatePassword(password));

        return ToStringPass(password);
    }

    public object PartTwo(string input) {
        return PartOne((string) PartOne(input));
    }

    public bool ValidatePassword(List<int> password) {
        return IOLCheck(password) && ThreeLettersCheck(password) && TwoPairsCheck(password);
    }

    public bool IOLCheck(List<int> password) {
        return !password.Contains(8) && !password.Contains(14) && !password.Contains(11);
    }

    public bool ThreeLettersCheck(List<int> password) {
        var numbersInRow = new List<int>();
        foreach (var number in password) {
            if (numbersInRow.Count != 0 && numbersInRow[^1] + 1 != number) {
                numbersInRow.Clear();
            }

            numbersInRow.Add(number);
            if (numbersInRow.Count == 3)
                return true;
        }

        return false;
    }

    public bool TwoPairsCheck(List<int> password) {
        bool onePair = false;
        int onePairInt = -1;
        bool twoPair = false;

        for (int i = 0; i < password.Count - 1; i++) {
            if (!onePair && password[i] == password[i + 1]) {
                onePair = true;
                onePairInt = password[i];
            }

            if (onePair && password[i] == password[i + 1] && onePairInt != password[i]) {
                twoPair = true;
                break;
            }
        }

        return onePair && twoPair;
    }

    public List<int> IncrementPassword(List<int> password) {
        var pass = false;
        if (password[^1] + 1 > 25) {
            password[^1] = 0;
            pass = true;
        } else {
            password[^1] += 1;
        }

        if (!pass) {
            return password;
        }

        for (int i = password.Count - 2; i >= 0; i--) {
            if (!pass) break;
            if (password[i] + 1 > 25) {
                password[i] = 0;
            } else {
                password[i] += 1;
                pass = false;
            }
        }

        return password;
    }

    public List<int> ToIntPass(string password) {
        return password.ToCharArray().Select(ch => ch.FromAlphabetToNumber()).ToList();
    }

    public string ToStringPass(List<int> password) {
        return string.Join("", password.Select(x => x.FromNumberToAlphabet()).ToList());
    }
}

public static class CharIntExtension {
    public static int FromAlphabetToNumber(this char c) {
        return c - 97;
    }

    public static char FromNumberToAlphabet(this int number) {
        return (char)(number + 97);
    }
}
