using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 所有AI行为的基类
/// </summary>
public class AICmd:BaseClass
{
    protected AICmdAction m_AIOwner;
    protected AIPawn m_Owenr;

    public static AICmd InitCmd<T>(AICmdAction inAIOwner) where T: AICmd,new()
    {
        AICmd AICmd = new T
        {
            m_AIOwner = inAIOwner,
            m_Owenr = inAIOwner.Owner
        };

        AICmd.m_AIOwner.PushAICmd(AICmd);
        return AICmd;
    }

    public virtual void Pushed()
    { }
    public virtual void Update()
    { }
    public virtual void Poped()
    { }
}