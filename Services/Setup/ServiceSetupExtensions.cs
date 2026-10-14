using DataAccess.EF.DbContextImpl;
using DataAccess.EF.Setup;
using Microsoft.Extensions.DependencyInjection;
using Services.Common;
using Services.Criteria;
using Services.Input;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Output;
using Services.Interfaces.Transactions;
using Services.Output;
using Services.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Setup
{
    public static class ServiceSetupExtensions
    {
        public static IServiceCollection SetupServices(this IServiceCollection services, string connString)
        {
            //input services
            services.AddScoped<IInputService, InputService>();
            services.AddScoped<IParsingService, SantanderCsvParsingService>();

            //entity services
            services.AddScoped<IChangeRequestService, ChangeRequestService>();
            services.AddScoped<IGenerateChangeRequestService, GenerateChangeRequestService>();
            services.AddScoped<ITransactionCategoryService, TransactionCategoryService>();
            services.AddScoped<ITransactionEvidenceService, TransactionEvidenceService>();
            services.AddScoped<ITransactionPropertyService, TransactionPropertyService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IGenerateExportFileService, GenerateExportFileService>();
            services.AddScoped<IExcelToCsvService, ExcelToCsvService>();

            //misc
            services.AddScoped<IMatchingCriteriaService, MatchingCriteriaService>();

            //mappers
            services.AddScoped<IChangeRequestMapper, ChangeRequestMapper>();
            services.AddScoped<IMatchingCriteriaMapper, MatchingCriteriaMapper>();
            services.AddScoped<ITransactionCategoryMapper, TransactionCategoryMapper>();
            services.AddScoped<ITransactionPropertyMapper, TransactionPropertyMapper>();
            services.AddScoped<ITransactionMapper, TransactionMapper>();
            services.AddScoped<ITransactionEvidenceMapper, TransactionEvidenceMapper>();
            services.AddScoped<IExportFileMapper, ExportFileMapper>();

            services.InitialiseEF(connString);

            return services;
        }
    }
}
