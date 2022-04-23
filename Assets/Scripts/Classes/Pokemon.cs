using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum Status
{
    PARALYZED,
    POISONED,
    BADLY_POISONED,
    BURNED,
    FROZEN,
    INFATUATED,
    ASLEEP,
    NONE
}

public enum Stat
{
    ATTACK,
    DEFENSE,
    SPECIAL_ATTACK,
    SPECIAL_DEFENSE,
    SPEED,
    EVASION,
    ACCURACY
}

public class StatBlock
{
    // Base stats
    public int base_hp;
    public int base_atk;
    public int base_def;
    public int base_spAtk;
    public int base_spDef;
    public int base_spe;

    // Adjusted stats
    public int current_hp;
    public int current_atk;
    public int current_def;
    public int current_spAtk;
    public int current_spDef;
    public int current_spe;

    // Weird stats--accuracy is chance to hit,
    // evasion is chance to randomly dodge
    public int accuracy_stage;
    public int evasion_stage;

    // Stat stages--I hate this
    public int atk_stage;
    public int spAtk_stage;
    public int def_stage;
    public int spDef_stage;
    public int spe_stage;

    public StatBlock(int hp, int atk, int def, int spAtk, int spDef, int spe)
    {
        base_hp         = hp;
        base_atk        = atk;
        base_def        = def;
        base_spAtk      = spAtk;
        base_spDef      = spDef;
        base_spe        = spe;

        resetStats();
    }
   
    public StatBlock()
    {
        base_hp = 100;
        base_atk = 100;
        base_def = 100;
        base_spAtk = 100;
        base_spDef = 100;
        base_spe = 100;

        resetStats();
    }

    // Reset all stats to the base as they should be
    public void resetStats()
    {
        current_hp      = base_hp;
        current_atk     = base_atk;
        current_def     = base_def;
        current_spAtk   = base_spAtk;
        current_spDef   = base_spDef;
        current_spe     = base_spe;

        atk_stage   = 0;
        spAtk_stage = 0;
        def_stage   = 0;
        spDef_stage = 0;
        spe_stage   = 0;

        accuracy_stage = 0;
        evasion_stage  = 0;
    }

    public void resetStatsExceptHP()
    {
        // boom
        int hp = current_hp;
        resetStats();
        current_hp = hp;
    }

    // calculate current values based on stage
    public void calculateCurrentStatValues()
    {
        current_atk   = (int)(base_atk   * getMultiplierFromStage(atk_stage   ));
        current_def   = (int)(base_def   * getMultiplierFromStage(def_stage   ));
        current_spAtk = (int)(base_spAtk * getMultiplierFromStage(spAtk_stage ));
        current_spDef = (int)(base_spDef * getMultiplierFromStage(spDef_stage ));
        current_spe   = (int)(base_spe   * getMultiplierFromStage(spe_stage   ));
    }

    public float getMultiplierFromStage(int stage)
    {
        // https://www.dragonflycave.com/mechanics/stat-stages
        switch (stage)
        {
            case -6:
                return 0.25f;
            case -5:
                return 0.28f;
            case -4:
                return 0.33f;
            case -3:
                return 0.4f;
            case -2:
                return 0.5f;
            case -1:
                return 0.66f;
            case 0:
                return 1.0f;
            case 1:
                return 1.5f;
            case 2:
                return 2.0f;
            case 3:
                return 2.5f;
            case 4:
                return 3.0f;
            case 5:
                return 3.5f;
            case 6:
                return 4.0f;
            default:
                return 1.0f;
        }
    }

    public void changeStage(Stat stat, int amountToChange)
    {
        switch(stat)
        {
            case Stat.ATTACK:
                atk_stage = getNewStage(atk_stage, amountToChange);
                break;
            case Stat.DEFENSE:
                def_stage = getNewStage(def_stage, amountToChange);
                break;
            case Stat.SPECIAL_ATTACK:
                spAtk_stage = getNewStage(spAtk_stage, amountToChange);
                break;
            case Stat.SPECIAL_DEFENSE:
                spDef_stage = getNewStage(spDef_stage, amountToChange);
                break;
            case Stat.SPEED:
                spe_stage = getNewStage(spe_stage, amountToChange);
                break;
            case Stat.EVASION:
                if(evasion_stage + amountToChange < 0)
                {
                    evasion_stage = 0;
                }
                if(evasion_stage + amountToChange > 6)
                {
                    evasion_stage = 6;
                }
                break;
            case Stat.ACCURACY:
                if (accuracy_stage + amountToChange < 0)
                {
                    accuracy_stage = 0;
                }
                if (evasion_stage + amountToChange > 6)
                {
                    accuracy_stage = 6;
                }
                break;
            default:
                break;
        }

        calculateCurrentStatValues();
    }

