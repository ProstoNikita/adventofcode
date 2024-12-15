namespace AdventOfCode.Y2015.Day22;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Wizard Simulator 20XX")]
class Solution : Solver {
    private static readonly List<SpellType> SpellTypes = Enum.GetValues<SpellType>().ToList();
    private static readonly int[] SpellRequirements = [53, 73, 113, 173, 229];
    
    public object PartOne(string input) {
        return 0;
    }

    public object PartTwo(string input) {
        return 0;
    }

    public int CalculateMinMana(int heroHealth, int bossHealth, int heroMana) {
        var spellTimers = new int[SpellTypes.Count];
        return RecursiveOneStep(heroHealth, bossHealth, heroMana, 0,true, spellTimers);
    }

    public int RecursiveOneStep(int heroHealth, int bossHealth, int heroMana, int heroManaUsed, bool isPlayer, int[] spellTimers) {
        if (isPlayer) {
            for (int i = 0; i < spellTimers.Length; i++) {
                if (spellTimers[i] > 0) {
                    var spellType = SpellTypes[i];
                    switch (spellType) {
                        case SpellType.Poison:
                            bossHealth -= 3;
                            break;
                        case SpellType.Recharge:
                            heroMana += 101;
                            break;
                        case SpellType.Shield:
                        case SpellType.Drain:
                        case SpellType.MagicMissile:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    spellTimers[i]--;
                }
            }

            var min = int.MaxValue;
            foreach (var spellType in SpellTypes) {
                if (SpellRequirements[SpellTypes.IndexOf(spellType)] <= heroMana &&
                    spellTimers[SpellTypes.IndexOf(spellType)] == 0) {
                    switch (spellType) {
                        case SpellType.MagicMissile:
                            break;
                        case SpellType.Drain:
                            break;
                        case SpellType.Shield:
                            break;
                        case SpellType.Poison:
                            break;
                        case SpellType.Recharge:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return min;
        } else {
            
            
            
            return 0;
        }
    }
}

public record Entity {
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
}


public enum SpellType {
    MagicMissile = 0,
    Drain = 1,
    Shield = 2,
    Poison = 3,
    Recharge = 4
}
