﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NLog;
using NuGetGallery.Operations.Common;

namespace NuGetGallery.Operations
{
    public abstract class OpsTask : ICommand
    {
        private const string CommandSuffix = "Task";

        private CommandAttribute _commandAttribute;
        private List<string> _arguments = new List<string>();
        private Logger _logger;
        protected internal Logger Log
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(GetType().Name)); }
            internal set { _logger = value; }
        }

        public CommandAttribute CommandAttribute
        {
            get
            {
                if (_commandAttribute == null)
                {
                    _commandAttribute = GetCommandAttribute();
                }
                return _commandAttribute;
            }
        }

        public IList<string> Arguments
        {
            get { return _arguments; }
        }
        
        [Import]
        public HelpCommand HelpCommand { get; set; }

        [Option("Gets help for this command", AltName = "?")]
        public bool Help { get; set; }

        [Option("Instead of performing any write operations, the command will just output what it WOULD do. Read operations are still performed.", AltName = "!")]
        public bool WhatIf { get; set; }

        public void Execute()
        {
            if (WhatIf)
            {
                Log.Info("Running in WhatIf mode");
            }
            if (Help)
            {
                HelpCommand.ViewHelpForCommand(CommandAttribute.CommandName);
            }
            else
            {
                ValidateArguments();
                ExecuteCommand();
            }
        }

        public abstract void ExecuteCommand();
        
        public virtual void ValidateArguments()
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method does quite a bit of processing.")]
        public virtual CommandAttribute GetCommandAttribute()
        {
            var attributes = GetType().GetCustomAttributes(typeof(CommandAttribute), true);
            if (attributes.Any())
            {
                return (CommandAttribute)attributes.FirstOrDefault();
            }

            // Use the command name minus the suffix if present and default description
            string name = GetType().Name;
            int idx = name.LastIndexOf(CommandSuffix, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                name = name.Substring(0, idx);
            }
            if (!String.IsNullOrEmpty(name))
            {
                return new CommandAttribute(name, TaskResources.DefaultCommandDescription);
            }
            return null;
        }
    }
}
