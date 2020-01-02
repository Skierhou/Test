using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("CanSeeObject")]
    [TaskCategory("Physics")]
    public class CanSeeObject : Conditional
    {
        [Tooltip("detectRange")]
        public SharedFloat detectRange;

        private GameObject m_Target;

        private bool canSeeObject = false;

        public override void OnStart()
        {
            m_Target = CharacterManager.Instance.Player.gameObject;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Target != null && !canSeeObject)
            {
                MapManager.Instance.currentRoom.AStarFind(transform.position, m_Target.transform.position, out int g);
                if (g * 0.1f < (float)detectRange.GetValue())
                {
                    canSeeObject = true;
                }
            }
            return canSeeObject ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            canSeeObject = false;
        }
    }
}