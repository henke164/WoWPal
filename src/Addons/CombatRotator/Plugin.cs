﻿using System;
using WowCyborgAddonUtilities;

namespace CombatRotator
{
    public class Plugin : PluginBase
    {
        private WowAddonInstaller _addonInstaller;

        public Plugin(ApplicationSettings settings)
            : base(settings)
        {
            _addonInstaller = new WowAddonInstaller(Settings.ServerUrl, Settings.WowAddonPath);
        }
        
        public override bool HandleInput(string command, string[] args)
        {
            if (command != "rotation")
            {
                return false;
            }

            switch (args[0])
            {
                case "list":
                    ListRotations();
                    break;
                case "set":
                    HandleSetCommand(args);
                    break;
                default:
                    Console.WriteLine("Unknown command", ConsoleColor.Red);
                    break;
            }

            return true;
        }

        private void ListRotations()
        {
            _addonInstaller.FetchRotations();

            Logger.Log("Available rotations:", ConsoleColor.Yellow);
            foreach (var rot in _addonInstaller.Rotations)
            {
                Logger.Log(rot.Key, ConsoleColor.Yellow);
            }
        }

        private void HandleSetCommand(string[] commandParameters)
        {
            if (commandParameters.Length < 2)
            {
                Logger.Log("Usage example: 'rotation set furywarrior'", ConsoleColor.Red);
            }

            try
            {
                _addonInstaller.FetchRotations();
                _addonInstaller.SetRotation(commandParameters[1]);
                Logger.Log($"{commandParameters[1]} successfully selected.", ConsoleColor.Green);

                Logger.Log($"Run /reload command in WoW", ConsoleColor.White);
                Logger.Log($"The 'Single target'/'Multi target' bar is draggable.", ConsoleColor.White);
                Logger.Log($"Press Caps Lock to toggle between single or multi target.", ConsoleColor.White);
                Logger.Log($"------------------------", ConsoleColor.Yellow);
                Logger.Log($"Setup your keybindings to following:", ConsoleColor.Yellow);
                Logger.Log(_addonInstaller.GetRotationInstructions(), ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error occured: {ex.Message}", ConsoleColor.Red);
            }
        }

        public override void ShowCommands()
        {
            Logger.Log($@"
rotation list                   Display all available rotations.

rotation set <rotation name>    Set the current rotation.
            ");
        }
    }
}
