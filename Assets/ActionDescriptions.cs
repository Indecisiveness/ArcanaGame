//Object containing basic info as well as effects for moves
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActionDescriptions", menuName = "ScriptableObjects/ActionDescriptions", order = 2)]
public class ActionDescriptions : ScriptableObject
{


    public Dictionary<string, UnitInfo.Action> actions = new Dictionary<string, UnitInfo.Action>{
        {"Basic Attack",
            new UnitInfo.Action("Basic Attack", 1, 0, "single", "A basic attack that hits adjacent targets","enemy", 0) },
        {"Basic Attack 2",
        new UnitInfo.Action("Basic Attack 2", 2, 0, "single", "A basic attack that hits at a distance","enemy", 0)},
        {"Basic Attack 3",
        new UnitInfo.Action("Basic Attack 3", 3, 0, "single", "A basic attack that hits at a distance","enemy", 0)},
        {"Fire",
        new UnitInfo.Action("Fire", 4, 0, "single", "A spell producing a ball of fire", "enemy", 6)},
        {"Earth",
        new UnitInfo.Action("Earth", 4, 1, "blast", "A spell that turns an area into sand and harms targets","enemies allies", 6)},
        {"Windstorm",
        new UnitInfo.Action("Windstorm", 4, 2, "blast", "A spell producing blast of air, knocking targets back", "enemies allies", 6)},
        {"Tidal Wave",
        new UnitInfo.Action("Tidal Wave", 4, 1, "blast", "A spell producing a rushing tide, replacing area with water", "enemies allies", 6)},
        {"Siphon",
        new UnitInfo.Action("Siphon", 4, 0, "single", "A spell draining MP from a single target to replenish the caster", "enemy", 0)},
        {"Project Force",
        new UnitInfo.Action("Project Force", 3, 0, "single", "A surge of chi which hits at a distance, using Pentacles", "enemy", 4)},
        {"Leg Sweep",
        new UnitInfo.Action("Leg Sweep", 1, 0, "single", "A low kick that knocks the target down, preventing movement", "enemy", 4)},
        {"Reversal Throw",
        new UnitInfo.Action("Reversal Throw", 1, 0, "single", "A throw that uses the target's strength against them", "enemy", 6)},
        {"Harness Chakra",
        new UnitInfo.Action("Harness Chakra", 0, 0, "single", "Focus spiritual energy to exchange HP and MP", "self", 0)},
        {"One-Two Punch",
        new UnitInfo.Action("One-Two Punch", 1, 0, "single", "Strike a target and set up for the next attack", "enemy", 4)},
        {"Steal",
        new UnitInfo.Action("Steal", 1, 0, "single", "Pickpocket a foe, gaining an equipped item", "enemy", 0)},
        {"Hamstring",
        new UnitInfo.Action("Hamstring", 1, 0, "single", "Slice a foe's tendons, reducing their mobility for three turns", "enemy", 4) },
        {"Tripwire",
        new UnitInfo.Action("Tripwire", 5, 3, "square", "Mark a large square with an immobilizing trap", "area", 4)},
        {"Garrote",
        new UnitInfo.Action("Garrote", 1, 0, "single", "Aim for the throat, preventing spellcasting for a turn", "enemy", 6)},
        {"Disarm",
        new UnitInfo.Action("Disarm", 1, 0, "single", "Employ a fencing maneuver to occupy the target's weapon, preventing attack", "enemy", 0)},
        {"Scorpion Sting",
        new UnitInfo.Action("Scorpion Sting", 1, 0, "single", "Channel the power of the scorpion to damage and poison a target", "enemy", 4)},
        {"Perforate",
        new UnitInfo.Action("Perforate", 1, 0, "single", "Perform two attacks in quick succession at a single target", "enemy", 6)},
        {"Seal Chakras",
        new UnitInfo.Action("Seal Chakras", 1, 0, "single", "Perform a precise blow at a pressure point, preventing MP regen", "enemy", 4)},
        {"Circling Vulture",
        new UnitInfo.Action("Circling Vulture", 1, 0, "single", "Perform a glancing blow, distracting the target and setting up a follow-up", "enemy", 4)},
        {"Charging Rhino",
        new UnitInfo.Action("Charging Rhino", 2, 0, "line", "A powerful thrust which can pierce an enemy and move the user forward", "enemies allies", 4)},
        {"Cure",
        new UnitInfo.Action("Cure", 3, 1, "blast", "A spell which restores health to targets in an area", "allies enemies", 4)},
        {"Dispel",
        new UnitInfo.Action("Dispel", 3, 1, "blast", "A spell which removes all lingering effects from targets in an area", "allies enemies", 2)},
        {"Sap Strength",
        new UnitInfo.Action("Sap Strength", 3, 0, "single", "A spell draining a target's vitality, reducing Swords temporarily", "enemy", 6)},
        {"Magic Shield",
        new UnitInfo.Action("Magic Shield", 3, 1, "blast", "Protect targets from the next spell that would cause damage", "allies enemies", 6)},
        {"Demotivate" ,
        new UnitInfo.Action("Demotivate", 3, 1, "blast", "A spell which deals MP damage to targets in a blast", "enemies allies", 6)},
        {"Prevention",
        new UnitInfo.Action("Prevention", 3, 1, "blast", "A spell which reduces the damage allies take from physical attacks", "allies", 6)},
        {"Ice",
        new UnitInfo.Action("Ice", 4, 1, "blast", "A spell that deals damage and slows targets", "enemies allies", 6)},
        {"Quicksand",
        new UnitInfo.Action("Quicksand", 4, 1, "blast", "A spell that drags targets in an area down, reducing their elevation and damaging them", "enemies allies", 6)},
        {"Lightning",
        new UnitInfo.Action("Lightning", 4, 2, "blast", "A spell that damages selected targets in an area with a chaining bolt of lightning", "enemies", 6)},
        {"Eruption",
        new UnitInfo.Action("Eruption", 4, 0, "single", "A spell that opens a geyser of magma below a target, causing severe damage", "enemy", 6)},
        { "Levitation",
        new UnitInfo.Action("Levitation", 0, 2, "blast", "A spell which telekinetically raises allies, allowing them to move without obstacle", "allies", 6)},
        {"Replenish",
        new UnitInfo.Action("Replenish", 4, 0, "single", "Gift an ally with spell power", "ally", 0)},
        {"Frenzy",
        new UnitInfo.Action("Frenzy", 0, 0, "single", "Enter a rage, preventing skill use but increasing damage", "self", 0)},
        {"Cleave",
        new UnitInfo.Action("Cleave", 1, 0, "double", "Attack two targets in range", "enemies", 8)},
        {"Jump Attack",
        new UnitInfo.Action("Jump Attack", 3, 0, "single", "Leap up to two squares, then attack an enemy", "enemy", 4)},
        {"War Cry",
        new UnitInfo.Action("War Cry", 0, 2, "blast", "Boost Swords for self and targets within 2", "self", 4)},
        {"Blooddrinker",
        new UnitInfo.Action("Blooddrinker", 1, 0, "single", "Attack a target and restore health based on damage dealt", "enemy", 6)},
        {"Double-Deal",
        new UnitInfo.Action("Double-Deal", 0, 0, "single", "Perform two actions this turn", "self", 4)},
        {"Sleight of Hand",
        new UnitInfo.Action("Sleight of Hand", 2, 0, "double", "Swap the positions of two targets in range", "any", 2)},
        {"Card Trick",
        new UnitInfo.Action("Card Trick", 5, 2, "blast", "Raise or lower a stat by 2 for all targets in the affected area", "any", 4)},
        {"March",
         new UnitInfo.Action("March", 3, 0, "single", "Move another unit instantly", "any", 2)},
        {"Shell Game",
        new UnitInfo.Action("Shell Game", 4, 0, "single", "Trade positions with an ally", "ally", 2)},
        {"Bamboozle",
        new UnitInfo.Action("Bamboozle", 3, 0, "single", "Prevent ability use for a turn", "enemy", 2)},
        {"Holy Smite",
        new UnitInfo.Action("Holy Smite", 1, 0, "single", "Channel divine energy to harm an enemy", "enemy", 4) },
        {"Pillar of Light",
        new UnitInfo.Action("Pillar of Light", 5, 1, "cross", "A spell summoning a holy beam to harm targets in an area", "enemies allies", 8)},
        {"Lay on Hands",
            new UnitInfo.Action("Lay on Hands", 1, 0, "single", "Restore health to a nearby ally", "ally", 4) },
        {"Knight's Challenge",
        new UnitInfo.Action("Knight's Challenge", 1, 0, "single", "Strike an enemy and goad them into targetting you", "enemy", 4)},
        {"Careful Blow",
        new UnitInfo.Action("Careful Blow", 1, 0, "single", "Strike an enemy while keeping shield raised, blocking the next hit", "enemy", 4)},
        {"Drain",
        new UnitInfo.Action("Drain", 3, 0, "single", "A spell that siphons life from a foe, gaining it back as health", "enemy", 0)},
        {"Tainted Rain",
        new UnitInfo.Action("Tainted Rain", 4, 1, "blast", "A spell that damages and poison targets in an area", "enemies allies", 6)},
        {"Rapid Recovery",
        new UnitInfo.Action("Rapid Recovery", 3, 1, "blast", "A spell that stimulates targets' cells, restoring health each turn", "allies enemies", 6)},
        {"Distemper",
        new UnitInfo.Action("Distemper", 4, 0, "single", "Cause a target to go berserk, losing use of abilities and spells", "any", 4)},
        {"Serum Tincture",
        new UnitInfo.Action("Serum Tincture", 3, 2, "blast", "Immediately restore MP to targets in area", "allies enemies", 4)},
        {"Blood Inversion",
        new UnitInfo.Action("Blood Inversion", 0, 0, "single", "Reduce HP to 1 and restore an equal amount of MP. Exchange S and W. Damage increases based on missing HP", "self", 0)},
        {"Telekinetic Grasp",
        new UnitInfo.Action("Telekinetic Grasp", 4, 0, "single", "A spell that crushes a target with psychic power and pulls them towards you", "enemy", 6)},
        {"Warp Strike",
        new UnitInfo.Action("Warp Strike", 1, 0, "single", "Strike a target, then vanish to a nearby location", "enemy", 4)},
        {"Flame Sweep",
        new UnitInfo.Action("Flame Sweep", 1, 0, "horLine", "Wreath sword in flame, then make a arcing cut", "enemies allies", 6)},
        {"Spectral Weapon",
        new UnitInfo.Action("Spectral Weapon", 4, 0, "single", "A spell summons a phantasmal sword to strike from a distance, using Swords", "enemy", 4)},
        {"Quake Strike",
        new UnitInfo.Action("Quake Strike", 1, 4, "strLine", "Call on the power of earth to strike targets in a line", "enemies allies", 6)},
        {"Blade Storm",
        new UnitInfo.Action("Blade Storm", 4, 1, "blast", "Summon a flurry of blades, applying bonuses from Swords to the spell", "enemies allies", 6)},

        {"Buffet",
        new UnitInfo.Action("Buffet", 3, 0, "enemy", "Beat wings to produce cutting winds", "enemy", 4)},

        {"Widen Blast",
        new UnitInfo.Action("Widen Blast", 0, 0, "single", "Increase the radius and MP cost of next move used", "self", 0)}

        };

