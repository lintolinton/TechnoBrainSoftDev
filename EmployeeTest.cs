using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace TechnobrainSoftDev
{
    public class Employee
    {

        //List of All employees
        IEnumerable<string> employeeList;


        //List of all manager IDs
        IEnumerable<string> managerIDList;

        //Properties

        public string CSVLocation { get; set; }

        //Constructor

        public Employee(string csvLocation)
        {
            //try reading line, catch IOException

            try
            {
                //Read all the lines from the csv, save to string employee

                string[] employee = File.ReadAllLines(csvLocation);

                employeeList = employee;

                var managersIDQuery =
                    (from _employee in employeeList
                     let elements = _employee.Split(",")
                     select elements[1]).Distinct();

                managerIDList = managersIDQuery.ToList();

                var numberOfCEOQuery =
                from emp in employeeList
                let elements = emp.Split(',')

                where elements[1] == ""

                select elements[1];

                var CEOresults = numberOfCEOQuery.ToList();

                if (CEOresults.Count != 1)
                {
                    throw new ApplicationException("InvalidCEONumberException");
                }
                else
                {
                    //Console.WriteLine("Awesome");
     
                }

            }
            catch(IOException)
            {
                Console.WriteLine("Program Exited. File Access error");
            }
        }

        public long getManagerSalaryBudget(string employeeID)
        {
            string _employeeID = employeeID;
            long salary = 0;

            //Check if employee is manager

            var managerID = managerIDList.ToList();

            if (managerID.IndexOf(_employeeID) < 0)
            {
                throw new ApplicationException("NotAManagerException");
            }
            else
            {
                var managerSalary = 0;
                foreach(string manID in managerID)
                {
                    var managerSalaryQuery =
                        (from manSal in employeeList
                        let elements = manSal.Split(",")
                        where elements[0] == _employeeID
                        select Convert.ToInt32(elements[2]));

                     var managerSalaryList = managerSalaryQuery.ToList();

                    managerSalary = managerSalaryList[0];
   
                }
                salary += managerSalary;

                //Get a list of all managers

             
            }
            //Direct subordinates
            var directSubordinateSalary = 0;

            var directSubordinatesQuery =
                from _employee in employeeList
                let elements = _employee.Split(",")
                where elements[1] == _employeeID
                select Convert.ToInt32(elements[2]);

            directSubordinateSalary = directSubordinatesQuery.ToList().Sum();

            salary += directSubordinateSalary;
            
            //indirect subordinates
            var indirectSubordinateSalary = 0;

            var indirectSubordinatesQuery =
                from _employee in employeeList
                let elements = _employee.Split(",")
                where elements[1] == _employeeID
                select elements[0];

            foreach (string _subordinate in indirectSubordinatesQuery)
            {
                var employeesQuery =
                from _employee in employeeList
                let elements = _employee.Split(",")
                where elements[1] == _subordinate
                select Convert.ToInt32(elements[2]);

                indirectSubordinateSalary += employeesQuery.ToList().Sum();

            }

            salary += indirectSubordinateSalary;

            return salary;
        }
        //main method to test the library
        static void Main(string[] args)
        {
            Employee empl = new Employee(@"C:\eclipse\test.csv");

            var salary = empl.getManagerSalaryBudget("Employee1");

            Console.WriteLine(salary);
        }
    }
}
