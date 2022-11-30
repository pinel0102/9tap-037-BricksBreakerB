using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.TestTools;

// NUnit
using NUnit.Framework;

// OPS - AntiCheat
using OPS.AntiCheat.Field;

namespace OPS.AntiCheat.Tests
{
    /// <summary>
    /// Test AntiCheats Protected Fields.
    /// </summary>
    public class TestProtectedFields
    {
        // Int
        #region Int

        [UnityTest]
        public IEnumerator Test_ProtectedInt16()
        {
            // Default Init Test
            var var_Protected = new ProtectedInt16();

            Assert.AreEqual(var_Protected, (Int16)0);

            // Custom Init Test
            var_Protected = new ProtectedInt16(5);

            Assert.AreEqual(var_Protected, (Int16)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (Int16)7);

            // Compare Test
            Assert.Greater(new ProtectedInt16(2), new ProtectedInt16(1));
            Assert.GreaterOrEqual(new ProtectedInt16(2), new ProtectedInt16(2));
            Assert.Less(new ProtectedInt16(1), new ProtectedInt16(2));
            Assert.LessOrEqual(new ProtectedInt16(2), new ProtectedInt16(2));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedInt32()
        {
            // Default Init Test
            var var_Protected = new ProtectedInt32();

            Assert.AreEqual(var_Protected, (Int32)0);

            // Custom Init Test
            var_Protected = new ProtectedInt32(5);

            Assert.AreEqual(var_Protected, (Int32)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (Int32)7);

            // Compare Test
            Assert.Greater(new ProtectedInt32(2), new ProtectedInt32(1));
            Assert.GreaterOrEqual(new ProtectedInt32(2), new ProtectedInt32(2));
            Assert.Less(new ProtectedInt32(1), new ProtectedInt32(2));
            Assert.LessOrEqual(new ProtectedInt32(2), new ProtectedInt32(2));

            yield return null;
        }


        [UnityTest]
        public IEnumerator Test_ProtectedInt64()
        {
            // Default Init Test
            var var_Protected = new ProtectedInt64();

            Assert.AreEqual(var_Protected, (Int64)0);

            // Custom Init Test
            var_Protected = new ProtectedInt64(5);

            Assert.AreEqual(var_Protected, (Int64)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (Int64)7);

            // Compare Test
            Assert.Greater(new ProtectedInt64(2), new ProtectedInt64(1));
            Assert.GreaterOrEqual(new ProtectedInt64(2), new ProtectedInt64(2));
            Assert.Less(new ProtectedInt64(1), new ProtectedInt64(2));
            Assert.LessOrEqual(new ProtectedInt64(2), new ProtectedInt64(2));

            yield return null;
        }

        #endregion

        // UInt
        #region UInt

        [UnityTest]
        public IEnumerator Test_ProtectedUInt16()
        {
            // Default Init Test
            var var_Protected = new ProtectedUInt16();

            Assert.AreEqual(var_Protected, (UInt16)0);

            // Custom Init Test
            var_Protected = new ProtectedUInt16(5);

            Assert.AreEqual(var_Protected, (UInt16)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (UInt16)7);

            // Compare Test
            Assert.Greater(new ProtectedUInt16(2), new ProtectedUInt16(1));
            Assert.GreaterOrEqual(new ProtectedUInt16(2), new ProtectedUInt16(2));
            Assert.Less(new ProtectedUInt16(1), new ProtectedUInt16(2));
            Assert.LessOrEqual(new ProtectedUInt16(2), new ProtectedUInt16(2));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedUInt32()
        {
            // Default Init Test
            var var_Protected = new ProtectedUInt32();

            Assert.AreEqual(var_Protected, (UInt32)0);

            // Custom Init Test
            var_Protected = new ProtectedUInt32(5);

            Assert.AreEqual(var_Protected, (UInt32)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (UInt32)7);

            // Compare Test
            Assert.Greater(new ProtectedUInt32(2), new ProtectedUInt32(1));
            Assert.GreaterOrEqual(new ProtectedUInt32(2), new ProtectedUInt32(2));
            Assert.Less(new ProtectedUInt32(1), new ProtectedUInt32(2));
            Assert.LessOrEqual(new ProtectedUInt32(2), new ProtectedUInt32(2));

            yield return null;
        }


        [UnityTest]
        public IEnumerator Test_ProtectedUInt64()
        {
            // Default Init Test
            var var_Protected = new ProtectedUInt64();

            Assert.AreEqual(var_Protected, (UInt64)0);

            // Custom Init Test
            var_Protected = new ProtectedUInt64(5);

            Assert.AreEqual(var_Protected, (UInt64)5);

            // Assign Test
            var_Protected = 7;

            Assert.AreEqual(var_Protected, (UInt64)7);

            // Compare Test
            Assert.Greater(new ProtectedUInt64(2), new ProtectedUInt64(1));
            Assert.GreaterOrEqual(new ProtectedUInt64(2), new ProtectedUInt64(2));
            Assert.Less(new ProtectedUInt64(1), new ProtectedUInt64(2));
            Assert.LessOrEqual(new ProtectedUInt64(2), new ProtectedUInt64(2));

            yield return null;
        }

        #endregion

        // Bool
        #region Bool

        [UnityTest]
        public IEnumerator Test_ProtectedBool()
        {
            // Default Init Test
            var var_Protected = new ProtectedBool();

            Assert.AreEqual(var_Protected, false);

            // Custom Init Test
            var_Protected = new ProtectedBool(false);

            Assert.AreEqual(var_Protected, false);

            // Assign Test
            var_Protected = true;

            Assert.AreEqual(var_Protected, true);

            yield return null;
        }

        #endregion

        // Float
        #region Float

        [UnityTest]
        public IEnumerator Test_ProtectedFloat()
        {
            // Default Init Test
            var var_Protected = new ProtectedFloat();

            Assert.AreEqual(var_Protected, (float)0);

            // Custom Init Test
            var_Protected = new ProtectedFloat(5.23f);

            Assert.AreEqual(var_Protected, (float)5.23f);

            // Assign Test
            var_Protected = 7.1f;

            Assert.AreEqual(var_Protected, (float)7.1f);

            // Compare Test
            Assert.Greater(new ProtectedFloat(2), new ProtectedFloat(1));
            Assert.GreaterOrEqual(new ProtectedFloat(2), new ProtectedFloat(2));
            Assert.Less(new ProtectedFloat(1), new ProtectedFloat(2));
            Assert.LessOrEqual(new ProtectedFloat(2), new ProtectedFloat(2));

            yield return null;
        }

        #endregion

        // Double
        #region Double

        [UnityTest]
        public IEnumerator Test_ProtectedDouble()
        {
            // Default Init Test
            var var_Protected = new ProtectedDouble();

            Assert.AreEqual(var_Protected, (double)0);

            // Custom Init Test
            var_Protected = new ProtectedDouble(5.23);

            Assert.AreEqual(var_Protected, (double)5.23);

            // Assign Test
            var_Protected = 7.1;

            Assert.AreEqual(var_Protected, (double)7.1);

            // Compare Test
            Assert.Greater(new ProtectedDouble(2), new ProtectedDouble(1));
            Assert.GreaterOrEqual(new ProtectedDouble(2), new ProtectedDouble(2));
            Assert.Less(new ProtectedDouble(1), new ProtectedDouble(2));
            Assert.LessOrEqual(new ProtectedDouble(2), new ProtectedDouble(2));

            yield return null;
        }

        #endregion

        // Decimal
        #region Decimal

        [UnityTest]
        public IEnumerator Test_ProtectedDecimal()
        {
            // Default Init Test
            var var_Protected = new ProtectedDecimal();

            Assert.AreEqual(var_Protected, (decimal)0);

            // Custom Init Test
            var_Protected = new ProtectedDecimal(5.23M);

            Assert.AreEqual(var_Protected, (decimal)5.23M);

            // Assign Test
            var_Protected = 7.1M;

            Assert.AreEqual(var_Protected, (decimal)7.1M);

            // Compare Test
            Assert.Greater(new ProtectedDecimal(2), new ProtectedDecimal(1));
            Assert.GreaterOrEqual(new ProtectedDecimal(2), new ProtectedDecimal(2));
            Assert.Less(new ProtectedDecimal(1), new ProtectedDecimal(2));
            Assert.LessOrEqual(new ProtectedDecimal(2), new ProtectedDecimal(2));

            yield return null;
        }

        #endregion

        // String
        #region String

        [UnityTest]
        public IEnumerator Test_ProtectedString()
        {
            // Default Init Test
            var var_Protected = new ProtectedString("");

            Assert.AreEqual(var_Protected, String.Empty);

            // Custom Init Test
            var_Protected = new ProtectedString("Hello World!");

            Assert.AreEqual(var_Protected, "Hello World!");

            // Assign Test
            var_Protected = "Hello World, nice to meet you!";

            Assert.AreEqual(var_Protected, "Hello World, nice to meet you!");

            yield return null;
        }

        #endregion

        // DateTime
        #region DateTime

        [UnityTest]
        public IEnumerator Test_ProtectedDateTime()
        {
            // Default Init Test
            var var_Protected = new ProtectedDateTime(new DateTime(1970,1,1));

            Assert.AreEqual(var_Protected, new DateTime(1970, 1, 1));

            // Custom Init Test
            var_Protected = new ProtectedDateTime(new DateTime(2022, 9, 2));

            Assert.AreEqual(var_Protected, new DateTime(2022, 9, 2));

            // Assign Test
            var_Protected = new DateTime(2020, 1, 3);

            Assert.AreEqual(var_Protected, new DateTime(2020, 1, 3));

            yield return null;
        }

        #endregion

        // Vector
        #region Vector

        [UnityTest]
        public IEnumerator Test_ProtectedVector2()
        {
            // Default Init Test
            var var_Protected = new ProtectedVector2(Vector2.zero);

            Assert.AreEqual(var_Protected, Vector2.zero);

            // Custom Init Test
            var_Protected = new ProtectedVector2(new Vector2(1,2));

            Assert.AreEqual(var_Protected, new Vector2(1, 2));

            // Assign Test
            var_Protected = new Vector2(2, 3);

            Assert.AreEqual(var_Protected, new Vector2(2, 3));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedVector3()
        {
            // Default Init Test
            var var_Protected = new ProtectedVector3(Vector3.zero);

            Assert.AreEqual(var_Protected, Vector3.zero);

            // Custom Init Test
            var_Protected = new ProtectedVector3(new Vector3(1, 2, 3));

            Assert.AreEqual(var_Protected, new Vector3(1, 2, 3));

            // Assign Test
            var_Protected = new Vector3(2, 3, 4);

            Assert.AreEqual(var_Protected, new Vector3(2, 3, 4));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedVector4()
        {
            // Default Init Test
            var var_Protected = new ProtectedVector4(Vector4.zero);

            Assert.AreEqual(var_Protected, Vector4.zero);

            // Custom Init Test
            var_Protected = new ProtectedVector4(new Vector4(1, 2, 3, 4));

            Assert.AreEqual(var_Protected, new Vector4(1, 2, 3, 4));

            // Assign Test
            var_Protected = new Vector4(2, 3, 4, 5);

            Assert.AreEqual(var_Protected, new Vector4(2, 3, 4, 5));

            yield return null;
        }

        #endregion

        // Quaternion
        #region Quaternion

        [UnityTest]
        public IEnumerator Test_ProtectedQuaternion()
        {
            // Default Init Test
            var var_Protected = new ProtectedQuaternion(Quaternion.identity);

            Assert.AreEqual(var_Protected, Quaternion.identity);

            // Custom Init Test
            var_Protected = new ProtectedQuaternion(new Quaternion(1, 2, 3, 4));

            Assert.AreEqual(var_Protected, new Quaternion(1, 2, 3, 4));

            // Assign Test
            var_Protected = new Quaternion(2, 3, 4, 5);

            Assert.AreEqual(var_Protected, new Quaternion(2, 3, 4, 5));

            yield return null;
        }

        #endregion
    }
}
