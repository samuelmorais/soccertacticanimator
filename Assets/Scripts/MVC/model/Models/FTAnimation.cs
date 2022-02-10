using System;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

namespace FootTactic
{
    public class FTAnimation
    {
        public bool debug;
        public FTAnimation()
        {
            
        }
        bool moved = false;
        public int NumFrames
        {
            get { return m_nextFreeIndex; }
            set { m_nextFreeIndex = value; }
        }

        public FTKeyFrame[] KeyFrames
        {
            get { return m_keyframes; }
            set { m_keyframes = value; }
        }

        public bool Moved { get => moved; set => moved = value; }

        // Max number of keyframes
        protected const int m_maxKeyframes = 50;

        // The keyframe array
        protected FTKeyFrame[] m_keyframes = new FTKeyFrame[m_maxKeyframes];

        // The index of the next free keyframe in the array. Also doubles as the number of keyframes in the array.
        protected int m_nextFreeIndex = 0;

        // Add (or update) a keyframe to the array. This also sorts the array so that the keyframes remain in ascending time order.
        // If a keyframe already exists at the given time, update that keyframe instead.
        public virtual void AddKeyframe(FTKeyFrame kf)
        {
            // Ensure keyframe array is not full
            if (m_nextFreeIndex < m_maxKeyframes)
            {
                // Does a keyframe already exists at the given time?
                FTKeyFrame existing = Array.Find(m_keyframes, o => o != null ? o.time.Equals(kf.time) : false);
                if (existing != null)
                {
                    // Update the existing keyframe
                    existing.vec = kf.vec;
                    existing.Rotation = kf.Rotation;
                    existing.animIndex = kf.animIndex;
                    moved = true;
                }
                else
                {
                    // Add new keyframe
                    m_keyframes[m_nextFreeIndex] = kf;
                    m_nextFreeIndex++;
                    if (kf.time.TotalSeconds > 0) moved = true;
                }
                if (!kf.Rotation.Equals(Mathf.Infinity) && !kf.Rotation.Equals(0))
                {
                    Debug.Log("Adding keyframe Rotation:    " + kf.Rotation + " animIndex: " + kf.animIndex + " next free index: " + m_nextFreeIndex);

                }
                if (m_nextFreeIndex == 2 && !kf.Rotation.Equals(Mathf.Infinity) && kf.animIndex > 0)
                {
                    Debug.Log("Worked! animIndex: "+kf.animIndex);
                    m_keyframes[0].Rotation = kf.Rotation;
                }
            }
            else
            {
                throw new OverflowException("The keyframe array is full.");
            }
        }

        public FTKeyFrame LastKeyFrame(TimeSpan time, bool skipCurrentFrame = true)
        {
            if(skipCurrentFrame)
                time = time.Add(TimeSpan.FromSeconds(-0.01f));
            else
            {
                time = time.Add(TimeSpan.FromSeconds(0.01f));
            }
            // Check sizes
            if (m_nextFreeIndex == 0)
            {
                throw new Exception("Attempted to get value from TinyAnim but no keyframes had been added to the array yet!");
            }
            else if (m_nextFreeIndex == 1)
            {
                return m_keyframes[0];
            }
            else if (m_nextFreeIndex > 1)
            {
                int index = Array.BinarySearch(m_keyframes, 0, m_nextFreeIndex, new FTKeyFrame(new Vector3(0, 0, 0), time));

                if (index < 0)
                {
                    // Not an exact match so fetch the closest index using bitwise complement
                    index = ~index;
                }
                Debug.Log("Time: " + time.TotalSeconds);
                return GetPrevKeyFrameTime(index, time);

            }

            // This should never happen but it is here to catch anomolies and satisfy the compiler
            throw new Exception("Attempted to get value from TinyAnim but no Vector was returned!");
           
        }

        public FTKeyFrame NextKeyFrame(TimeSpan time)
        {
            
            // Check sizes
            if (m_nextFreeIndex == 0)
            {
                throw new Exception("Attempted to get value from TinyAnim but no keyframes had been added to the array yet!");
            }
            else if (m_nextFreeIndex == 1)
            {
                return m_keyframes[0];
            }
            else if (m_nextFreeIndex > 1)
            {
                int index = Array.BinarySearch(m_keyframes, 0, m_nextFreeIndex, new FTKeyFrame(new Vector3(0, 0, 0), time));

                if (index < 0)
                {
                    // Not an exact match so fetch the closest index using bitwise complement
                    index = ~index;
                }
                
                return GetNextKeyFrameTime(index, time);

            }

            // This should never happen but it is here to catch anomolies and satisfy the compiler
            throw new Exception("Attempted to get value from TinyAnim but no Vector was returned!");
        }

