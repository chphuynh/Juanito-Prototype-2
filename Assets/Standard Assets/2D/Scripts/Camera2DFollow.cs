using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float yOffset = -1;
        public float zDistance = 0;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public bool isUnit = false;
        public bool isPlayer = false;

        private Vector3 OffVec;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            OffVec = new Vector3(0, yOffset, zDistance);
            m_LastTargetPosition = target.position + OffVec; 
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if(isUnit && target == null)
            {
                Destroy(gameObject);
            }
            
            if(isUnit && !isPlayer)
            {
                transform.localScale = new Vector3(1,Mathf.PingPong(Time.time/3,0.1f)+0.9f,1);
                
            }

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = ((target.position + OffVec) - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = (target.position + OffVec) + m_LookAheadPos;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = (target.position + OffVec);
        }
    }
}
