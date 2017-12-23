namespace P01_BillsPaymentSystem.Data
{
    internal class Configuration
    {
        internal const string ConnectionString = @"Server=.;Database=BillsPaymentSystem;Integrated Security=True";

        // internal, за да може само нашия DbContext да го чете!!!
    }
}
