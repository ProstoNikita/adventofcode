namespace AdventOfCode.Y2015.Day15;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Science for Hungry People")]
class Solution : Solver {
    public object PartOne(string input) {
        var cookieMaker = new CookieMaker();
        foreach (var ingredient in input.Split('\n')) {
            var tokens = ingredient.Split(' ');
            cookieMaker.AddIngredient(tokens[0].Replace(":", ""),
                int.Parse(tokens[2].Replace(",", "")),
                int.Parse(tokens[4].Replace(",", "")),
                int.Parse(tokens[6].Replace(",", "")),
                int.Parse(tokens[8].Replace(",", "")),
                int.Parse(tokens[10].Replace(",", "")));
        }

        return cookieMaker.CalculateBestValue();
    }

    public object PartTwo(string input) {
        var cookieMaker = new CookieMaker();
        foreach (var ingredient in input.Split('\n')) {
            var tokens = ingredient.Split(' ');
            cookieMaker.AddIngredient(tokens[0].Replace(":", ""),
                int.Parse(tokens[2].Replace(",", "")),
                int.Parse(tokens[4].Replace(",", "")),
                int.Parse(tokens[6].Replace(",", "")),
                int.Parse(tokens[8].Replace(",", "")),
                int.Parse(tokens[10].Replace(",", "")));
        }

        return cookieMaker.CalculateBestValue(true);
    }
}

public class CookieMaker {
    public List<Ingredient> Ingredients { get; } = [];
    public List<int[]> Combinations { get; } = [];


    public void AddIngredient(string name, int capacity, int durability, int flavor, int texture, int calories) {
        var ingredient = new Ingredient {
            Name = name,
            Capacity = capacity,
            Durability = durability,
            Flavor = flavor,
            Texture = texture,
            Calories = calories
        };
        Ingredients.Add(ingredient);
    }

    public long CalculateBestValue(bool checkCalories = false) {
        GenerateCombinations(Ingredients.Count, 100, new int[Ingredients.Count], Combinations, 0, checkCalories);

        var max = 0L;
        foreach (var combo in Combinations) {
            var valueCap = 0;
            var valueDur = 0;
            var valueFl = 0;
            var valueTex = 0;
            for (var i = 0; i < combo.Length; i++) {
                valueCap += Ingredients[i].Capacity * combo[i];
                valueDur += Ingredients[i].Durability * combo[i];
                valueFl += Ingredients[i].Flavor * combo[i];
                valueTex += Ingredients[i].Texture * combo[i];
            }

            long curr = Math.Max(0, valueCap) * Math.Max(0, valueDur) * Math.Max(0, valueFl) * Math.Max(0, valueTex);
            max = Math.Max(max, curr);
        }

        return max;
    }

    void GenerateCombinations(int numIngredients, int remaining, int[] currentCombo, List<int[]> combinations,
        int index = 0, bool checkCalories = false) {
        if (index == numIngredients - 1) {
            currentCombo[index] = remaining;

            if (checkCalories) {
                var calories = 0;
                for (var i = 0; i < numIngredients; i++) {
                    calories += Ingredients[i].Calories * currentCombo[i];
                }

                if (calories != 500) {
                    return;
                }
            }

            combinations.Add((int[])currentCombo.Clone());
            return;
        }

        for (int i = 0; i <= remaining; i++) {
            currentCombo[index] = i;
            GenerateCombinations(numIngredients, remaining - i, currentCombo, combinations, index + 1, checkCalories);
        }
    }
}

public record Ingredient {
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int Durability { get; set; }
    public int Flavor { get; set; }
    public int Texture { get; set; }
    public int Calories { get; set; }
}
