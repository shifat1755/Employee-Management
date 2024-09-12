namespace EmployeeManagement.Models
{
   public class Response<T>
    {
        public T Data { get; set; }
        public string? Message { get; set; }
        public bool? isSuccessful { get; set; }
    }
}
