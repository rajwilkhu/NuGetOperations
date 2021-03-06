﻿using System;

namespace NuGetGallery.Operations.Common
{
    public static class ArgCheck
    {
        public static void RequiredOrEnv(object value, string name, string envVar)
        {
            if (value == null) 
            {
                throw CreateRequiredOrEnvEx(name, envVar);
            }
        }

        public static void RequiredOrEnv(string value, string name, string envVar)
        {
            if (String.IsNullOrWhiteSpace(value)) 
            {
                throw CreateRequiredOrEnvEx(name, envVar);
            }
        }

        public static void Required(object value, string name)
        {
            if (value == null)
            {
                throw CreateRequiredEx(name);
            }
        }

        public static void Required(string value, string name)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw CreateRequiredEx(name);
            }
        }

        private static CommandLineException CreateRequiredEx(string name)
        {
            return new CommandLineException(String.Format(CommandHelp.Option_Required, name));
        }

        private static CommandLineException CreateRequiredOrEnvEx(string name, string envVar)
        {
            return new CommandLineException(String.Format(CommandHelp.Option_RequiredOrEnv, name, envVar));
        }
    }
}
