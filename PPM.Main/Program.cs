using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Data.SqlClient;
using PPM.Console.UI;
using PPM.DAL;

namespace PPM.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
             
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine();
            System.Console.WriteLine("*****************************************************");
            System.Console.WriteLine("     WELCOME TO PROLIFICS PROJECT MANAGER (PPM)      ");
            System.Console.WriteLine("*****************************************************");

            Menu menuObj = new Menu();

            int choice;
            int moduleChoice;

            while (true)
            {
                moduleChoice = menuObj.ModuleChoices();

                switch (moduleChoice)
                {
                    //Project Module
                    case 1:
                        // chooses cases in project module operation
                        choice = menuObj.ProjectModuleOperations();
                        switch (choice)
                        {
                            case 1:
                                menuObj.AddProject();
                                break;

                            case 2:
                                menuObj.ListAllProjects();
                                break;

                            case 3:
                                menuObj.ListProjectById();
                                break;

                            case 4:
                                menuObj.DeleteProject();
                                break;

                            case 5:
                                menuObj.ViewProjects();
                                break;

                            case 6:
                                menuObj.UpdateProjects();
                                break;

                            case 7:
                                menuObj.AssignEmployeeToProject();
                                break;

                            case 8:
                                menuObj.RemoveEmployeeFromProject();
                                break;

                            case 9:
                                menuObj.ViewProjectDetails();
                                break;

                            case 10:
                                menuObj.MenuQuit();
                                break;

                            default:
                                System.Console.WriteLine(
                                    "Invalid operation choice. Please try again."
                                );
                                break;
                        }
                        break;

                    //Employee Module
                    case 2:
                        //choose cases in employee module operation
                        choice = menuObj.EmployeeModuleOperations();

                        switch (choice)
                        {
                            case 1:
                                menuObj.AddEmployee();
                                break;

                            case 2:
                                menuObj.ListAllEmployees();
                                break;

                            case 3:
                                menuObj.ListEmployeeById();
                                break;

                            case 4:
                                menuObj.DeleteEmployee();
                                break;

                            case 5:
                                menuObj.ViewEmployees();
                                break;

                            case 6:
                                menuObj.UpdateEmployees();
                                break;

                            case 7:
                                menuObj.MenuQuit();
                                break;

                            default:
                                System.Console.WriteLine(
                                    "Invalid operation choice. Please try again."
                                );
                                break;
                        }
                        break;

                    //Role Module
                    case 3:
                        //choose cases in role module operation
                        choice = menuObj.RoleModuleOperations();

                        switch (choice)
                        {
                            case 1:
                                menuObj.AddRole();
                                break;

                            case 2:
                                menuObj.ListAllRoles();
                                break;

                            case 3:
                                menuObj.ListRoleById();
                                break;

                            case 4:
                                menuObj.DeleteRole();
                                break;

                            case 5:
                                menuObj.ViewRoles();
                                break;

                            case 6:
                                menuObj.UpdateRoles();
                                break;

                            case 7:
                                menuObj.MenuQuit();
                                break;

                            default:
                                System.Console.WriteLine(
                                    "Invalid operation choice. Please try again."
                                );
                                break;
                        }
                        break;

                    //Save the data
                    case 4:
                        menuObj.MenuSave();
                        break;

                    case 5:
                        menuObj.LoadAppState();
                        break;

                    case 6:
                        menuObj.Quit();
                        return;

                    default:
                        System.Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        
    }
}
