using EcomForge.Application.Abstractions;
using Microsoft.SemanticKernel;

namespace EcomForge.Modules.AI.Plugins;

public sealed class EcommerceAiPlugin : IEcommerceAiPlugin
{
    [KernelFunction("recommend_products")]
    public string RecommendProducts(string category, decimal maxPrice)
    {
        return $"Suggest products in category '{category}' up to {maxPrice:C}. Prioritize practical benefits, stock availability and clear next steps.";
    }

    [KernelFunction("explain_order_status")]
    public string ExplainOrderStatus(string status)
    {
        return status.ToLowerInvariant() switch
        {
            "pending" => "The order was created and is waiting for payment confirmation.",
            "paid" => "Payment was confirmed and the order is being prepared.",
            "shipped" => "The order left the store and is on its way to the customer.",
            "cancelled" => "The order was cancelled and should no longer be fulfilled.",
            _ => "Unknown status. Ask for the order identifier and check the order history."
        };
    }
}
