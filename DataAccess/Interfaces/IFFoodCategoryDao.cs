using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces;

public interface IFFoodCategoryDao
{
    int CreateCategory(int menuId, string name, List<int> foodItemIds);

    bool UpdateCategory(
        int categoryId,
        int menuId,
        string name,
        List<int> foodItemIds);

    bool DeleteCategory(int categoryId);
}