    public static int getNewStage(int currentStage, int amountToChange)
    {
        return (Math.Abs(currentStage + amountToChange) > 6 ? currentStage : currentStage + amountToChange);
    }

    public float getChanceToHit()
    {
        switch(this.accuracy_stage)
        {
            case -6:
                return 0.284f;
            case -5:
                return 0.329f;
            case -4:
                return 0.404f;
            case -3:
                return 0.427f;
            case -2:
                return 0.533f;
            case -1:
                return 0.674f;
            default:
                return 1.0f;
        }
    }
    
    public float getChanceToEvade()
    {
        switch(this.evasion_stage)
        {
            case 1:
                return 0.341f;
            case 2:
                return 0.5f;
            case 3:
                return 0.558f;
            case 4:
                return 0.596f;
            case 5:
                return 0.671f;
            case 6:
                return 0.716f;
            default:
                return 0.001f;
        }
    }

    public bool buffedOrDebuffed()
    {
        return this.current_atk    != this.base_atk
            || this.current_def    != this.base_def
            || this.current_spAtk  != this.base_spAtk
            || this.current_spDef  != this.base_spDef
            || this.current_spe    != this.base_spe
            || this.evasion_stage  != 0
            || this.accuracy_stage != 0;
    }
}

public class Pokemon //: MonoBehaviour
{
    // Base stats
    public StatBlock statBlock;

    // Types/attributes
    public List<Attribute> types;

    // Moveset
    public List<Move> moves;

    // Status effect--the base statuses, plus the weird ones
    public List<Status> statuses;
    public bool flinch = false;
    public bool confused = false;
    public bool conscious = true;
    public bool active = false;

    // Game objects
    public Sprite sprite;
    public Sprite frontSprite;

    // Miscellany
    public string name;
    public Vector3 spriteScale; // hackety hack hack

    public Pokemon(string name)
    {
        this.name = name;
        this.statBlock = new StatBlock();
        this.types = new List<Attribute>();
        this.moves = new List<Move>();
        this.statuses = new List<Status>();
    }

    public Pokemon(string name, StatBlock stats)
    {
        this.name = name;
        this.statBlock = stats;
        this.types = new List<Attribute>();
        this.moves = new List<Move>();
        this.statuses = new List<Status>();
    }

    public void takeDamage(int injury)
    {
        statBlock.current_hp -= injury;
        if (statBlock.current_hp <= 0)
        {
            statBlock.current_hp = 0;
            conscious = false;
        }
    }

