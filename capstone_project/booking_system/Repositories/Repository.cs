namespace BookingSystem.Repositories;

using BookingSystem.Interfaces;
using BookingSystem.Contexts;
using BookingSystem.Models;

public abstract class Repository<K, T> : IRepository<K, T> where T : class
{
    protected readonly BookingDbContext _bookingdbcontext;
    public Repository(BookingDbContext bookingDbContext)
    {
        _bookingdbcontext = bookingDbContext;
    }
    public async Task<T> Add(T item)
    {
        _bookingdbcontext.Add(item);
        await _bookingdbcontext.SaveChangesAsync();
        return item;
    }

    public abstract Task<T> Get(K key);

    public abstract Task<IEnumerable<T>> GetAll();


    public async Task<T> Delete(K key)
    {
        var item = await Get(key);
        if (item != null)
        {
            _bookingdbcontext.Remove(item);
            await _bookingdbcontext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for deleting");
    }
    public async Task<T> Update(K key, T item)
    {
        var myItem = await Get(key);
        if (myItem != null)
        {
            _bookingdbcontext.Entry(myItem).CurrentValues.SetValues(item);
            await _bookingdbcontext.SaveChangesAsync();
            return item;
        }
        throw new Exception("No such item found for updation");
    }
}