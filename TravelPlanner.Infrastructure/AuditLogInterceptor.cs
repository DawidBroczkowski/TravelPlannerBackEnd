using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Infrastructure
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChanges(eventData, result);

            var auditLogs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted || e.State == EntityState.Added))
            {
                var tableName = entry.Metadata.GetTableName();
                var key = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());

                var auditLog = new AuditLog
                {
                    TableName = tableName!,
                    RecordId = key?.CurrentValue as int? ?? 0,
                    OperationType = entry.State.ToString(),
                    ChangedById = (int?)_httpContextAccessor.HttpContext?.Items["UserId"] ?? 0,
                    ChangedAt = DateTime.UtcNow
                };

                if (entry.State == EntityState.Modified)
                {
                    auditLog.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    auditLog.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditLog.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                }
                else if (entry.State == EntityState.Added)
                {
                    auditLog.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }

                auditLogs.Add(auditLog);
            }

            context.Set<AuditLog>().AddRange(auditLogs);

            return base.SavingChanges(eventData, result);
        }
    }

}
