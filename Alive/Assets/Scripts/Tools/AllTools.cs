using System;
using System.Collections.Generic;

public enum EToolType
{
    None,
    SmallBloodDrug,
    BigBloodDrug,
    SmallMagicDrug,
    BigMagicDrug,
    Max_Count
}

[Config]
public class SmallBloodDrug : ToolBase{ }

[Config]
public class BigBloodDrug : ToolBase { }

[Config]
public class SmallMagicDrug : ToolBase { }

[Config]
public class BigMagicDrug : ToolBase { }