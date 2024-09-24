using AspNetCore.Reporting;

using Emp_Details_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace Emp_Details_UI.Controllers
{
    public class EmpUIController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string BaseUrlEmp;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmpUIController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _configuration = configuration;
            BaseUrlEmp = configuration["BaseUrlEmp"];
            this._webHostEnvironment = webHostEnvironment;
        }

        // This is a HttpGet method
        public async Task<IActionResult> Index()
        {
            try
            {
                string urlParameters = "GetEmp";
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrlEmp + urlParameters);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        List<EmpUIModel> lst = JsonConvert.DeserializeObject<List<EmpUIModel>>(responseContent);

                        ViewBag.EMP = lst;

                        return View();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public async Task<IActionResult> Print()
        {
            try
            {
                // Fetch data from the API
                string urlParameters = "GetEmp";
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrlEmp + urlParameters);

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize response content to list
                        string responseContent = await response.Content.ReadAsStringAsync();
                        List<EmpUIModel> lst = JsonConvert.DeserializeObject<List<EmpUIModel>>(responseContent);

                        int extension = 1;
                        var path = $"{this._webHostEnvironment.WebRootPath}\\Report\\Report1.rdlc";
                        Dictionary<string, string> parameters = new Dictionary<string, string>();


                        LocalReport localreport1 = new LocalReport(path);
                        localreport1.AddDataSource("Emplist", lst);

                        int ext = (int)(DateTime.Now.Ticks >> 10);

                        var result = localreport1.Execute(RenderType.Pdf, extension, null);

                        return File(result.MainStream, "application/pdf");
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Error fetching data from the API.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }



    



        public async Task<JsonResult> Empdetails(EmpUIModel model)
        {

            string urlparametrs = "GetEmp";
            var httpclient = new HttpClient();


            var dataToSerialize = new { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, Salary = model.Salary };

            var content = new StringContent(JsonConvert.SerializeObject(dataToSerialize), Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(BaseUrlEmp + urlparametrs, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();



                return Json(responseData);
            }
            else
            {
                return Json("Error");
            }

        }






        // This is a HttpPost method
        [HttpPost]
        public async Task<IActionResult> SaveEmp(EmpUIModel model)
        {
            try
            {
                string urlParameters = "SaveEmp";
                string data = JsonConvert.SerializeObject(model);
                using (var httpClient = new HttpClient())
                {
                    StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(BaseUrlEmp + urlParameters, Content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        int r = JsonConvert.DeserializeObject<int>(responseData);

                        if (r > 0)
                        {
                            TempData["MSG"] = "success";
                        }
                        else if (r == -1)
                        {
                            TempData["MSG"] = "exist";
                        }
                        else
                        {
                            TempData["MSG"] = "Fail";
                        }

                        return Redirect("/EmpUI/Index");
                    }
                    else
                    {
                        return Json("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // This is a HttpPost method
        [HttpPost]
        public async Task<IActionResult> DeleteEmp(int id)
        {
            try
            {
                string urlParameters = "DeleteEmp";

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrlEmp + urlParameters + "/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        int r = JsonConvert.DeserializeObject<int>(responseData);
                        return Json(r);
                    }
                    else
                    {
                        return Json("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditEmp(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string urlParameters = "EditEmp";
                    HttpResponseMessage response = await httpClient.GetAsync(BaseUrlEmp + urlParameters + "/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        EmpUIModel lst = JsonConvert.DeserializeObject<EmpUIModel>(data);
                        return Json(lst);
                    }
                    else
                    {
                        return Json("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
