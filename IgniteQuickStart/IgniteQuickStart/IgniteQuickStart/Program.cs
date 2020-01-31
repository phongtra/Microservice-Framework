 using System;
 using System.Collections.Generic;
 using Apache.Ignite.Core;
 using Apache.Ignite.Core.Client;
 using Apache.Ignite.Core.Deployment;
 using IgniteQuickStart.DTOs;

 namespace IgniteQuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ignition.ClientMode = true;
            IgniteConfiguration cfg = new IgniteConfiguration(); //.FromXml();
//            cfg.SpringConfigUrl = "D:\\repos\\apache-ignite-2.7.5-bin\\config\\customConfig.client.xml";
            cfg.SpringConfigUrl         = "D:\\repos\\apache-ignite-2.7.5-bin\\config\\customConfig.Default.xml";
            cfg.ClientMode              = true;
            cfg.PeerAssemblyLoadingMode = PeerAssemblyLoadingMode.CurrentAppDomain;
            cfg.JvmOptions              = new List<string> {"-XX:+UseG1GC", "-XX:+DisableExplicitGC"};

            try
            {
                using (var ignite = Ignition.Start(cfg))
                {
                    Console.WriteLine();
                    Console.WriteLine(">>> Task execution example started.");


//                // Generate employees to calculate average salary for.
                    ICollection<Employee> employees = Employees();

                    Console.WriteLine();
                    Console.WriteLine(">>> Calculating average salary for employees:");

//                foreach (Employee employee in employees)
//                    Console.WriteLine(">>>     " + employee);

                    var computes = ignite.GetCompute();
//                var task = new AverageSalaryTask();
//                computes.Broadcast(task);
                    // Execute task and get average salary.
                    var avgSalary = computes
                        .Execute(new AverageSalaryTask(), employees);

                    Console.WriteLine("DONE");
                    Console.WriteLine(">>> Average salary for all employees: " + avgSalary);
                    Console.WriteLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

        /// <summary>
        /// Generates collection of employees for example.
        /// </summary>
        /// <returns>Collection of employees.</returns>
        private static ICollection<Employee> Employees()
        {
            return new[]
            {
                new Employee(
                    "James Wilson",
                    12500,
                    new Address("1096 Eddy Street, San Francisco, CA", 94109),
                    new List<string> {"Human Resources", "Customer Service"}
                    ),
                new Employee(
                    "Daniel Adams",
                    11000,
                    new Address("184 Fidler Drive, San Antonio, TX", 78205),
                    new List<string> {"Development", "QA"}
                    ),
                new Employee(
                    "Cristian Moss",
                    12500,
                    new Address("667 Jerry Dove Drive, Florence, SC", 29501),
                    new List<string> {"Logistics"}
                    ),
                new Employee(
                    "Allison Mathis",
                    25300,
                    new Address("2702 Freedom Lane, Hornitos, CA", 95325),
                    new List<string> {"Development"}
                    ),
                new Employee(
                    "Breana Robbin",
                    6500,
                    new Address("3960 Sundown Lane, Austin, TX", 78758),
                    new List<string> {"Sales"}
                    ),
                new Employee(
                    "Philip Horsley",
                    19800,
                    new Address("2803 Elsie Drive, Sioux Falls, SD", 57104),
                    new List<string> {"Sales"}
                    ),
                new Employee(
                    "Brian Peters",
                    10600,
                    new Address("1407 Pearlman Avenue, Boston, MA", 02110),
                    new List<string> {"Development", "QA"}
                    ),
                new Employee(
                    "Jack Yang",
                    12900,
                    new Address("4425 Parrish Avenue Smithsons Valley, TX", 78130),
                    new List<string> {"Sales"}
                    )
            };
        }
    }
}
