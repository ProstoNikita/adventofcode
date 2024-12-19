namespace AdventOfCode.Y2015.Day22;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Wizard Simulator 20XX")]
class Solution : Solver {
    private Dictionary<SpellType, Spell> spells = new() {
        { SpellType.MagicMissile, new Spell { Type = SpellType.MagicMissile, IsEffect = false, SpellMana = 53 } },
        { SpellType.Drain, new Spell { Type = SpellType.Drain, IsEffect = false, SpellMana = 73 } },
        { SpellType.Shield, new Spell { Type = SpellType.Shield, IsEffect = true, SpellMana = 113, EffectTimer = 6 } },
        { SpellType.Poison, new Spell { Type = SpellType.Poison, IsEffect = true, SpellMana = 173, EffectTimer = 6 } }, {
            SpellType.Recharge,
            new Spell { Type = SpellType.Recharge, IsEffect = true, SpellMana = 229, EffectTimer = 5 }
        }
    };

    public object PartOne(string input) {
        var hero = new Entity {
            Health = 10,
            Mana = 250,
        };

        var boss = new Entity {
            Health = 13,
        };

        return CalculateMinMana(hero, boss, [], true);
    }

    public object PartTwo(string input) {
        return 0;
    }

    public int CalculateMinMana(Entity hero, Entity boss, List<Spell> usedSpells, bool isPlayer) {
        var min = int.MaxValue;

        if (isPlayer) {
            Console.WriteLine("-- Player turn --");
            Console.WriteLine(hero);
            Console.WriteLine(boss);
            
            foreach (var spell in spells.Select(spellPair => spellPair.Value.Clone())) {
                var heroClone = hero.CloneEn();
                var bossClone = boss.CloneEn();
                
                if (hero.Mana - spell.SpellMana < 0) {
                    if (spell.Type == SpellType.MagicMissile) {
                        return int.MaxValue;
                    }

                    continue;
                }

                if (!spell.IsEffect) {
                    spell.Apply(heroClone, bossClone);
                }

                if (spell.IsEffect && !hero.HasEffect(spell)) {
                    spell.Cast(heroClone);
                }
                
                heroClone.Mana -= spell.SpellMana;
                Console.WriteLine($"{spell.Type} casted!");

                if (bossClone.Health <= 0) {
                    return usedSpells.Sum(sp => sp.SpellMana);
                }
                
                heroClone.ApplyEffectsExcept(bossClone, spell.Type);
                
                if (bossClone.Health <= 0) {
                    return usedSpells.Sum(sp => sp.SpellMana);
                }
                
                min = Math.Min(min, CalculateMinMana(heroClone, bossClone, usedSpells, false));
            }
        } else {
            Console.WriteLine("-- Boss turn --");
            Console.WriteLine(hero);
            Console.WriteLine(boss);
            var heroClone = hero.CloneEn();
            var bossClone = boss.CloneEn();
            
            heroClone.ApplyEffects(bossClone);
            
            if (bossClone.Health <= 0) {
                return usedSpells.Sum(sp => sp.SpellMana);
            }
            
            bossClone.Attack(heroClone);
            
            return heroClone.Health <= 0 ? int.MaxValue : CalculateMinMana(heroClone, bossClone, usedSpells, true);
        }

        return min;
    }

    public record Entity {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int Mana { get; set; }

        public List<Spell> Effects { get; set; } = [];

        public void ApplyEffectsExcept(Entity enemy, SpellType spellType) {
            foreach (var effect in Effects.Where(effect => effect.Type != spellType)) {
                effect.Apply(this, enemy);
            }

            Effects = Effects.Where(x => x.EffectTimer != 0).ToList();
        }
        
        public void ApplyEffects(Entity enemy) {
            foreach (var effect in Effects) {
                effect.Apply(this, enemy);
            }

            Effects = Effects.Where(x => x.EffectTimer != 0).ToList();
        }

        public bool HasEffect(Spell spell) {
            return Effects.Any(x => x.Type == spell.Type && x.IsEffect);
        }

        public void Attack(Entity enemy) {
            Console.WriteLine($"Boss attacks! {Math.Max(0, Damage - enemy.Armor)} of damage!");
            
            enemy.Health -= Math.Max(0, Damage - enemy.Armor);
            enemy.Armor = 0;
        }

        public Entity CloneEn() {
            return this with {Effects = Effects.Select(x => x.Clone()).ToList()};
        }
    }

    public enum SpellType {
        MagicMissile = 0,
        Drain = 1,
        Shield = 2,
        Poison = 3,
        Recharge = 4
    }

    public class Spell {
        public SpellType Type { get; set; }
        public int SpellMana { get; set; }
        public bool IsEffect { get; set; }
        public int EffectTimer { get; set; }

        public Spell Clone() {
            return (Spell)MemberwiseClone();
        }

        public void Apply(Entity hero, Entity boss) {
            if (IsEffect && EffectTimer == 0) return;
            
            switch (Type) {
                case SpellType.MagicMissile:
                    Console.WriteLine("Magic Missile deals 4 damage");
                    boss.Health -= 4;
                    break;
                case SpellType.Drain:
                    Console.WriteLine("Drain deals 2 damage, recover 2 damage");
                    boss.Health -= 2;
                    hero.Health += 2;
                    break;
                case SpellType.Shield:
                    Console.WriteLine($"Shield is used, timer: {EffectTimer - 1}");
                    hero.Armor = 7;
                    break;
                case SpellType.Poison:
                    Console.WriteLine($"Poison is used, timer: {EffectTimer - 1}");
                    boss.Health -= 3;
                    break;
                case SpellType.Recharge:
                    Console.WriteLine($"Recharge is used, timer: {EffectTimer - 1}");
                    hero.Mana += 101;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (IsEffect) {
                EffectTimer--;
            }
        }

        public void Cast(Entity hero) {
            hero.Effects.Add(this);
        }
    }
}
