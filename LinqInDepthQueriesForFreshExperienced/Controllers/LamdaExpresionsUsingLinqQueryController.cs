using LinqInDepthQueriesForFreshExperienced.Models;
using LinqInDepthQueriesForFreshExperienced.NorthWind_Connect;
using LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LinqInDepthQueriesForFreshExperienced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LamdaExpresionsUsingLinqQueryController : ControllerBase
    {
        NorthwindDbContext _northwind_DBContext;
        NorthwindContext _northwindContext;

        public LamdaExpresionsUsingLinqQueryController(NorthwindDbContext northwind_DBContext, NorthwindContext northwindContext)
        {
            _northwind_DBContext = northwind_DBContext;
            _northwindContext = northwindContext;
        }
        [HttpGet]
        [Route("GetEmployeesData")]
        //2nd of shortcutfor routing 
        //[HttpGet("GetEmployeesData")]
        //Example: Fetching All Records from employee table example
        public async Task<IActionResult> GetEmployeesData()
        {
            //Basic LamdaLinQ synatx is
            //A lambda expression is written using the => lamda operator
            //lamda expressions will reduce the normal linq query synatx.
            //now a days in realtime we are using this lamda expressions with linq.

            //Normal LinqQuery:  var result = from abc in _northwind_DBContext.Employees select abc;
            //Lamda expression Linq queryis below for fetching data from employee
            var result = _northwind_DBContext.Employees.ToList(); // Returns all employees data with all columns.

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

            //Normal LINQ QUERY:var result = from a in _northwind_DBContext.Employees where a.City == "London" select a;//linqquey 
            //SqlQuery:     //select * from  Employees where City='London'
            //LAMDA EXPRESSION USING LINQ query:
            var result = _northwind_DBContext.Employees.Where(a => a.City == "London").ToList();//=>we called lamda opertor
                                                 //(parameters) => expression
            //here expression is a anoymous function.these functions we used in lamda expressions
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);
         // var highEarners = context.Employees.Where(e => e.Salary > 50000).ToList(); 
           // Filters employees with salary > 50,000
        }
        [HttpGet]
        [Route("GetEmployeesDatawith_ReuiredColumnsonlyShowing")]
        public async Task<IActionResult> GetEmployeesDatawith_ReuiredColumnsonlyShowing()
        {//here we are fetchingall the data and showing only one column only.
         //normal linq query:var result = from a in _northwind_DBContext.Employees select new { EmployeeFullName = a.FirstName + a.LastName };
         //SqlQuery Format:select FirstName+LastName as 'EmployeeFullName' from  Employees 
         //Lamda Expression With Linq query:
            var result = _northwind_DBContext.Employees.Select(e => new { e.FirstName, e.LastName,e.Address,e.City}).ToList();
            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GetOrderDataWithNamestatswiths")]
        public async Task<IActionResult> GetDataByNamesStartswiths()
        {//here we are fetchingall employess  data.
         //normal linq query:var result = from s in _northwind_DBContext.Customers where s.ContactName.StartsWith("A") select s;
         //lamda expression linq query like below.
            var result = _northwind_DBContext.Customers.Where(a => a.ContactName.StartsWith("A")).ToList();
                       //SQLQUERY:select * from Customers where ContactName like 'A%'
                       var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(result);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }

        [HttpGet]
        [Route("OrderByusage")]
        public async Task<IActionResult> OrderbyUsage()
        {
            /*
             //Normal linq queries with ascending order and descending order write like this.
               var orderByAscendingResult = from s in lststudentsObj
                                            orderby s.StudentName ascending
                                            select s;//it will Show the  total columns 
                                                     //Select new{StudentId,Age}//you can also select required columns
               var orderByDescendingResult = from s in lststudentsObj
                                             orderby s.StudentName descending
                                             select s;
            */
            //ascending order/descending order  lamda expresion linq query.
            var orderByAscendingResult = _northwind_DBContext.Customers.OrderBy(e=>e.ContactName).ToList();//ascending order
            var orderByDescendingResult = _northwind_DBContext.Customers.OrderByDescending(e => e.ContactName).ToList();//descending order

            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(orderByDescendingResult);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
        [HttpGet]
        [Route("GroupByusageWithOrginalTable")]
        public async Task<IActionResult> GroupByusage()
        {
           
            //Sql Groupby Query:select   CompanyName,Count(*)     from Customers group by CompanyName
            var groupedCompanyNameData = _northwind_DBContext.Customers.GroupBy(s => s.CompanyName)
                                     .Select(g => new { CompanyName = g.Key, CompanyName1 = g.ToList() });
            //It converts your data to jsonformat
            var convertedData = JsonConvert.SerializeObject(groupedCompanyNameData);
            return StatusCode(StatusCodes.Status200OK, convertedData);

        }
    }
    
}
