﻿using System;
using WowCyborg.ConsoleUtilities;
using WowCyborg.Runners;

namespace WowCyborg
{
    static class Program
    {
        public static BotRunnerBase BotRunner;

        static void Main()
        {
            ValidateAddonFiles();

            BotRunner = new BotFollower();
            RenderStartMessage();
            HandleInput();
        }

        static void RenderStartMessage()
        {
            Log($@"
 __      __             _________        ___.                        
/  \    /  \______  _  _\_   ___ \___.__.\_ |__   ___________  ____  
\   \/\/   /  _ \ \/ \/ /    \  \<   |  | | __ \ /  _ \_  __ \/ ___\ 
 \        (  <_> )     /\     \___\___  | | \_\ (  <_> )  | \/ /_/  >
  \__/\  / \____/ \/\_/  \______  / ____| |___  /\____/|__|  \___  / 
       \/                       \/\/          \/            /_____/  
-----------------------------------------------------------------------
            ", ConsoleColor.Yellow);

            ShowHelp();
        }

        static void HandleInput()
        {
            Log(">> ", ConsoleColor.White, true);
            var command = Console.ReadLine().Split(' ');

            switch (command[0])
            {
                case "addon":
                    AddonInput.HandleInputParameters(command);
                    break;

                case "rotation":
                    RotationInput.HandleInputParameters(command);
                    break;

                case "server":
                    ServerHandler.HandleInput(command);
                    break;

                case "help":
                    ShowHelp();
                    break;

                default:
                    Log("Unknown command", ConsoleColor.Red);
                    break;
            }

            HandleInput();
        }

        public static void Log(string str, ConsoleColor color, bool inLine = false)
        {
            Console.ForegroundColor = color;
            if (inLine)
            {
                Console.Write(str);
                return;
            }
            Console.WriteLine(str);
        }
                
        private static void ValidateAddonFiles()
        {
            if (!AddonInstaller.AddonExists())
            {
                AddonInput.ReinstallAddon();
            }
            else
            {
                AddonInstaller.DownloadRotations();
            }
        }

        public static void ShowHelp()
        {
            Log($@"
Help:

Commands:                       Description:
----------------------------------------------------------------------------------------------------------------
addon reload                    If this program cant see the addon on startup you will need to run this command.
                                It commands the program to find out where the addon is located on the screen.

addon reinstall                 Download latest addon files, and reinstall it.

rotation list                   Display all available rotations.

rotation set <rotation name>    Set the current rotation.

server start                    Starts a server on localhost with endpoints to control the bot.

server stop                     Stops the running server.
            ", ConsoleColor.White);
        }
    }
}
