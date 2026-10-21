using DataAccess.Interfaces;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RestaurantDao : BaseDao, IFRestaurantDao
    {

        public RestaurantDao(string connectionString) : base(connectionString) { }

        public Restaurant? GetResaurantMenuById(int restaurantId)
        {
            using var connection = CreateConnection();

            try
            {
                var query = @"SELECT
                        li.listingId,
                        li.price,
                        li.creationTimeStamp,
                        li.itemInstanceId_fk AS ItemInstanceId,
                        li.sellerAccountId_fk AS SellerAccountId,

                        af.accountId,
                        af.userName,
                        af.surName,
                        af.email,
                        af.phoneNo,

                        ii.itemInstanceId,
                        ii.itemDefinitionId_fk,
                        ii.accountId_fk AS ItemInstanceAccountId,

                        id.itemDefinitionId,
                        id.itemName,
                        id.itemDescription,
                        id.gameId_fk,

                        g.gameId,
                        g.gameTitle

                        FROM Listing li

                        LEFT JOIN Account af
                        ON af.accountId = li.sellerAccountId_fk

                        LEFT JOIN ItemInstance ii
                        ON ii.itemInstanceId = li.itemInstanceId_fk

                        LEFT JOIN ItemDefinition id
                        ON id.itemDefinitionId = ii.itemDefinitionId_fk

                        LEFT JOIN Game g
                        ON g.gameId = id.gameId_fk

                        WHERE li.listingId = @ListingId
                        AND li.statusId_fk = 1";

                Restaurant? restaurant = connection.Query<Listing, Account, ItemInstance, ItemDefinition, Game, Listing>(
                            query,
                            (list, account, itemInstance, itemDef, game) =>
                            {
                                itemDef.Game = game;
                                itemInstance.ItemDefinition = itemDef;
                                list.SellerAccount = account;
                                list.ItemInstance = itemInstance;
                                return list;
                            },
                            new { listingId },
                            splitOn: "accountId,itemInstanceId,itemDefinitionId,gameId"
                        ).SingleOrDefault();

                return restaurant;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while trying to get resturant menu. Error was: '{ex.Message}'", ex);
            }
        }



    }
}
