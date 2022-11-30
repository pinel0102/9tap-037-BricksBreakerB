using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.TestTools;

// NUnit
using NUnit.Framework;

// OPS - AntiCheat
using OPS.AntiCheat.Prefs;

namespace OPS.AntiCheat.Tests
{
    /// <summary>
    /// Test AntiCheats Protected PlayerPrefs.
    /// </summary>
    public class TestProtectedPlayerPrefs
    {
        // HasKey
        #region HasKey

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsHasKey()
        {
            String var_Key = "OPS_AntiCheat_Test";

            Assert.AreEqual(false, ProtectedPlayerPrefs.HasKey(var_Key));

            ProtectedPlayerPrefs.SetInt(var_Key, 1234);

            Assert.AreEqual(1234, ProtectedPlayerPrefs.GetInt(var_Key));

            Assert.AreEqual(true, ProtectedPlayerPrefs.HasKey(var_Key));

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Int
        #region Int

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsInt()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetInt(var_Key, 1234);

            Assert.AreEqual(ProtectedPlayerPrefs.GetInt(var_Key), 1234);

            ProtectedPlayerPrefs.SetInt(var_Key, 123456);

            Assert.AreEqual(ProtectedPlayerPrefs.GetInt(var_Key), 123456);

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Float
        #region Float

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsFloat()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetFloat(var_Key, 1234.123f);

            Assert.AreEqual(ProtectedPlayerPrefs.GetFloat(var_Key), 1234.123f);

            ProtectedPlayerPrefs.SetFloat(var_Key, 123456.12345f);

            Assert.AreEqual(ProtectedPlayerPrefs.GetFloat(var_Key), 123456.12345f);

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // String
        #region String

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsString()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetString(var_Key, "Hello World!");

            Assert.AreEqual(ProtectedPlayerPrefs.GetString(var_Key), "Hello World!");

            ProtectedPlayerPrefs.SetString(var_Key, "Hello World, nice to meet you!");

            Assert.AreEqual(ProtectedPlayerPrefs.GetString(var_Key), "Hello World, nice to meet you!");

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Bool
        #region Bool

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsBool()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetBool(var_Key, false);

            Assert.AreEqual(ProtectedPlayerPrefs.GetBool(var_Key), false);

            ProtectedPlayerPrefs.SetBool(var_Key, true);

            Assert.AreEqual(ProtectedPlayerPrefs.GetBool(var_Key), true);

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Vector
        #region Vector

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector2()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetVector2(var_Key, Vector2.zero);

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector2(var_Key).ToString(), Vector2.zero.ToString());

            ProtectedPlayerPrefs.SetVector2(var_Key, new Vector2(1, 2));

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector2(var_Key).ToString(), new Vector2(1, 2).ToString());

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector3()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetVector3(var_Key, Vector3.zero);

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector3(var_Key).ToString(), Vector3.zero.ToString());

            ProtectedPlayerPrefs.SetVector3(var_Key, new Vector3(1, 2, 3));

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector3(var_Key).ToString(), new Vector3(1, 2, 3).ToString());

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector4()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetVector4(var_Key, Vector4.zero);

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector4(var_Key).ToString(), Vector4.zero.ToString());

            ProtectedPlayerPrefs.SetVector4(var_Key, new Vector4(1, 2, 3, 4));

            Assert.AreEqual(ProtectedPlayerPrefs.GetVector4(var_Key).ToString(), new Vector4(1, 2, 3, 4).ToString());

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Quaternion
        #region Quaternion

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsQuaternion()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedPlayerPrefs.SetQuaternion(var_Key, Quaternion.identity);

            Assert.AreEqual(ProtectedPlayerPrefs.GetQuaternion(var_Key).ToString(), Quaternion.identity.ToString());

            ProtectedPlayerPrefs.SetQuaternion(var_Key, new Quaternion(1, 2, 3, 4));

            Assert.AreEqual(ProtectedPlayerPrefs.GetQuaternion(var_Key).ToString(), new Quaternion(1, 2, 3, 4).ToString());

            ProtectedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion
    }
}
