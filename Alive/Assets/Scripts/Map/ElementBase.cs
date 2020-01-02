using System;
using System.Collections.Generic;

public enum ElementType
{
    None,
    Wall,
    DestroyedItem,
    Door,
    Obstacle, 
};

public class ElementBase:BaseClassMono
{
    public ElementType ElementType;
}