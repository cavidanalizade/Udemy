﻿using App.API.Helper;
using App.BUSINESS.DTOs.Brand;
using App.BUSINESS.DTOs.Category;
using App.BUSINESS.Services.Interfaces;
using App.CORE.Entities;
using App.DAL.Repositories.Implementations;
using App.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.BUSINESS.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Create(CreateCategoryDto createCategoryDto)
        {
            if (!createCategoryDto.LogoImg.CheckContent("image/"))
            {
                throw new Exception( "Duzgun format daxil edin");
            }
            Category category = new Category()
            {
                Name = createCategoryDto.Name,
                LogoUrl = createCategoryDto.LogoImg.UploadFile(folderName: "C:\\Users\\II novbe\\Desktop\\Api_project\\App.BUSINESS\\Upload\\"),
                CreatedAt=DateTime.Now,

                

                
            };
            await _repo.Create(category);

            _repo.Save();
        }

        public async Task Delete(int id)
        {
             _repo.delete(id);
            _repo.Save();
        }

        public async Task<ICollection<Category>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
             return await categories.ToListAsync(); 
        }

        public async Task<Category> GetById(int id)
        {
            if(id<=0) throw new Exception("Bad Request");
            return await _repo.GetById(id);
        }

        public async Task<ICollection<Category>> RecycleBin()
        {
            var categories = await _repo.RecycleBin();
            return await categories.ToListAsync();
        }

        public async Task Update(UpdateCategoryDto updateCategoryDto)
        {

            if (updateCategoryDto == null) throw new Exception("Bad Request");

            var existingCategory = await _repo.GetById(updateCategoryDto.Id);
            existingCategory.Name = updateCategoryDto.Name;
            if(updateCategoryDto.LogoImg != null)
            {
                existingCategory.LogoUrl.RemoveFile("C:\\Users\\II novbe\\Desktop\\Api_project\\App.BUSINESS\\Upload\\");
                existingCategory.LogoUrl = updateCategoryDto.LogoImg.UploadFile(folderName: "C:\\Users\\II novbe\\Desktop\\Api_project\\App.BUSINESS\\Upload\\");
                existingCategory.UpdatedAt=DateTime.Now;
            }
            _repo.Update(existingCategory);
            _repo.Save();
        }
        public async Task Restore()
        {
            _repo.Restore();
            _repo.Save();
        }

        public async Task deleteAll()
        {
            _repo.deleteAll();
            _repo.Save();
        }
    }
}
