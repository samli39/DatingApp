using DatingApp.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DAL
{
    public class ValuesDAL
    {
        private DatingDbContext _context;

        public ValuesDAL(DatingDbContext context)
        {
            _context = context;
        }

        //get all values from table
        public async Task<List<Values>> GetAllValues()
        {
            return await _context.Values.ToListAsync();
        }

        //get a value based on id
        public async Task<Values> GetValueById(int id)
        {
            return await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
        }

        //insert a data to database
        public async Task<Values> AddValue(Values value)
        {
            await _context.Values.AddAsync(value);
            await _context.SaveChangesAsync();

            return value;
        }

        //inseet multiple data to database at once
        public async Task<List<Values>> AddMultiValues(List<Values> value)
        {
            await _context.Values.AddRangeAsync(value);

            await _context.SaveChangesAsync();
            return value;
        }

        //update a data 
        public async Task<Values> UpdateValue(Values value)
        {
            try
            {
                _context.Update(value);
                await _context.SaveChangesAsync();
                return value;
            }catch(Exception e)
            {
                return value;
            }
        }

        //update multi-data at once
        public  List<Values> UpdateMultiValues(List<Values> value)
        {
            _context.Values.UpdateRange(value);
            return value;
        }

        //delete a data by id
        public async Task<bool> DeleteValue(int id)
        {
            Values value = await _context.Values.FindAsync(id);
            _context.Values.Remove(value);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
