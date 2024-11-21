using LinqInDepthQueriesForFreshExperienced.Models;
using LinqInDepthQueriesForFreshExperienced.NorthWind_Connect;
using LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net;
namespace LinqInDepthQueriesForFreshExperienced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalLinqQueriesController : ControllerBase
    {
        NorthwindDbContext _northwind_DBContext;
        NorthwindContext _northwindContext;

        public NormalLinqQueriesController(NorthwindDbContext northwind_DBContext, NorthwindContext northwindContext)
        {
            _northwind_DBContext = northwind_DBContext;
            _northwindContext = northwindContext;
        }

        [HttpGet]
        [Route("GetEmployeesData")]
        //2nd of shortcutfor routing 
        //[HttpGet("GetEmployeesData")]
        public async Task<IActionResult> GetEmployeesData()
        {
            //Basic LinQ synatx
            //var result=from variablename in datasource  (optional clause ) select variablename
            //here (optional clause ) means where clause,order by clause,Group by Clause...

            //here we are fetchingall employess  data.
            //var means implcit typed local variable
            //synatx://var result=from localvariablename in datasource  (optional clause ) select localvariablename
            //Note:abc means its a localvariablename
            var result = from abc in _northwind_DBContext.Employees select abc;
            //sqlqueryconverted by compiler:select * from employees
            //the below written for json serialization refrence looping purpose written .net 8.0 to fix this refrence looping we are using this one.lower versions you will not get.
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result, jsonSettings);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployeesDatawith_CityWise")]
        public async Task<IActionResult> GetAllEmployeesDataCityWise()
        {
            //synatx://var result=from localvariablename in datasource  (optional clause ) select localvariablename   
            //it will return employee data with it department along with all the columns data
            var result = from a in _northwind_DBContext.Employees where a.City == "London" select a;//linqquey 
                                                                                                    //SqlQuery:     //select * from  Employees where City='London'


            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployeesDatawith_ReuiredColumnsonlyShowing")]
        public async Task<IActionResult> GetEmployeesDatawith_ReuiredColumnsonlyShowing()
        {//here we are fetchingall the data and showing only one column only.
            var result = from a in _northwind_DBContext.Employees select new { EmployeeFullName = a.FirstName + a.LastName };
            //SqlQuery Format:select FirstName+LastName as 'EmployeeFullName' from  Employees 

            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetOrderDataWithNamestatswiths")]
        public async Task<IActionResult> GetDataByNamesStartswiths()
        {//here we are fetchingall employess  data.
            var result = from s in _northwind_DBContext.Customers where s.ContactName.StartsWith("A") select s;
            //SQLQUERY:select * from Customers where ContactName like 'A%'
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetEmployees&DeptDataByUsingJoins")]
        public async Task<IActionResult> GetDataByUsingJoins()
        {
            //here we are fetching employess&DepartMenent with data by using joins and orderby descending with required columns.
            //sqlquery:select e.FirstName, e.LastName, e.City, d.DeptName from employee e join Departments d on d.Id==e.EmpId order by e.City desc
            var result = from e in _northwindContext.Employees join d in _northwindContext.Departments on e.EmpId equals d.Id orderby e.City descending select new { e.FirstName, e.LastName, e.City, d.DeptName };
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("take(number) Usage")]
        public async Task<IActionResult> TakeUsage()
        {
            //if you want to get the only first 5 records in atable use this take(number) method.
            //select top 5 * from customers
            var result = (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(5);
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("Skip(number) Usage")]
        public async Task<IActionResult> SkipUsage()
        {
            //if you want to get the only first 5 records in a table use this take(number) method.
            //after using the take() method you can use skip() method .
            //skip will skip or ignore the given count of records after taking the records.

            /*
             * WITH CTE AS (
            SELECT *, ROW_NUMBER() OVER (ORDER BY CustomerID) AS rn
            FROM customers
            )
            SELECT * FROM CTE WHERE rn = 1;
            */
            //BASED ON YOU REQUIREMNT YOU CAN FEATCH THE REUIRED RECORDS BY USING ABOVE QUERY.

            var result = (from lstcustmer in _northwindContext.Customers select lstcustmer).Take(5).Skip(4);
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("AgeWithFilter")]
        public async Task<IActionResult> AgeWithFilter()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()//CREATE LIST OBJECT FIRST
{//List with multiple objects declaring and assigning the data like this way
   new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,//this is one object
   new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 21 } ,//this is one object
   new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,//this is one object
   new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,//this is one object
   new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }//this is one object
};
            var filteredResult = from s in lststudentsObj
                                 where s.Age > 15 && s.Age <= 20
                                 select new { FullName = s.StudentName };//giving the alisaname
                                                                         //It converts your data to jsonstringformat
            var convertedData = JsonConvert.SerializeObject(filteredResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("OrderByusage")]
        public async Task<IActionResult> OrderbyUsage()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()
{
   new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,
   new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 21 } ,
   new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,
   new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,
   new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }
};
            var orderByAscendingResult = from s in lststudentsObj
                                         orderby s.StudentName ascending
                                         select s;//it will Show the  total columns 
                           //Select new{StudentId,Age}//you can also select required columns
            var orderByDescendingResult = from s in lststudentsObj
                                          orderby s.StudentName descending
                                          select s;
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(orderByDescendingResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusageWith dummydata")]
        public async Task<IActionResult> GroupByusage()
        {
            //example with dummydata
            List<StudentData> lststudentsObj = new List<StudentData>()
{
   new StudentData() { StudentID = 1, StudentName = "John", Age = 13} ,
   new StudentData() { StudentID = 2, StudentName = "Moin",  Age = 13 } ,
   new StudentData() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,
   new StudentData() { StudentID = 4, StudentName = "Ram" , Age = 20} ,
   new StudentData() { StudentID = 5, StudentName = "Ron" , Age = 15 }
};
            //Sql Groupby Query:select   CompanyName,Count(*)     from Customers group by CompanyName
            var groupedStudents = lststudentsObj.GroupBy(s => s.Age)
                                     .Select(g => new { Age = g.Key, Students = g.ToList() });
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedStudents);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusageWithCount")]
        public async Task<IActionResult> GroupByusageWithCount()
        {
            //example with dummydata
            // Define a list of fruits
            List<string> fruits = new List<string>
{
"apple", "banana", "orange", "apple", "grape", "banana", "apple"
};

            // Group the fruits using Query syntax(RealTime Usethis one)
            var groupedFruits = fruits.GroupBy(f => f)
                          .Select(g => new { Fruit = g.Key, Count = g.Count() });

            // Group the fruits using method syntax
            var fruitsGrouped1 = fruits.GroupBy(fruit => fruit);

            // Print the grouped fruits
            foreach (var group in fruitsGrouped1)
            {
                Console.WriteLine($"Fruit: {group.Key}, Count: {group.Count()}");
            }
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedFruits);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
    }
}