        // Removes a keyframe from the array at a given time.
        // Will also sort the array if necessary.
        public void RemoveKeyframe(TimeSpan time)
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

        // Resize the keyframe array
        public void SetMaxKeyframes(int newMax)
        {
            Array.Resize(ref m_keyframes, newMax);
        }

        // Sort keyframes in ascending time order
        public void SortArray()
        {
            // Sort keyframes and shuffle nulls to the end of the array
            Array.Sort(m_keyframes, (a, b) => a == null ? 1 : b == null ? -1 : a.CompareTo(b));
        }

        // Get the Vector3 value for the given time
        public FTFrameData GetValue(TimeSpan time)
        {
            // Check sizes
            if (m_nextFreeIndex == 0)
            {
                throw new Exception("Attempted to get value from TinyAnim but no keyframes had been added to the array yet!");
            }
            else if (m_nextFreeIndex == 1)
            {
                return new FTFrameData(0, Quaternion.identity, m_keyframes[0].vec,0, true, m_keyframes[0].animIndex);
            }
            else if (m_nextFreeIndex > 1)
            {
                int index = Array.BinarySearch(m_keyframes, 0, m_nextFreeIndex, new FTKeyFrame(new Vector3(0, 0, 0), time));
                
                if (index < 0)
                {
                    // Not an exact match so fetch the closest index using bitwise complement
                    index = ~index;
                }
                
                return GetKeyFrameDataAtIndex(index, time);
                    
            }

            // This should never happen but it is here to catch anomolies and satisfy the compiler
            throw new Exception("Attempted to get value from TinyAnim but no Vector was returned!");
        }

        public FTKeyFrame GetPrevKeyFrameTime(int index, TimeSpan time)
        {
           
            FTKeyFrame prevKeyFrame =  index - 1 >= 0 && index - 1 < m_nextFreeIndex ? m_keyframes[index - 1] : index == 0 ? m_keyframes[0] : m_keyframes[m_nextFreeIndex - 1];
            while(prevKeyFrame.time.Equals(time) && index > 1)
            {
                index--;
                prevKeyFrame = index - 1 >= 0 && index - 1 < m_nextFreeIndex ? m_keyframes[index - 1] : index == 0 ? m_keyframes[0] : m_keyframes[m_nextFreeIndex - 1];               
            }
            return prevKeyFrame;

        }

        public FTKeyFrame GetNextKeyFrameTime(int index, TimeSpan time)
        {

            FTKeyFrame nextKeyFrame = index < m_nextFreeIndex? m_keyframes[index] : m_keyframes[m_nextFreeIndex - 1]; //index < m_nextFreeIndex && index >= 0 ? m_keyframes[index] : index >= m_nextFreeIndex ? m_keyframes[m_nextFreeIndex - 1] : m_keyframes[0];
            while (nextKeyFrame.time.Equals(time) && index < m_nextFreeIndex -1)
            {
                index++;
                nextKeyFrame = index + 1 < m_nextFreeIndex ? m_keyframes[index + 1] : index < m_nextFreeIndex ? m_keyframes[index] : m_keyframes[m_nextFreeIndex - 1];
            }
            return nextKeyFrame;

        }

