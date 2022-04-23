using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSpecifier
{
    NORMAL,
    FIRE,
    WATER,
    ELECTRIC,
    GRASS,
    ICE,
    FIGHTING,
    POISON,
    GROUND,
    FLYING,
    PSYCHIC,
    BUG,
    ROCK,
    GHOST,
    DRAGON,
    DARK,
    STEEL,
    FAIRY
};

public class Attribute
{
    // Type as an enum
    public TypeSpecifier type;

    // A dictionary of every other attribute and the degree to which it is strong/weak against it
    // This is monodirectional
    public Dictionary<Attribute, float> resistantWeakAgainst;

    public Attribute(TypeSpecifier t)
    {
        type = t;
        resistantWeakAgainst = new Dictionary<Attribute, float>();
    }

    public void weakTo(Attribute other)
    {
        resistantWeakAgainst.Add(other, 2.0f);
    }

    public void resistantTo(Attribute other)
    {
        resistantWeakAgainst.Add(other, 0.5f);
    }

    public void immuneTo(Attribute other)
    {
        resistantWeakAgainst.Add(other, 0.0f);
    }

    public bool weakOrResistant(Attribute other)
    {
        return resistantWeakAgainst.ContainsKey(other);
    }

    public string getName()
    {
        switch(type)
        {
            case TypeSpecifier.NORMAL:
                return "NORMAL";
            case TypeSpecifier.FIRE:
                return "FIRE";
            case TypeSpecifier.WATER:
                return "WATER";
            case TypeSpecifier.ELECTRIC:
                return "ELECTRIC";
            case TypeSpecifier.GRASS:
                return "GRASS";
            case TypeSpecifier.ICE:
                return "ICE";
            case TypeSpecifier.FIGHTING:
                return "FIGHTING";
            case TypeSpecifier.POISON:
                return "POISON";
            case TypeSpecifier.FLYING:
                return "FLYING";
            case TypeSpecifier.PSYCHIC:
                return "PSYCHIC";
            case TypeSpecifier.BUG:
                return "BUG";
            case TypeSpecifier.ROCK:
                return "ROCK";
            case TypeSpecifier.GHOST:
                return "GHOST";
            case TypeSpecifier.DRAGON:
                return "DRAGON";
            case TypeSpecifier.DARK:
                return "DARK";
            case TypeSpecifier.STEEL:
                return "STEEL";
            case TypeSpecifier.FAIRY:
                return "FAIRY";
        }
        return "";
    }
}
