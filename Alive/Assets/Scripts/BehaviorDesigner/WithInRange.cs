using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("CanSeeObject")]
    [TaskCategory("Physics")]
    public class WithInRange : Conditional
    {
        private GameObject m_Target;

        private bool inRange = false;

        public override TaskStatus OnUpdate()
        {
            if (m_Target != null)
            {
                if (Vector3.Distance(m_Target.transform.position,transform.position) < gameObject.GetComponent<AIPawn>().CFG_AttackRange)
                {
                    inRange = true;
                }
            }
            return inRange ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            inRange = false;
        }

        public override void OnReset()
        {
            m_Target = CharacterManager.Instance.Player.gameObject;
        }
    }
}