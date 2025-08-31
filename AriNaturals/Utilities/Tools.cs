using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AriNaturals.Utilities
{
    public static class Tools
    {
        public static string GenerateOrderNumber(long nextId)
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string sequencePart = nextId.ToString("D4");
            return $"AN-ORD-{datePart}-{sequencePart}";
        }
    }
}