    public bool addStatusEffect(Status status)
    {
        if (!this.statuses.Contains(status))
        {
            this.statuses.Add(status);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void executeMove(Move move, Pokemon defender)
    {
        bool cantMove = false;
        bool stillAsleep = false;
        List<Status> removeTheseStatuses = new List<Status>();

        // Start by applying status effect
        // TODO - what the heck is infatuation and frozen?
        foreach (Status status in statuses)
        {
            switch(status)
            {
                case Status.ASLEEP:
                    int awake = UnityEngine.Random.Range(0, 3); // one in 3 chance
                    if(awake <= 0)
                    {
                        DialogueManager.instance.addToQueue(this.name + " woke up!");
                        removeTheseStatuses.Add(status);
                        break;
                    }
                    stillAsleep = true;
                    break;
                case Status.BURNED:
                    DialogueManager.instance.addToQueue(this.name + " took damage from burn!");
                    int burn = this.statBlock.base_hp / 16;
                    this.takeDamage(burn);
                    break;
                case Status.POISONED:
                    DialogueManager.instance.addToQueue(this.name + " took damage from poison!");
                    int poison = this.statBlock.base_hp / 16;
                    this.takeDamage(poison);
                    break;
                case Status.BADLY_POISONED:
                    DialogueManager.instance.addToQueue(this.name + " took damage from poison!");
                    int badpoison = this.statBlock.base_hp / 8;
                    this.takeDamage(badpoison);
                    break;
                case Status.PARALYZED:
                    int noLongerParalyzed = UnityEngine.Random.Range(0, 3); // one in 3 chance to come out of paralysis
                    int paralyzed = UnityEngine.Random.Range(0, 4); // one in 4 chance to not be able to move
                    if( noLongerParalyzed <= 0)
                    {
                        DialogueManager.instance.addToQueue(this.name + " is cured of its paralysis!");
                        removeTheseStatuses.Add(status);
                        break;
                    }
                    cantMove = true;
                    break;
                case Status.FROZEN:
                case Status.INFATUATED:
                    break;
                default:
                    break;
            }
        }

        if( this.statBlock.current_hp <= 0 )
        {
            this.conscious = false;
            return;
        }

        // remove statuses that are no longer applicable
        foreach (Status status in removeTheseStatuses)
        {
            this.statuses.Remove(status);
        }

        if( stillAsleep ) // sleep
        {
            DialogueManager.instance.addToQueue(this.name + " is fast asleep!");
            return;
        }
        if( cantMove ) // paralysis
        {
            DialogueManager.instance.addToQueue(this.name + " is paralyzed! It can't move!");
            return;
        }
        if (this.flinch)
        {
            this.flinch = false;
            DialogueManager.instance.addToQueue(this.name + " flinched!");
            return;
        }

        if (this.confused)
        {
            int comeOutOfConfusion = UnityEngine.Random.Range(0, 3); // one in 3 chance
            if(comeOutOfConfusion <= 0)
            {
                DialogueManager.instance.addToQueue(this.name + " is no longer confused!");
            }
            else
            {
                DialogueManager.instance.addToQueue(this.name + " is confused!");
                int confusionChance = UnityEngine.Random.Range(0, 2); // one in 2 chance
                if (confusionChance <= 0)
                {
                    DialogueManager.instance.addToQueue(this.name + " hurt itself in its confusion!");

                    this.takeConfusionDamage();
                }
                return;

            }
        }

        DialogueManager.instance.addToQueue(this.name + " used " + move.name + "!");
        move.execute(this, defender);
    }

    public void executeMoveAt(int moveNum, Pokemon defender)
    {
        this.executeMove(moves[moveNum], defender);
    }

    public void takeConfusionDamage()
    {
        // [(((2A/5 + 2)*B*40)/C)/50] + 2
        int injury = ((((((2 * 100) / 5) + 2) * this.statBlock.current_atk * 40) / this.statBlock.current_def) / 50) + 2;
        this.takeDamage(injury);
    }

    public void executeRandomMove(Pokemon defender)
    {
        int index = UnityEngine.Random.Range(0, moves.Count);
        this.executeMoveAt(index, defender);
    }

    public static int calculateAdjustedInjury(Pokemon attacker, Pokemon defender, Move move, bool crit)
    {
        // Level is always 100
        const int level = 100;

        // The base power of the move, which will be multiplied by a bunch of nonsense
        int basePower;

        // The relevant attack/defense skills that the move uses
        // and the ratio between them
        int attack;
        int defense;
        float atkDefRatio;

        // The final adjusted injury
        int injury;

        // All of the multipliers possible for the damage calculation
        // Note that level is ignored, as all pokemon are of the same level
        float randomMultiplier;
        float stabMultiplier = 1.0f;
        float typeMultiplier;
        float critMultiplier = (crit ? 2.0f : 1.0f);
        float burnMultiplier = 1.0f;

        // Determine type multiplier first, as that's the most difficult step
        List<Attribute> defenderTypes = defender.types;

        // Get the type multiplier first
        typeMultiplier = findTypeMultiplier(defenderTypes, move);

        // If the type of the move is in the type of the attacker, then
        // get a stab bonus of 1.5.  Ignoring adaptability rules here.
        foreach (Attribute attr in attacker.types)
        {
            if (attr == move.attribute)
            {
                stabMultiplier = 1.5f;
            }
        }

        // If the attacker is burned, then its physical moves are less effective
        foreach (Status status in attacker.statuses)
        {
            if (status == Status.BURNED && !move.special)
            {
                burnMultiplier = 0.5f;
            }
        }

        // Get the base power from the move
        basePower = move.power;

        // Calculate attack and defense
        attack  = (move.special ? attacker.statBlock.current_spAtk : attacker.statBlock.current_atk);
        defense = (move.special ? defender.statBlock.current_spDef : defender.statBlock.current_def);

        atkDefRatio = (float)attack / defense;

        // Finally, calculate random multiplier as a random multiplier between 0.85 and 1.0
        randomMultiplier = UnityEngine.Random.Range(0.85f, 1.0f);

        // Use a complex and weird formula to determine the injury
        injury = (int)(((((((2 * level) / 5) + 2) * basePower * atkDefRatio) / 50) + 2)
            * randomMultiplier * stabMultiplier * typeMultiplier * critMultiplier * burnMultiplier);

        return injury;
    }

    public static float findTypeMultiplier(List<Attribute> defenderTypes, Move move)
    {
        float multiplier = 1.0f;
        Attribute moveAttribute = move.attribute;

        foreach (Attribute type in defenderTypes)
        {
            if(type.weakOrResistant(moveAttribute))
            {
                multiplier *= type.resistantWeakAgainst[moveAttribute];
            }
        }

        return (multiplier > 4.0f ? 4.0f : multiplier);
    }
}
