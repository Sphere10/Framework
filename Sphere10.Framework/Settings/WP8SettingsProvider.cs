//-----------------------------------------------------------------------
// <copyright file="WP8SettingsProvider.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

#if __WP8__

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Sphere10.Framework {

    public class WP8SettingsProvider : BaseSettingsProvider
    {
        private const string NullValueString = "NULL!8be93b18-cecb-4be0-8d05-d6c43123aa4e";
        public override bool ContainsSetting(Type settingsObjectType, object id = null)
        {
            Preconditions.CheckNotNull(settingsObjectType, "settingsObjectType");
            var key = PropertyNameToKey(settingsObjectType, id, "ID");
            return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        }


        protected override SettingsObject LoadInternal(Type settingsObjectType, object id = null)
        {
            var setting = Tools.Object.Create(settingsObjectType) as SettingsObject;
            setting.ID = id;
            foreach (var field in GetSettingsFields(settingsObjectType))
            {
                field.SetValue(setting, LoadPropertyValue(settingsObjectType, id, field.Name, field.FieldType));
            }

            foreach (var property in GetSettingsProperties(settingsObjectType))
                property.SetValue(setting, LoadPropertyValue(settingsObjectType, id, property.Name, property.PropertyType));
            return setting;
        }

        protected override void SaveInternal(SettingsObject settings)
        {
            var settingType = settings.GetType();
            foreach (var field in GetSettingsFields(settings.GetType()))
                SavePropertyValue(settingType, settings.ID, field.Name, field.GetValue(settings));

            foreach (var property in GetSettingsProperties(settings.GetType()))
                SavePropertyValue(settingType, settings.ID, property.Name, property.GetValue(settings, null));

            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        public override void DeleteSetting(SettingsObject settings)
        {
            var settingType = settings.GetType();
            foreach (var field in GetSettingsFields(settingType))
            {
                DeletePropertyValue(settingType, settings.ID, field.Name);
            }

            foreach (var property in GetSettingsProperties(settings.GetType()))
                DeletePropertyValue(settingType, settings.ID, property.Name);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        public override void ClearSettings()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        #region Auxillary methods



        private string PropertyNameToKey(Type settingObjectType, object settingID, string propertyName)
        {
            return
                string.Format(
                    "{0}.{1}",
                    string.Format(
                        "{0}{1}",
                        settingObjectType.FullName,
                        settingID != null ? "." + ConvertExChangeType<string>(settingID) : string.Empty),
                    propertyName
                );
        }

        private object LoadPropertyValue(Type settingObjectType, object settingID, string propertyName, Type expectedValueType)
        {
            var key = PropertyNameToKey(settingObjectType, settingID, propertyName);
            object storedValue = null;

            // Check if stored value is null
            var nullCheckValue = IsolatedStorageSettings.ApplicationSettings[key];
            if (nullCheckValue == null || nullCheckValue.ToString() == NullValueString)
                return null;

            switch (GetStorageClass(expectedValueType))
            {
                case StorageClass.String:
                    storedValue = IsolatedStorageSettings.ApplicationSettings[key] as string;
                    break;
                case StorageClass.Integer:
                    storedValue = (int)IsolatedStorageSettings.ApplicationSettings[key];
                    break;
                case StorageClass.Bool:
                    storedValue = (bool)IsolatedStorageSettings.ApplicationSettings[key];
                    break;
                case StorageClass.Float:
                    storedValue = (float)IsolatedStorageSettings.ApplicationSettings[key];
                    break;
                case StorageClass.Double:
                    storedValue = (double)IsolatedStorageSettings.ApplicationSettings[key];
                    break;
                case StorageClass.DateTime:
                    storedValue = Tools.Parser.Parse<DateTime?>( IsolatedStorageSettings.ApplicationSettings[key] as string);
                    break;
                case StorageClass.XMLSerialized:
                    storedValue = XmlProvider.ReadFromString(expectedValueType, IsolatedStorageSettings.ApplicationSettings[key] as string);
                    break;
                default:
                    throw new Exception("Internal Error F1C3E9A8-BAD2-48AF-9533-E78DE234B353");
            }

            return Tools.Object.ChangeType(storedValue, expectedValueType);
        }

        private void SavePropertyValue(Type settingObjectType, object settingID, string propertyName, object value)
        {
            var key = PropertyNameToKey(settingObjectType, settingID, propertyName);
            if (value != null)
            {
                switch (GetStorageClass(value.GetType()))
                {
                    case StorageClass.String:
                        IsolatedStorageSettings.ApplicationSettings[key] = ConvertExChangeType<string>(value);
                        break;
                    case StorageClass.Integer:
                        IsolatedStorageSettings.ApplicationSettings[key] = ConvertExChangeType<int>(value);
                        break;
                    case StorageClass.Bool:
                        IsolatedStorageSettings.ApplicationSettings[key] = ConvertExChangeType<bool>(value);
                        break;
                    case StorageClass.Float:
                        IsolatedStorageSettings.ApplicationSettings[key] = ConvertExChangeType<float>(value);
                        break;
                    case StorageClass.Double:
                        IsolatedStorageSettings.ApplicationSettings[key] = ConvertExChangeType<double>(value);
                        break;
                    case StorageClass.DateTime:
                        IsolatedStorageSettings.ApplicationSettings[key] = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", ConvertExChangeType<DateTime>(value));
                        break;
                    case StorageClass.XMLSerialized:
                        IsolatedStorageSettings.ApplicationSettings[key] = XmlProvider.WriteToString(key);
                        break;
                    default:
                        throw new Exception("Internal Error 01B66BCD-FDEB-4FCF-B32F-44B2C8A2A81C");
                }
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[key] = NullValueString;
            }
        }

        private void DeletePropertyValue(Type settingObjectType, object settingID, string propertyName)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(PropertyNameToKey(settingObjectType, settingID, propertyName));
        }

        private StorageClass GetStorageClass(Type type)
        {
            var retval = StorageClass.XMLSerialized;
            TypeSwitch.DoType(type,
                TypeSwitch.Case<string>(() => retval = StorageClass.String),
                TypeSwitch.Case<bool>(() => retval = StorageClass.Bool),
                TypeSwitch.Case<bool?>(() => retval = StorageClass.Bool),
                TypeSwitch.Case<byte>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<byte?>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<sbyte>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<sbyte?>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<short>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<short?>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<ushort>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<ushort?>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<int>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<int?>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<uint>(() => retval = StorageClass.Integer),
                TypeSwitch.Case<uint?>(() => retval = StorageClass.Integer),
                /*TypeSwitch.Case<long>((x) => {throw new NotSupportedException(string.Format("Settings of type {0} not supported", x.GetType().Name)); }),
                TypeSwitch.Case<long?>((x) => {throw new NotSupportedException(string.Format("Settings of type {0} not supported", x.GetType().Name)); }),
                TypeSwitch.Case<ulong>((x) => {throw new NotSupportedException(string.Format("Settings of type {0} not supported", x.GetType().Name)); }),
                TypeSwitch.Case<ulong?>((x) => {throw new NotSupportedException(string.Format("Settings of type {0} not supported", x.GetType().Name)); }),*/
                TypeSwitch.Case<float>(() => retval = StorageClass.Float),
                TypeSwitch.Case<float?>(() => retval = StorageClass.Float),
                TypeSwitch.Case<double>(() => retval = StorageClass.Double),
                TypeSwitch.Case<double?>(() => retval = StorageClass.Double),
                TypeSwitch.Case<decimal>(() => retval = StorageClass.Double),
                TypeSwitch.Case<decimal?>(() => retval = StorageClass.Double),
                TypeSwitch.Case<DateTime>(() => retval = StorageClass.DateTime),
                TypeSwitch.Case<DateTime?>(() => retval = StorageClass.DateTime),
                TypeSwitch.Default(() => retval = StorageClass.XMLSerialized)
            );
            return retval;
        }

        private IEnumerable<FieldInfo> GetSettingsFields(Type settingsObjectType)
        {
            return settingsObjectType.GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.SetField).Where(f => Attribute.IsDefined(f, typeof(DefaultValueAttribute)));
        }

        private IEnumerable<PropertyInfo> GetSettingsProperties(Type settingsObjectType)
        {
            return settingsObjectType.GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty).Where(p => Attribute.IsDefined(p, typeof(DefaultValueAttribute)) && p.GetIndexParameters().Length == 0);
        }

        public T ConvertExChangeType<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private enum StorageClass
        {
            String,
            Integer,
            Float,
            Double,
            Bool,
            DateTime,
            XMLSerialized
        }

        #endregion
    }
 }


#endif
