// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Rr+tH1EiybdQL4VS81tnlUcWL2I5HLYJOpEdUvbAGvc4dJmHjruqet6uHlYYzRSOzXDiXQC+HcT9w2CPCVEOUn0M7uZcNLP+LNeB7KUkiILKTAqz+NU3/3sLS4famduGZjypcSmjssupViH30WObgszbV8mCbo23EanOtYmsvFDYk6G93nNQX8YHwmKmkD2ouH3Wfg1xoTnd5bW6U36oj4OJlhXGYDjzTvp3z0JuJNhLKtVytQeEp7WIg4yvA80DcoiEhISAhYaBmDWNAMGSJvmwLOBi95utCgYMHDTk1ht9F618X6Hwgw/odH8dUdkCB4SKhbUHhI+HB4SEhSQ9Qkwzcldgtk3XbqFgs2vAQ74+WaEmcx3bByZBKmSVaAE3HIeGhIWE");
        private static int[] order = new int[] { 11,4,7,13,12,6,10,8,9,11,11,11,12,13,14 };
        private static int key = 133;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
