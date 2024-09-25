// Services/OrderService.cs
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApi_CloudNautica.Dto;
using WebApi_CloudNautica.Services.Interface;

public class OrderService : IOrderService
{
    private readonly string _connectionString;

    public OrderService(string connectionString)
    {
        _connectionString = connectionString;
    }



    //public async Task<OrderResponse> GetOrderDetailsAsync(OrderRequest request)
    //{
    //    using (var connection = new SqlConnection(_connectionString))
    //    {
    //        try
    //        {

    //            var customerQuery = "SELECT FirstName, LastName, Email, HouseNo, Street, Town, PostCode FROM CUSTOMERS WHERE CUSTOMERID = @CustomerId";
    //            var customer = await connection.QuerySingleOrDefaultAsync(customerQuery, new { CustomerId = request.CustomerId });

    //            if (customer == null || customer.Email != request.User)
    //            {
    //                throw new InvalidOperationException("Invalid email or customer ID.");
    //            }

    //            // Retrieve the most recent order
    //            var orderQuery = @"
    //        SELECT TOP 1 o.ORDERID, o.ORDERDATE, o.DELIVERYEXPECTED, o.CONTAINSGIFT, 
    //              c.HOUSENO, c.STREET, c.TOWN, c.POSTCODE
    //        FROM ORDERS o
    //        JOIN CUSTOMERS c ON o.CUSTOMERID = c.CUSTOMERID
    //        WHERE o.CUSTOMERID = @CustomerId
    //        ORDER BY o.ORDERDATE DESC";

    //            var order = await connection.QuerySingleOrDefaultAsync(orderQuery, new { CustomerId = request.CustomerId });

    //            var response = new OrderResponse
    //            {
    //                Customer = new Customer
    //                {
    //                    FirstName = customer.FirstName,
    //                    LastName = customer.LastName
    //                },
    //                Order = order != null ? new Order
    //                {
    //                    OrderNumber = order.ORDERID,
    //                    OrderDate = order.ORDERDATE.ToString("dd-MMM-yyyy"),
    //                    DeliveryExpected = order.DELIVERYEXPECTED.ToString("dd-MMM-yyyy"),
    //                    DeliveryAddress = $"{order.HOUSENO} {order.STREET}, {order.TOWN}, {order.POSTCODE}",
    //                    OrderItems = await GetOrderItemsAsync(connection, order.ORDERID)
    //                } : null
    //            };

    //            return response;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("Error fetching customer data", ex);
    //        }
    //    }
    //}

    //private async Task<List<OrderItem>> GetOrderItemsAsync(SqlConnection connection, int orderId)
    //{
    //    var itemsQuery = @"
    //        SELECT p.PRODUCTNAME, oi.QUANTITY, oi.PRICE
    //        FROM ORDERITEMS oi
    //        JOIN PRODUCTS p ON oi.PRODUCTID = p.PRODUCTID
    //        WHERE oi.ORDERID = @OrderId";

    //    var items = (await connection.QueryAsync<OrderItem>(itemsQuery, new { OrderId = orderId })).AsList();

    //    // Replace product names with "Gift" if applicable
    //    foreach (var item in items)
    //    {
    //        item.Product = item.Product == "contains a gift" ? "Gift" : item.Product;
    //    }

    //    return items;
    //}

    public async Task<OrderResponse> GetOrderDetailsAsync(OrderRequest request)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Ensure the connection is open before querying
            try
            {
                var customerQuery = "SELECT FirstName, LastName, Email, HouseNo, Street, Town, PostCode FROM CUSTOMERS WHERE CUSTOMERID = @CustomerId";
                var customer = await connection.QuerySingleOrDefaultAsync(customerQuery, new { CustomerId = request.CustomerId });

                // Check if the customer exists and the email matches
                if (customer == null || customer.Email != request.User)
                {
                    return new OrderResponse
                    {
                        Success = false,
                        Message = "Invalid email or customer ID.",
                        Customer = null,
                        Order = null
                    };
                }

                // Retrieve the most recent order
                var orderQuery = @"
            SELECT TOP 1 o.ORDERID, o.ORDERDATE, o.DELIVERYEXPECTED, o.CONTAINSGIFT, 
                  c.HOUSENO, c.STREET, c.TOWN, c.POSTCODE
            FROM ORDERS o
            JOIN CUSTOMERS c ON o.CUSTOMERID = c.CUSTOMERID
            WHERE o.CUSTOMERID = @CustomerId
            ORDER BY o.ORDERDATE DESC";

                var order = await connection.QuerySingleOrDefaultAsync(orderQuery, new { CustomerId = request.CustomerId });

                // Prepare the response
                var response = new OrderResponse
                {
                    Success = true,
                    Customer = new Customer
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName
                    },
                    Order = order != null ? new Order
                    {
                        OrderNumber = order.ORDERID,
                        OrderDate = order.ORDERDATE.ToString("dd-MMM-yyyy"),
                        DeliveryExpected = order.DELIVERYEXPECTED.ToString("dd-MMM-yyyy"),
                        DeliveryAddress = $"{order.HOUSENO} {order.STREET}, {order.TOWN}, {order.POSTCODE}",
                        OrderItems = await GetOrderItemsAsync(connection, order.ORDERID)
                    } : null
                };

                return response;
            }
            catch (SqlException sqlEx)
            {
                // Log SQL specific errors
                throw new Exception("Database error fetching customer data", sqlEx);
            }
            catch (Exception ex)
            {
                // General error handling
                throw new Exception("Unexpected error fetching customer data", ex);
            }
        }
    }

    private async Task<List<OrderItem>> GetOrderItemsAsync(SqlConnection connection, int orderId)
    {
        // Check if orderId is valid
        if (orderId <= 0)
        {
            return new List<OrderItem>(); // Return an empty list if the orderId is invalid
        }

        var itemsQuery = @"
        SELECT p.PRODUCTNAME, oi.QUANTITY, oi.PRICE
        FROM ORDERITEMS oi
        JOIN PRODUCTS p ON oi.PRODUCTID = p.PRODUCTID
        WHERE oi.ORDERID = @OrderId";

        var items = (await connection.QueryAsync<OrderItem>(itemsQuery, new { OrderId = orderId })).AsList();

        foreach (var item in items)
        {
            if (item != null) // Check if item is not null
            {
                // Check if Product property is not null before accessing
                if (!string.IsNullOrEmpty(item.Product))
                {
                    item.Product = item.Product.Contains("contains a gift") ? "Gift" : item.Product;
                }
            }
        }

        return items;
    }



}
