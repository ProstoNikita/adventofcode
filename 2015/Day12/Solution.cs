using System.Text.Json;

namespace AdventOfCode.Y2015.Day12;

using System;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("JSAbacusFramework.io")]
class Solution : Solver {
    public object PartOne(string input) {
        var matches = Regex.Matches(input, @"[-]{0,1}[\d]*[.]{0,1}[\d]");
        var result = 0;
        foreach (Match numbers in matches) {
            result += int.Parse(numbers.Value);
        }

        return result;
    }

    public object PartTwo(string input) {
        var json = JsonDocument.Parse(input);
        return GetValues(json.RootElement);
    }

    public int GetValues(JsonElement node) {
        switch (node.ValueKind) {
            case JsonValueKind.Object:
                if (node.EnumerateObject().ToList().Any(item =>
                        item.Value.ValueKind == JsonValueKind.String && item.Value.ValueEquals("red"))) break;
                return node.EnumerateObject().ToList()
                    .Select(item => GetValues(item.Value)).Sum();
            case JsonValueKind.Array:
                return node.EnumerateArray().ToList().Select(GetValues).Sum();
            case JsonValueKind.Number:
                return int.Parse(node.GetRawText());
            case JsonValueKind.String:
            case JsonValueKind.Undefined:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return 0;
    }
}
