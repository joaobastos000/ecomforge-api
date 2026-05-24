namespace EcomForge.Application.Abstractions;

public interface IEcommerceAiPlugin
{
    string RecommendProducts(string category, decimal maxPrice);
    string ExplainOrderStatus(string status);
}