        public virtual FTFrameData GetKeyFrameDataAtIndex(int index, TimeSpan time)
        {
           
            FTKeyFrame prevKeyFrame = index - 1 >= 0 && index - 1 < m_nextFreeIndex  ?  m_keyframes[index - 1] : index == 0 ? m_keyframes[0] : m_keyframes[m_nextFreeIndex - 1];
            FTKeyFrame nextKeyFrame = index < m_nextFreeIndex && index >= 0 ? m_keyframes[index] : index >= m_nextFreeIndex ? m_keyframes[m_nextFreeIndex-1] : m_keyframes[0];
            
            FTKeyFrame prevPrevKeyFrame = index - 2 < m_nextFreeIndex && index > 1 ? m_keyframes[index - 2] : ((index > 0  && index - 1 < m_nextFreeIndex) ? m_keyframes[index -1] : prevKeyFrame);
            FTKeyFrame nextNextKeyFrame = index + 1 < m_nextFreeIndex && index + 1 >= 0 ? m_keyframes[index + 1] : ((index >= 0 && index < m_nextFreeIndex) ? m_keyframes[index] : nextKeyFrame);

            if (prevKeyFrame != null && nextKeyFrame != null && index < m_nextFreeIndex && index > 0)
            {
                int animationIndex = prevKeyFrame.animIndex;

                if(animationIndex == 0)
                {
                   
                    Quaternion rot = Vector3.Distance(nextKeyFrame.vec, prevKeyFrame.vec) > 0.1f  ? Quaternion.LookRotation(new Vector3(nextKeyFrame.vec.x,0, nextKeyFrame.vec.z) - new Vector3(prevKeyFrame.vec.x, 0, prevKeyFrame.vec.z), Vector3.up) : !prevKeyFrame.Rotation.Equals(Mathf.Infinity) ? Quaternion.Euler(0, prevKeyFrame.Rotation,0) : !nextKeyFrame.Rotation.Equals(Mathf.Infinity) ? Quaternion.Euler(0, nextKeyFrame.Rotation, 0) : Quaternion.identity;
                    Quaternion rotPrev = Vector3.Distance(prevKeyFrame.vec ,prevPrevKeyFrame.vec) > 0.1f && prevKeyFrame.Rotation.Equals(Mathf.Infinity) ? Quaternion.LookRotation(prevKeyFrame.vec - prevPrevKeyFrame.vec, Vector3.up) : prevKeyFrame.Rotation.Equals(Mathf.Infinity) ? rot : Quaternion.Euler(0,prevKeyFrame.Rotation,0);
                    Quaternion rotNext = Vector3.Distance(prevKeyFrame.vec, prevPrevKeyFrame.vec) > 0.1f && prevKeyFrame.Rotation.Equals(Mathf.Infinity) ? Quaternion.LookRotation(nextNextKeyFrame.vec - nextKeyFrame.vec, Vector3.up) : nextKeyFrame.Rotation.Equals(Mathf.Infinity) ? rot : Quaternion.Euler(0, nextKeyFrame.Rotation, 0); //nextNextKeyFrame.vec != nextKeyFrame.vec ? Quaternion.LookRotation(nextNextKeyFrame.vec - nextKeyFrame.vec, Vector3.up) : rot;
                    float prevTime = (float)prevKeyFrame.time.TotalSeconds;
                    float nextTime = (float)nextKeyFrame.time.TotalSeconds;
                    float currTime = (float)time.TotalSeconds;
                    float iniDiff = (currTime - prevTime) * 2f + 0.5f;
                    float finDiff = 0.5f - 2f * (nextTime - currTime);
                    bool startTransition = iniDiff > 0.5f && iniDiff < 1f;
                    bool endTransition = finDiff > 0 && finDiff < 0.5f;

                    if (startTransition && endTransition && rotPrev != rotNext)
                    {
                        
                        rot = Quaternion.Lerp(rotPrev, rotNext, (iniDiff + finDiff) / 2f);
                    }
                    if (startTransition && rotPrev != rot)
                    {
                       
                        rot = Quaternion.Lerp(rotPrev, rot, iniDiff);
                    }
                    else if (endTransition && rotNext != rot)
                    {
                        
                        rot = Quaternion.Lerp(rot, rotNext, finDiff);
                    }

                    return new FTFrameData(
                        Vector3.Distance(prevKeyFrame.vec, nextKeyFrame.vec) / ((float)nextKeyFrame.time.Subtract(prevKeyFrame.time).TotalSeconds > 0 ? (float)nextKeyFrame.time.Subtract(prevKeyFrame.time).TotalSeconds : 0.01f),
                        rot,
                        TinyLerp(
                            prevKeyFrame,
                            nextKeyFrame,
                            time),
                        (float)(time.TotalSeconds - prevKeyFrame.time.TotalSeconds),
                        false,
                        animationIndex
                    );
                }
                else
                {
                    Quaternion rot = Quaternion.Euler(0, prevKeyFrame.Rotation, 0);// Quaternion.LookRotation(nextKeyFrame.vec - prevPrevKeyFrame.vec, Vector3.up);
                    return new FTFrameData(
                        0,
                        rot,
                        prevKeyFrame.vec,
                        (float)(time.TotalSeconds - prevKeyFrame.time.TotalSeconds),
                        false,
                        animationIndex
                    );
                }
                
            }
            else if (index == 0)
            {
                // Time < first keyframe time - Return first keyframe's value
                Quaternion rot = Quaternion.identity;

                if (m_keyframes[1] != null)
                {
                    rot = Vector3.Distance(m_keyframes[1].vec , m_keyframes[0].vec) > 0.1f ? Quaternion.LookRotation(new Vector3(m_keyframes[1].vec.x,0, m_keyframes[1].vec.z)- new Vector3(m_keyframes[0].vec.x, 0, m_keyframes[0].vec.z), Vector3.up) : Quaternion.Euler(0, m_keyframes[0].Rotation != Mathf.Infinity? m_keyframes[0].Rotation:0, 0);
                }
                if (!m_keyframes[0].Rotation.Equals(Mathf.Infinity))
                {
                    rot = Quaternion.Euler(0, m_keyframes[0].Rotation, 0);
                }
                return new FTFrameData(                    
                    0,
                    rot,
                    m_keyframes[0].vec,
                    0,
                    m_keyframes[1] == null,
                    0
                    );
            }
            else if (index >= m_nextFreeIndex - 1 && m_nextFreeIndex  >=  2)
            {
                // Time > last keyframe time - Return last keyframe's value
                Quaternion rot = Vector3.Distance(m_keyframes[m_nextFreeIndex - 1].vec, m_keyframes[m_nextFreeIndex - 2].vec) > 0.1f?Quaternion.LookRotation(new Vector3(m_keyframes[m_nextFreeIndex - 1].vec.x, 0, m_keyframes[m_nextFreeIndex - 1].vec.z) - new Vector3(m_keyframes[m_nextFreeIndex - 2].vec.x, 0, m_keyframes[m_nextFreeIndex - 2].vec.z), Vector3.up) : Quaternion.identity;
                if (!m_keyframes[m_nextFreeIndex - 1].Rotation.Equals(Mathf.Infinity))
                {
                    rot = Quaternion.Euler(0, m_keyframes[m_nextFreeIndex - 1].Rotation, 0);
                }
                return new FTFrameData(                    
                    0,
                    rot,
                    m_keyframes[m_nextFreeIndex - 1].vec,
                    (float)time.TotalSeconds - (float)m_keyframes[m_nextFreeIndex - 1].time.TotalSeconds,
                    false,
                    m_keyframes[m_nextFreeIndex -1 ].animIndex
                    );
            }

            return new FTFrameData(                    
                0,
                Quaternion.Euler(new Vector3(10f,130f,243f)),
                Vector3.zero,
                0,
                true,
                0
            );
            
        }

        // Interpolate between two FTKeyFrames
        protected Vector3 TinyLerp(FTKeyFrame start, FTKeyFrame end, TimeSpan time)
        {
            // Calculate blend value depending on what is chosen (percentage or smooth step)
            float percent = 0f;
            
            
            percent = Percent(time, start.time, end.time);
            

            return new Vector3(start.vec.x + (percent * (end.vec.x - start.vec.x)),
                               start.vec.y + (percent * (end.vec.y - start.vec.y)),
                               start.vec.z + (percent * (end.vec.z - start.vec.z)));
        }
        

        // Calculate percentage blend value of the current given time between a start and end time
        protected float Percent(TimeSpan current, TimeSpan start, TimeSpan end)
        {
            float tCurrent = (float)current.Ticks;
            float tStart = (float)start.Ticks;
            float tEnd = (float)end.Ticks;
            return (tCurrent - tStart) / (tEnd - tStart);
        }

        // Ensures a given value stays between two boundaries
        private float Clamp(float value, float startValue, float endValue)
        {
            value = Math.Min(value, Math.Max(startValue, endValue));
            value = Math.Max(value, Math.Min(startValue, endValue));
            return value;
        }
    }
}
