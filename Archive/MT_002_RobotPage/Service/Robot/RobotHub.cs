using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Robot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Service.Robot
{
    //One robot connection for the whole HMI (Robot page, Lot Entry recipe load)
    public static class RobotHub
    {
        private static readonly object loadLock = new object();
        private static IUrRobot robot;
        private static RobotSettingsModel settings;

        public static IUrRobot Robot
        {
            get
            {
                if(robot == null)
                {
                    robot = Global.IsOfflineMode ? (IUrRobot)new UrRobotSimulator() : new UrRobot();
                }
                return robot;
            }
        }

        public static RobotSettingsModel Settings
        {
            get
            {
                if(settings == null)
                {
                    settings = RobotSettingsService.Load();
                }
                return settings;
            }
        }

        //Called once at start-up: connect in the background when a robot IP is set (always in Offline mode)
        public static void AutoConnect()
        {
            string ip = Settings.RobotIPAddress;
            if(!Global.IsOfflineMode && string.IsNullOrWhiteSpace(ip))
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    if(!Robot.IsConnected)
                    {
                        Robot.Connect(ip);
                    }
                }
                catch(Exception ex) { LogHelper.Error(nameof(RobotHub), "Auto connect: " + ex.Message); }
            });
        }

        //New Lot: put the recipe's points on the robot through a short HMI teach session.
        //Returns null when done, otherwise the reason it failed. Blocks up to about 20 s: call off the UI thread.
        public static string LoadRecipeForLot(RobotRecipeModel recipe, KeyenceDll plc)
        {
            if(recipe == null)
            {
                return "No recipe selected.";
            }

            lock(loadLock)
            {
                try
                {
                    if(!Robot.IsConnected)
                    {
                        if(string.IsNullOrWhiteSpace(Settings.RobotIPAddress) && !Global.IsOfflineMode)
                        {
                            return "Robot IP address is not set (Robot page).";
                        }
                        Robot.Connect(Settings.RobotIPAddress);
                    }

                    if(Robot.State.InTeachMode)
                    {
                        return "Robot is in teach mode. Leave teach mode on the Robot page first.";
                    }

                    plc.Write<bool>(PLCTag.Robot_Teach_Req, true);
                    Robot.NotifyTeachRequest(true);

                    if(!waitUntil(() => Robot.InterpreterReady, 10000))
                    {
                        return "Robot did not enter teach mode. Check that the PLC allows it (no lot running, not in auto) and the robot program is running.";
                    }

                    string reply;
                    List<string> statements = RobotStatements.LoadRecipe(recipe);
                    for(int i = 0; i < statements.Count; i++)
                    {
                        if(!Robot.Execute(statements[i], out reply))
                        {
                            return $"Robot refused point {i + 1} of {statements.Count}: {reply}";
                        }
                    }

                    Robot.Execute(RobotStatements.EndInterpreter(), out reply);

                    if(!waitUntil(() => !Robot.State.InTeachMode && Robot.State.RecipeIdOut == recipe.RecipeID, 5000))
                    {
                        return $"Robot did not report recipe {recipe.RecipeID} after loading.";
                    }

                    LogHelper.KeyValue(nameof(RobotHub), "Recipe loaded for lot", $"{recipe.RecipeID} {recipe.RecipeName}");
                    return null;
                }
                catch(Exception ex)
                {
                    LogHelper.Error(nameof(RobotHub), ex);
                    return "Robot recipe load failed: " + ex.Message;
                }
                finally
                {
                    try
                    {
                        plc.Write<bool>(PLCTag.Robot_Teach_Req, false);
                        Robot.NotifyTeachRequest(false);
                    }
                    catch(Exception ex) { LogHelper.Error(nameof(RobotHub), ex); }
                }
            }
        }

        private static bool waitUntil(Func<bool> condition, int timeoutMs)
        {
            DateTime until = DateTime.Now.AddMilliseconds(timeoutMs);
            while(DateTime.Now < until)
            {
                if(condition()) { return true; }
                Thread.Sleep(100);
            }
            return condition();
        }
    }
}
