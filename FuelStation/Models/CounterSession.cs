namespace OrganizationsWaterSupply.Models
{
    public class CounterSession
    {

        public int RegistrationNumber { get; set; }
        public int ModelId { get; set; }
        public CounterSession(int RegistrationNumber, int ModelId)
        {
            this.RegistrationNumber = RegistrationNumber;
            this.ModelId = ModelId;
        }
    }

}
