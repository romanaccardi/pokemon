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
}
