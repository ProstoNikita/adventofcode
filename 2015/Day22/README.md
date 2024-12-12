## --- Day 22: Wizard Simulator 20XX ---
Little Henry Case decides that defeating bosses with [swords and stuff](21) is boring.  Now he's playing the game with a wizard.  Of course, he gets stuck on another boss and needs your help again.

In this version, combat still proceeds with the player and the boss taking alternating turns.  The player still goes first.  Now, however, you don't get any equipment; instead, you must choose one of your spells to cast.  The first character at or below <code>0</code> hit points loses.

Since you're a wizard, you don't get to wear armor, and you can't attack normally.  However, since you do <em>magic damage</em>, your opponent's armor is ignored, and so the boss effectively has zero armor as well.  As before, if armor (from a spell, in this case) would reduce damage below <code>1</code>, it becomes <code>1</code> instead - that is, the boss' attacks always deal at least <code>1</code> damage.

Read the [full puzzle](https://adventofcode.com/2015/day/22).
