using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
namespace BlastGamePort
{
    class SaveLoadManager
    {
        static int mHightScore = 0;

        public static int HightScore
        {
            get { return mHightScore; }
            set { mHightScore = value; }
        }

        public static string OptionSettingStr { get; set; }

        public static Object LoadAppSettingValue(string Key)
        {
#if ! OS_W8
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            // If the key exists, retrieve the value.
            if (isolatedStore.Contains(Key))
            {
                return isolatedStore[Key];
            }
#else
#endif
            return null;
        }

        public static bool SaveAppSettingValue(string Key, Object value)
        {
#if ! OS_W8
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            bool valueChanged = false;

            try
            {
                // If the key exists
                if (isolatedStore.Contains(Key))
                {
                    // If the value has changed
                    if (isolatedStore[Key] != value)
                    {
                        // Store the new value
                        isolatedStore[Key] = value;
                        valueChanged = true;
                    }
                }
                // Otherwise create the key.
                else
                {
                    isolatedStore.Add(Key, value);
                    valueChanged = true;
                }
                if (valueChanged)
                {
                    isolatedStore.Save();
                }

                return valueChanged;
            }
            catch (Exception e)
            {
                return false;
            }
#else
            return false;
#endif
        }

    }
}
