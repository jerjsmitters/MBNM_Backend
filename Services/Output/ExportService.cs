using Common.Dto.Filters;
using Common.Dto.Output;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Output;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Common;
using Services.Interfaces.Output;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Output
{
    public class ExportService(
        EfDbContext efDbContext,
        ITransactionService transactionService, 
        IGenerateExportFileService generateExportFileService, 
        IExportFileMapper exportMapper) : IExportService
    {
        public async Task<ExportFileDto> CreateExportFileForTransactions(TransactionEntityFilter transactionFilter)
        {
            var filteredResults = await transactionService.GetAllFiltered(transactionFilter);
            var generatedFile = await generateExportFileService.GenerateExportFile(transactionFilter, filteredResults.Transactions);

            var exportFile = await SaveExportFile(generatedFile);
            return exportFile;
        }

        public async Task<List<ExportFileDto>> GetAllExportFiles()
        {
            var files = await efDbContext
                .ExportFiles
                .ToListAsync();

            if (files == null || files.Count == 0)
            {
                return [];
            }

            return files
                .Select(exportMapper.MapEntityToDto)
                .ToList();
        }

        public async Task<ExportFileDto?> GetExportFile(int id)
        {
            var file = await efDbContext.ExportFiles.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
            {
                return null;
            }

            return exportMapper.MapEntityToDto(file);
        }

        private async Task<ExportFileDto> SaveExportFile(FileInfo fileInfo)
        {
            var exportFile = new ExportFile
            {
                FilePath = fileInfo.FullName
            };

            var result = await efDbContext.AddAsync(exportFile);
            await efDbContext.SaveChangesAsync();

            return exportMapper.MapEntityToDto(result.Entity);
        }
    }
}
