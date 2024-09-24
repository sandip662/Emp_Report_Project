using Emp_Details_API.Interface;
using Emp_Details_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emp_Details_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpAPIController : ControllerBase
    {


        private IEmpRepository iEmpRepository;

        public EmpAPIController(IEmpRepository EmpRepository)
        {
            iEmpRepository = EmpRepository;
        }



        [HttpGet]
        [Route("GetEmp")]
        public List<EmpAPIModel> GetEmp()
        {
            List<EmpAPIModel> Emp = iEmpRepository.GetEmp(0);
            return Emp;

        }


        [HttpPost]
        [Route("SaveEmp")]

        public async Task<int> SaveEmp(EmpAPIModel model)
        {


            int r;
            if (model.Id == 0)
            {
                r = iEmpRepository.SaveEmp(model, "INSERT");

            }
            else
            {
                r = iEmpRepository.SaveEmp(model, "UPDATE");


            }
        
            return r;
        }



        [HttpGet]
        [Route("DeleteEmp/{id}")]
        public async Task<int> DeleteEmp(int id)
        {
            EmpAPIModel model = new EmpAPIModel();
            model.Id = id;
            int r = iEmpRepository.DeleteEmp(model, "DELETE");

            return r;
        }






        [HttpGet]
        [Route("EditEmp/{id}")]
        public async Task<EmpAPIModel> EditEmp(int id)
        {
            List<EmpAPIModel> lstobj = iEmpRepository.GetEmp(id);

            if (lstobj.Count == 1)
            {
                return lstobj.FirstOrDefault();
            }

            return null;
        }



    }
}