    public Dictionary<string, string[]> actionEffects = new Dictionary<string, string[]>
    {
        { "Basic Attack", new[]
            {
            "attack,swords,melee"
        }
        },
        {"Basic Attack 2", new[]
            {
            "attack,swords"
            }
        },
        {"Basic Attack 3", new[]
            {
            "attack,swords"
            }
        },
        {"Fire", new[]
            {
               "attack,wands,spell,fire"
            }
        },
        {"Earth", new[]
            {
               "attack,wands,spell,area,earth", "tile,sand,0,none"
            }
        },
        {"Windstorm", new[]
            {
               "attack,wands,spell,area,wind", "move1,target,1,forced,push"
            }
        },
        {"Tidal Wave", new[]
            {
               "attack,wands,spell,area,water", "tile,water,0,none"
            }
        },
        {"Siphon", new[]
            {
                "attack,wands,spell,MPDam,siphon"
            }

        },

            {"Project Force", new[]
            {
               "attack,pentacles"
            }
        },
            {"Leg Sweep", new[]
            {
               "attack,swords,melee", "status,immobile,1"
            }
        },
                {"Reversal Throw", new[]
            {
               "attack,swords,melee,reversal", "move1,target,1,free"
            }
        },
        { "Harness Chakra", new[]
            {
               "self,chakraswap,0", "self,another,1"
            }
        },
        {"One-Two Punch", new[]
            {
               "attack,swords,melee","self,combo,2"
            }
        },

        { "Steal", new[]
            {
               "status,itemsteal,-1"
                        }
        },
        { "Hamstring", new[]
            {
               "attack,swords,melee", "status,move-1,3"
            }
        },
        { "Tripwire", new[]
            {
               "tile,null,0,tripwire"
            }
        },
        { "Garrote", new[]
            {
               "attack,swords,melee", "status,silence,2"
            }
        },
        { "Disarm", new[]
            {
               "status,disarm,2"
            }
        },
       {"Scorpion Sting", new[]
            {
               "attack,swords,melee", "status,poison,3"
            }
        },
        { "Perforate", new[]
            {
               "attack,swords,melee", "attack,swords,melee"
            }
        },
        { "Seal Chakras", new[]
            {
               "attack,swords,melee", "status,seal,1"
            }
        },
        { "Circling Vulture", new[]
            {
               "attack,swords,melee", "move1,self,1,free"
            }
        },
        { "Charging Rhino", new[]
            {
               "move1,self,1,forced,charge", "attack,swords,melee"
            }
        },
        { "Cure", new[]
            {
               "attack,wands,healing,area,spell"
            }
        },
        { "Dispel", new[]
            {
              "status,dispel,0"
            }
        },
        { "Sap Strength", new[]
            {
               "attack,wands,spell", "status,swords-2,1"
            }
        },
        { "Magic Shield", new[]
            {
               "status,shield,2"
            }
        },
        {"Demotivate", new[]
            {
                "attack,wands,spell,area,MPDam"
            }
        },
        { "Prevention", new[]
            {
               "status,physBlock,3"
            }
        },
        { "Ice", new[]
            {
               "attack,wands,spell,area,water,wind", "status,move-1,1"
            }
        },
        { "Quicksand", new[]
            {
               "attack,wands,spell,area,water,earth", "tile,none,-1,none"
            }
        },
        { "Lightning", new[]
            {
               "attack,wands,spell,area,wind,fire,enemies"
            }
        },
        { "Eruption", new[]
            {
               "attack,wands,spell,fire,earth"
            }
        },
        { "Levitation", new[]
            {
               "status,noTerrain,noElevation,3,3"
            }
        },
        { "Replenish", new[]
            {
               "attack,pentacles,replenish"
            }
        },
        { "Frenzy", new[]
            {
               "status,rage,3"
            }
        },
        { "Cleave", new[]
            {
               "attack,swords,melee"
            }
        },
        { "Jump Attack", new[]
            {
               "move1,self,2,free,noTerrain,noElevation", "attack,swords,melee"
            }
        },
        { "War Cry", new[]
            {
               "status,swords2,3","self,another,1"
            }
        },
        { "Blooddrinker", new[]
            {
            "attack, swords, melee, drain"
            }
        },
        { "Double-Deal", new[]
            {
               "self,double,1"
            }
        },
        { "Sleight of Hand", new[]
            {
               "status,swap,0"
            }
        },
        { "Card Trick", new[]
            {
               "effect,card" //needs a pop-up
            }
        },
        { "March", new[]
            {
               "move1,target,4,free"
            }
        },
        { "Shell Game", new[]
            {
               "status,swap,0"
            }
        },
        { "Bamboozle", new[]
            {
               "status,disable,2"
            }
        },
        { "Holy Smite", new[]
            {
               "attack,swords,melee,smite,holy"
            }
        },
        { "Pillar of Light", new[]
            {
               "attack,wands,spell,area,pentAvg,holy"
            }
        },
        { "Blessing", new[]
            {
               "status,pent4,3","status,cups4,3"
            }
        },
        { "Lay on Hands", new[]
            {
               "attack,wands,healing"
            }
        },
        { "Knight's Challenge", new[]
            {
               "attack,swords,melee", "status,challenge,2"
            }
        },
        { "Careful Blow", new[]
            {
               "attack,swords,melee", "self,careful,1"
            }
        },
        { "Drain", new[]
            {
               "attack,wands,spell,drain"
            }
        },
        { "Tainted Rain", new[]
            {
               "attack,wands,spell,area", "status,poison,3"
            }
        },
        { "Rapid Recovery", new[]
            {
               "status,regen,3"
            }
        },
        { "Distemper", new[]
            {
               "status,rage,3"
            }
        },
        { "Serum Tincture", new[]
            {
               "status,MPRegen,0", "self,MPRegen,0"
            }
        },
        { "Blood Inversion", new[]
            {
               "self,bloodinversion,3","self,another,1"
            }
        },
        { "Telekinetic Grasp", new[]
            {
               "attack,wands,spell", "move1,target,2,pull,forced"
            }
        },
        { "Warp Strike", new[]
            {
               "attack,swords,melee", "move1,self,2,free,noTerrain,noElevation"
            }
        },
        { "Flame Sweep", new[]
            {
               "attack,swords,melee,wandHigh,fire"
            }
        },
        { "Spectral Weapon", new[]
            {
               "attack,swords,spell"
            }
        },
        { "Quake Strike", new[]
            {
               "attack,swords,melee", "attack,wands,spell,earth,area"
            }
        },
        { "Blade Storm", new[]
            {
               "attack,wands,spell,area,swordBuff"
            }
        },


        { "Buffet", new[]
            {
            "attack,cups,wind"
            }
        },

        {"Widen Blast", new[]
            {
             "self,widen,another,1,1"
            }
        }

    };


    


}
