namespace AdventOfCode.Y2015.Day21;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("RPG Simulator 20XX")]
class Solution : Solver {
    public Entity Boss;
    public Entity Hero;
    
    public List<Item> Weapons = [
        new("Dagger", 8, 4, 0),
        new("Shortsword", 10, 5, 0),
        new("Warhammer", 25, 6, 0),
        new("Longsword", 40, 7, 0),
        new("Greataxe", 74, 8, 0)
    ];

    public List<Item> Armor = [
        new("Leather", 13, 0, 1),
        new("Chainmail", 31, 0, 2),
        new("Splintmail", 53, 0, 3),
        new("Bandedmail", 75, 0, 4),
        new("Platemail", 102, 0, 5),
    ];

    public List<Item> Rings = [
        new("Damage +1", 25, 1, 0),
        new("Damage +2", 50, 2, 0),
        new("Damage +3", 100, 3, 0),
        new("Defense +1", 20, 0, 1),
        new("Defense +2", 40, 0, 2),
        new("Defense +3", 80, 0, 3),
    ];

    public object PartOne(string input) {
        var lines = input.Split('\n');
        
        Boss = new Entity {
            Name = "Boss",
            Health = int.Parse(lines[0].Split(" ")[2]),
            Damage = int.Parse(lines[1].Split(" ")[1]),
            Armor = int.Parse(lines[2].Split(" ")[1]),
        };
        
        Hero = new Entity {
            Name = "Hero",
            Health = 100,
        };

        var min = int.MaxValue;
        var permutations = CalculatePermutations();
        
        foreach (var items in permutations) {
            foreach (var item in items) {
                Hero.Add(item);
            }

            if (DoesHeroWin(Hero, Boss) && min > Hero.Items.Sum(x => x.Cost)) {
                Hero.Items.ForEach(Console.WriteLine);
                min = Hero.Items.Sum(x => x.Cost);
                Console.WriteLine($"Min: {min}");
                Console.WriteLine("-------------");
            }
            
            Hero.Reset();
        }
        
        
        return min;
    }

    public object PartTwo(string input) {
        var max = 0;
        var permutations = CalculatePermutations();
        
        foreach (var items in permutations) {
            foreach (var item in items) {
                Hero.Add(item);
            }

            if (!DoesHeroWin(Hero, Boss) && max < Hero.Items.Sum(x => x.Cost)) {
                Hero.Items.ForEach(Console.WriteLine);
                max = Hero.Items.Sum(x => x.Cost);
                Console.WriteLine($"Max: {max}");
                Console.WriteLine("-------------");
            }
            
            Hero.Reset();
        }
        
        return max;
    }

    public bool DoesHeroWin(Entity hero, Entity boss) {
        var heroDamage = hero.Damage - boss.Armor;
        if (heroDamage <= 0) heroDamage = 1;
        var bossDamage = boss.Damage - hero.Armor;
        if (bossDamage <= 0) bossDamage = 1;

        var heroWin = false;
        var currentHero = hero.Health;
        var currentBoss = boss.Health;
        while (true) {
            currentBoss -= heroDamage;
            if (currentBoss <= 0) {
                heroWin = true;
                break;
            }
            
            currentHero -= bossDamage;
            if (currentHero <= 0) {
                break;
            }
        }
        
        return heroWin;
        // return boss.Health / heroDamage < (hero.Health / bossDamage);
    }
    
    public List<List<Item>> CalculatePermutations()
    {
        var permutations = new List<List<Item>>();
        
        foreach (var weapon in Weapons)
        {
            var armorOptions = Armor.Prepend(null);

            foreach (var armor in armorOptions)
            {
                var ringCombinations = GetRingCombinations();

                foreach (var ringCombination in ringCombinations)
                {
                    var loadout = new List<Item> { weapon };

                    if (armor != null)
                    {
                        loadout.Add(armor);
                    }

                    loadout.AddRange(ringCombination);
                    permutations.Add(loadout);
                }
            }
        }

        return permutations;
    }

    private List<List<Item>> GetRingCombinations()
    {
        var combinations = new List<List<Item>> { new() };

        for (int i = 0; i < Rings.Count; i++)
        {
            combinations.Add([Rings[i]]);

            for (int j = i + 1; j < Rings.Count; j++)
            {
                combinations.Add([Rings[i], Rings[j]]);
            }
        }

        return combinations;
    }
    
    public void PrintPermutations(List<List<Item>> permutations)
    {
        foreach (var loadout in permutations)
        {
            Console.WriteLine("Loadout:");
            foreach (var item in loadout)
            {
                Console.WriteLine($" - {item.Name} (Cost: {item.Cost}, Damage: {item.Damage}, Defense: {item.Armor})");
            }
            Console.WriteLine();
        }
    }
}

public record Item {
    public string Name { get; }
    public int Cost { get; }
    public int Damage { get; }
    public int Armor { get; }

    public Item(string name, int cost, int damage, int armor) {
        Name = name;
        Cost = cost;
        Damage = damage;
        Armor = armor;
    }
}



public record Entity {
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    
    public List<Item> Items { get; set; } = [];

    public void Add(Item item) {
        Damage += item.Damage;
        Armor += item.Armor;
        Items.Add(item);
    }

    public void Reset() {
        Damage = 0;
        Armor = 0;
        Items.Clear();
    }
}
