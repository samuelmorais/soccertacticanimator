using System;
using UnityEngine;

namespace FootTactic
{
    public class FTAnimationBall : FTAnimation
    {
        private Transform ballTransformHelper;
        public new FTKeyFrameBall[] KeyFrames
        {
            get { return m_keyframes; }
            set { m_keyframes = value; }
        }

        public FTAnimationBall(){
            SetTransformHelper();
        }

        void SetTransformHelper(){
            ballTransformHelper = GameObject.Find("BallTransformHelper").transform;
        }

        
        protected new FTKeyFrameBall[] m_keyframes = new FTKeyFrameBall[m_maxKeyframes];

        public void AddKeyframe(FTKeyFrameBall kf)
        {
            // Ensure keyframe array is not full
            if (m_nextFreeIndex < m_maxKeyframes)
            {
                // Does a keyframe already exists at the given time?
                FTKeyFrameBall existing = Array.Find(m_keyframes, o => o != null ? o.time.Equals(kf.time) : false);
                if (existing != null)
                {
                    // Update the existing keyframe
                    existing.vec = kf.vec;
                    existing.BodyPart = kf.BodyPart;
                    existing.Action = kf.Action;
                    existing.playerIndex = kf.playerIndex;
                }
                else
                {
                    // Add new keyframe
                    m_keyframes[m_nextFreeIndex] = kf;
                    m_nextFreeIndex++;
                }
                
                if (m_nextFreeIndex == 2 && !kf.Rotation.Equals(Mathf.Infinity) && kf.animIndex > 0)
                {
                    
                    m_keyframes[0].Rotation = kf.Rotation;
                }
            }
            else
            {
                throw new OverflowException("The keyframe array is full.");
            }
        }

        public new void RemoveKeyframe(TimeSpan time)
        {
            // Find the keyframe in the array
            int existingIndex = Array.FindIndex(m_keyframes, o => o != null ? o.time.Equals(time) : false);
            if (existingIndex != -1)
            {
                m_keyframes[existingIndex] = null;

                // Resort the array if we have removed any except the last in the list
                if (existingIndex < m_nextFreeIndex - 1)
                {
                    SortArray();
                }

                m_nextFreeIndex--;
            }
        }

        public new void RemoveLastKeyFrame()
        {
            if (m_keyframes.Length > 0)
            {
                m_keyframes[m_keyframes.Length - 1] = null;

                m_nextFreeIndex--;
            }
        }

        // Resize the keyframe array
        public new void SetMaxKeyframes(int newMax)
        {
            Array.Resize(ref m_keyframes, newMax);
        }

        // Sort keyframes in ascending time order
        public new void SortArray()
        {
            // Sort keyframes and shuffle nulls to the end of the array
            Array.Sort(m_keyframes, (a, b) => a == null ? 1 : b == null ? -1 : a.CompareTo(b));
        }

        // Get the Vector3 value for the given time
        public new FTBallFrameData GetValue(TimeSpan time)
        {
            // Check sizes
            if (m_nextFreeIndex == 0)
            {
                throw new Exception("Attempted to get value from TinyAnim but no keyframes had been added to the array yet!");
            }
            else if (m_nextFreeIndex == 1)
            {
                return new FTBallFrameData(m_keyframes[0].vec, m_keyframes[0].playerIndex, m_keyframes[0].BodyPart, m_keyframes[0].Action);
            }
            else if (m_nextFreeIndex > 1)
            {
                int index = Array.BinarySearch(m_keyframes, 0, m_nextFreeIndex, new FTKeyFrame(new Vector3(0, 0, 0), time));

                if (index < 0)
                {
                    // Not an exact match so fetch the closest index using bitwise complement
                    index = ~index;
                }

                return GetKeyBallFrameDataAtIndex(index, time);

            }

            // This should never happen but it is here to catch anomolies and satisfy the compiler
            throw new Exception("Attempted to get value from TinyAnim but no Vector was returned!");
        }

        

        public FTBallFrameData GetKeyBallFrameDataAtIndex(int index, TimeSpan time)
        {
            FTKeyFrameBall prevKeyFrame = index - 1 >= 0 && index - 1 < m_nextFreeIndex ? m_keyframes[index - 1] : index == 0 ? m_keyframes[0] : m_keyframes[m_nextFreeIndex - 1];
            FTKeyFrameBall nextKeyFrame = index < m_nextFreeIndex && index >= 0 ? m_keyframes[index] : index >= m_nextFreeIndex ? m_keyframes[m_nextFreeIndex - 1] : m_keyframes[0];

            if (prevKeyFrame != null && nextKeyFrame != null && index < m_nextFreeIndex && index > 0)
            {
                return new FTBallFrameData(                        
                        GetTrajectoryPoint(
                            prevKeyFrame,
                            nextKeyFrame,
                            time),
                        prevKeyFrame.playerIndex,
                        prevKeyFrame.BodyPart,
                        prevKeyFrame.Action
                    );
            }

            return new FTBallFrameData(
                        prevKeyFrame.vec,
                        prevKeyFrame.playerIndex,
                        prevKeyFrame.BodyPart,
                        prevKeyFrame.Action
                    );
        }

        Vector3 GetTrajectoryPoint(FTKeyFrame start, FTKeyFrame end, TimeSpan time){
            float currentTime = (float)time.TotalSeconds - (float)start.time.TotalSeconds;
            float totalTime = (float)end.time.TotalSeconds - (float)start.time.TotalSeconds;
            if(currentTime == 0) return start.vec;
            Vector3 vGravity = Physics.gravity;		
		    var launchPos = start.vec;
            var targetPos = end.vec;
            ballTransformHelper.position = launchPos;		
		    ballTransformHelper.transform.rotation = Quaternion.identity;
            Vector3 lauchingVector = (targetPos - launchPos - 0.5f * vGravity * totalTime * totalTime)/totalTime;		
            var initialSpeed = lauchingVector.magnitude;
            Vector3 lauchingVectorHorizontal = new Vector3(targetPos.x,0,targetPos.z) - new Vector3(launchPos.x,0,launchPos.z);
            float directionAngle = 180f + Mathf.Atan2(lauchingVectorHorizontal.x, lauchingVectorHorizontal.z) * Mathf.Rad2Deg;
            float theta = Vector3.Angle (lauchingVector, lauchingVectorHorizontal);	
            float velocityY = lauchingVector.magnitude * Mathf.Sin(theta/180f * Mathf.PI);
            float velocityZ = lauchingVector.magnitude * Mathf.Cos(theta/180f * Mathf.PI);;
            float height = (velocityY * currentTime) + 0.5f * vGravity.y * currentTime * currentTime;
            float distance = velocityZ * currentTime;                    
            Vector3 offset = new Vector3 (0, height, -1f * distance );            
            offset =  Quaternion.Euler(0, directionAngle, 0) *  offset;                
            Vector3 point =  launchPos + ballTransformHelper.InverseTransformDirection(offset);
            return point;     	
        }


      


    }
}
