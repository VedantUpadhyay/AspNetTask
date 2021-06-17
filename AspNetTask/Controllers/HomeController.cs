using AspNetTask.Models;
using AspNetTask.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDapperService dapperService;

        public HomeController(ILogger<HomeController> logger,
            IDapperService dapperService)
        {
            _logger = logger;
            this.dapperService = dapperService;
        }

        [HttpPost]
        public async Task<IActionResult> addAthletePost(int TestId)
        {
            IFormCollection Form = Request.Form;
            int playerId = int.Parse(Form["PlayerId"].ToString());
            string result = Form["result"].ToString();

            bool res = await dapperService.AddAthlete(TestId, playerId, result);

            return RedirectToAction(nameof(TestDetails), new { 
                id = TestId
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAthletePost(int specificTestId,int testId)
        {
            IFormCollection UpdateForm = Request.Form;
            int playerId = int.Parse(UpdateForm["PlayerId"]);
            string result = UpdateForm["result"].ToString();

            bool res = await dapperService.UpdateAthlete(specificTestId, playerId, result, testId);

            return RedirectToAction(nameof(TestDetails),new { 
                id = testId
            });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteTest(int testId)
        {
            bool res = await dapperService.DeleteTest(testId);

            return res ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(TestDetails),new { 
                id = testId
            });
        }

        [HttpPost]
        public async Task<IActionResult> deleteAthleteFromTest(int specificTestId,int testId)
        {
            bool success;
            if (specificTestId != 0)
            {
                success = await dapperService.DeleteAthleteFromTest(specificTestId); 
            }

            return RedirectToAction(nameof(TestDetails),new { 
                id = testId
            });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAthlete(int id,string playerName, string result,int testId)
        {
            bool res = await dapperService.DoesAthleteExist(id);

            if (res == false)
            {
                return RedirectToAction(nameof(TestDetails), new
                {
                    id = testId
                });
            }

            ViewBag.selectedPlayerName = playerName;
            ViewBag.athleteResult = result;
            ViewBag.specificTestId = id;
            ViewBag.testId = testId;

            return View(await dapperService.GetAthletes());
        }

        [HttpGet]
        public async Task<IActionResult> TestDetails([FromRoute]int id,string TestType)
        {
            IEnumerable<SpecificTest> specificTests;

            try
            {

                specificTests = await dapperService.GetTestDetails(id);
            }
            catch (SqlException ex)
            {

                return RedirectToAction(nameof(Index),new { 
                    error = ex.Message
                });
            }

            if (specificTests.Count() == 0)
            {


                specificTests = new List<SpecificTest>();
            }
            else
            {
                
            }

            ViewBag.TestType = await dapperService.GetTestType(id);
            ViewBag.TestId = id;
            //specificTests = specificTests.DefaultIfEmpty();
            return View(specificTests);
        

        //    return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(string error = null)
        {
            IEnumerable<Test> Tests = await dapperService.GetAllTests();

            ViewBag.Error = error;

            return View(Tests);
        }

        [HttpGet]
        public async Task<IActionResult> AddAthlete(int TestId)
        {
            List<Athlete> athletes = await dapperService.GetAthletes();

            ViewBag.TestId = TestId;

            return View(athletes);
        }

        [HttpGet]
        public IActionResult CreateTest(Test test = null)
        {
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestPost(Test test)
        {
            Enum.TryParse(test.TestType, out TestType testType);

            bool result = await dapperService.CreateTest(testType, test.DateAM);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(CreateTest), new { 
                test = test
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
