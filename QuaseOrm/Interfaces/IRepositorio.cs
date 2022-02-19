using QuaseOrm.Enums;
using QuaseOrm.Utils;
using System.Collections.Generic;

namespace QuaseOrm.Interfaces
{
    public interface IRepositorio
    {
        List<T> GetAll<T>() where T : new();
        List<T> GetBy<T>(Parameters param) where T : new();
        List<T> GetBy<T>(Parameters param, SqlOperatorComparition sqlOperator) where T : new();
        T GetById<T>(object valor) where T : new();
        T Add<T>(T obj) where T : new();
        T Edit<T>(T obj) where T : new();
        void Delete<T>(T obj) where T : new();
    }
}
