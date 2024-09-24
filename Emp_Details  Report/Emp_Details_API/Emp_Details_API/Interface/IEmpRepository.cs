using Emp_Details_API.Model;

namespace Emp_Details_API.Interface
{
    public interface IEmpRepository
    {
        public List<EmpAPIModel> GetEmp(int Id);
        public int SaveEmp(EmpAPIModel model, string REC_TYPE);
        public int DeleteEmp(EmpAPIModel model, string REC_TYPE);
    }
}
