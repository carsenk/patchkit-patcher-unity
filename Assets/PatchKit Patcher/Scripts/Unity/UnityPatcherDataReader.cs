using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PatchKit.Patcher.Debug;
using PatchKit.Patcher.Utilities;
using UnityEngine;

namespace PatchKit.Patcher.Unity
{
    public class UnityPatcherDataReader : IPatcherDataReader
    {
        private const string ForceSecretEnvironmentVariable = "PK_PATCHER_FORCE_SECRET";
        private const string ForceVersionEnvironmentVariable = "PK_PATCHER_FORCE_VERSION";

        private static readonly DebugLogger DebugLogger = new DebugLogger(typeof(UnityPatcherDataReader));
        private static readonly List<string> CommandLineArgs = Environment.GetCommandLineArgs().ToList();

        public UnityPatcherDataReader()
        {
            DebugLogger.LogConstructor();
        }

        public PatcherData Read()
        {
            DebugLogger.Log("Reading.");

            PatcherData data = new PatcherData();

            string forceAppSecret;
            if (TryReadEnvironmentVariable(ForceSecretEnvironmentVariable, out forceAppSecret))
            {
                DebugLogger.Log(string.Format("Setting forced app secret {0}", forceAppSecret));
                data.AppSecret = forceAppSecret;
            }
            else
            {
                string appSecret;

                if (!TryReadArgument("--secret", out appSecret))
                {
                    throw new ApplicationException("Unable to parse secret from command line.");
                }
                data.AppSecret = IsReadable() ? appSecret : DecodeSecret(appSecret);
            }

            string forceOverrideLatestVersionIdString;
            if (TryReadEnvironmentVariable(ForceVersionEnvironmentVariable, out forceOverrideLatestVersionIdString))
            {
                int forceOverrideLatestVersionId;

                if (int.TryParse(forceOverrideLatestVersionIdString, out forceOverrideLatestVersionId))
                {
                    DebugLogger.Log(string.Format("Setting forced version id {0}", forceOverrideLatestVersionId));
                    data.OverrideLatestVersionId = forceOverrideLatestVersionId;
                }
            }
            else
            {
                data.OverrideLatestVersionId = 0;
            }

            string relativeAppDataPath;

            if (!TryReadArgument("--installdir", out relativeAppDataPath))
            {
                throw new ApplicationException("Unable to parse app data path from command line.");
            }

            data.AppDataPath = MakeAppDataPathAbsolute(relativeAppDataPath);

            return data;
        }

        private static string MakeAppDataPathAbsolute(string relativeAppDataPath)
        {
            string path = Path.GetDirectoryName(Application.dataPath);

            if (Platform.GetRuntimePlatform() == RuntimePlatform.OSXPlayer)
            {
                path = Path.GetDirectoryName(path);
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return Path.Combine(path, relativeAppDataPath);
        }

        private static bool TryReadArgument(string argumentName, out string value)
        {
            int index = CommandLineArgs.IndexOf(argumentName);

            if (index != -1 && index < CommandLineArgs.Count - 1)
            {
                value = CommandLineArgs[index + 1];

                return true;
            }

            value = null;

            return false;
        }

        private static bool IsReadable()
        {
            return HasArgument("--readable");
        }

        private static bool HasArgument(string argumentName)
        {
            return CommandLineArgs.Contains(argumentName);
        }

        private static bool TryReadEnvironmentVariable(string argumentName, out string value)
        {
            value = Environment.GetEnvironmentVariable(argumentName);

            return value != null;
        }

        private static string DecodeSecret(string encodedSecret)
        {
            var bytes = Convert.FromBase64String(encodedSecret);

            for (int i = 0; i < bytes.Length; ++i)
            {
                byte b = bytes[i];
                bool lsb = (b & 1) > 0;
                b >>= 1;
                b |= (byte) (lsb ? 128 : 0);
                b = (byte) ~b;
                bytes[i] = b;
            }

            var chars = new char[bytes.Length/sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}