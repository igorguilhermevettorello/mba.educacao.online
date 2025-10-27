namespace MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Payments
{
    public interface IConfigurationManager
    {
        string GetValue(string node);
    }
}