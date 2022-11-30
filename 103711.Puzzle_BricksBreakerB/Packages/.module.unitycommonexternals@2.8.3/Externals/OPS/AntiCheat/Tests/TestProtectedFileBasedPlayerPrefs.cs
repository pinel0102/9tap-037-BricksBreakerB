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
    /// Test AntiCheats Protected File Based PlayerPrefs.
    /// </summary>
    public class TestProtectedFileBasedPlayerPrefs
    {
        private String playerFilePath;

        [SetUp]
        public void Setup()
        {
            playerFilePath = ProtectedFileBasedPlayerPrefs.FilePath;

            ProtectedFileBasedPlayerPrefs.FilePath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "AntiCheatTestFile.obj");
        }

        [TearDown]
        public void Teardown()
        {
            ProtectedFileBasedPlayerPrefs.FilePath = playerFilePath;
        }

        // HasKey
        #region HasKey

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsHasKey()
        {
            String var_Key = "OPS_AntiCheat_Test";
            
            Assert.AreEqual(false, ProtectedFileBasedPlayerPrefs.HasKey(var_Key));

            ProtectedFileBasedPlayerPrefs.SetInt(var_Key, 1234);

            Assert.AreEqual(1234, ProtectedFileBasedPlayerPrefs.GetInt(var_Key));

            Assert.AreEqual(true, ProtectedFileBasedPlayerPrefs.HasKey(var_Key));

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Int
        #region Int

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsInt()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetInt(var_Key, 1234);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetInt(var_Key), 1234);

            ProtectedFileBasedPlayerPrefs.SetInt(var_Key, 123456);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetInt(var_Key), 123456);

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Float
        #region Float

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsFloat()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetFloat(var_Key, 1234.123f);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetFloat(var_Key), 1234.123f);

            ProtectedFileBasedPlayerPrefs.SetFloat(var_Key, 123456.12345f);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetFloat(var_Key), 123456.12345f);

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // String
        #region String

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsString()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetString(var_Key, "Hello World!");

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetString(var_Key), "Hello World!");

            ProtectedFileBasedPlayerPrefs.SetString(var_Key, "Hello World, nice to meet you!");

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetString(var_Key), "Hello World, nice to meet you!");

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Bool
        #region Bool

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsBool()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetBool(var_Key, false);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetBool(var_Key), false);

            ProtectedFileBasedPlayerPrefs.SetBool(var_Key, true);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetBool(var_Key), true);

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Vector
        #region Vector

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector2()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetVector2(var_Key, Vector2.zero);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector2(var_Key).ToString(), Vector2.zero.ToString());

            ProtectedFileBasedPlayerPrefs.SetVector2(var_Key, new Vector2(1, 2));

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector2(var_Key).ToString(), new Vector2(1, 2).ToString());

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector3()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetVector3(var_Key, Vector3.zero);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector3(var_Key).ToString(), Vector3.zero.ToString());

            ProtectedFileBasedPlayerPrefs.SetVector3(var_Key, new Vector3(1, 2, 3));

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector3(var_Key).ToString(), new Vector3(1, 2, 3).ToString());

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsVector4()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetVector4(var_Key, Vector4.zero);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector4(var_Key).ToString(), Vector4.zero.ToString());

            ProtectedFileBasedPlayerPrefs.SetVector4(var_Key, new Vector4(1, 2, 3, 4));

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetVector4(var_Key).ToString(), new Vector4(1, 2, 3, 4).ToString());

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion

        // Quaternion
        #region Quaternion

        [UnityTest]
        public IEnumerator Test_ProtectedPlayerPrefsQuaternion()
        {
            String var_Key = "OPS_AntiCheat_Test";

            ProtectedFileBasedPlayerPrefs.SetQuaternion(var_Key, Quaternion.identity);

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetQuaternion(var_Key).ToString(), Quaternion.identity.ToString());

            ProtectedFileBasedPlayerPrefs.SetQuaternion(var_Key, new Quaternion(1, 2, 3, 4));

            Assert.AreEqual(ProtectedFileBasedPlayerPrefs.GetQuaternion(var_Key).ToString(), new Quaternion(1, 2, 3, 4).ToString());

            ProtectedFileBasedPlayerPrefs.DeleteKey(var_Key);

            yield return null;
        }

        #endregion
    }
}
