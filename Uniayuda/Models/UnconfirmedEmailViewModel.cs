namespace Uniayuda.Models
{
    public class UnconfirmedEmailViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}