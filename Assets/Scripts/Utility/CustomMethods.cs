using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CustomMethods
{
    public static class ExtendedMathUtility
    {
        #region Vector Methods

        /// <summary>
        /// Returns the Distance between 2 vectors, ignoring their Y component
        /// </summary>
        public static float VectorXZDistance(Vector3 v1, Vector3 v2)
        {
            return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
        }

        /// <summary>
        /// Returns the Squared Distance between 2 vectors, ignoring their Y component
        /// </summary>
        public static float VectorXZDistanceSquared(Vector3 v1, Vector3 v2)
        {
            return (v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z);
        }

        /// <summary>
        /// Returns the magnitude of a vector only considering its XZ components
        /// </summary>
        public static float VectorXZMagnitude(Vector3 v)
        {
            return Mathf.Sqrt(v.x * v.x + v.z * v.z);
        }

        /// <summary>
        /// Returns the squared magnitude of a vector only considering its XZ components
        /// </summary>
        public static float VectorXZSquaredMagnitude(Vector3 v)
        {
            return v.x * v.x + v.z * v.z;
        }

        /// <summary>
        /// Returns a direction from p1 to p2, ignoring their Y difference
        /// </summary>
        public static Vector3 HorizontalDirection(Vector3 p1, Vector3 p2)
        {
            return Vector3.ClampMagnitude(new Vector3(p2.x - p1.x, 0f, p2.z - p1.z), 1f);
        }

        #endregion
    }
    public static class ExtendedDataUtility
    {
        /// <summary>
        /// Returns all fields of the specified type in the given class instance
        /// </summary>
        public static List<T> GetAllFieldsFromTypeInObject<T>(object instance)
        {
            List<T> foundVars = new();

            foreach (System.Reflection.FieldInfo field in instance.GetType().GetFields())
            {
                if (field.FieldType.Equals(typeof(T)))
                {
                    foundVars.Add((T)field.GetValue(instance));
                }
            }

            return foundVars;
        }
    }
}
