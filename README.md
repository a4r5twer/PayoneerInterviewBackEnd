# PayoneerInterviewBackEnd

## Set Up Instruction 
1. intall dotnet ef tools 
Run the following command to install 
```
dotnet tool install --global dotnet-ef --version 8.0.0
```

2. Set up the local database 
Change the connection string in config file properly 
```
/src/appsettings.Development.json
```
Run the following command under /src
```
dotnet ef database update
```
## How to run the unit test and intergration test 
1. See the .http file for the sample request for intergration test 
2. Goes to the following folder /sec 
Run the following command 

```
dotnet test 
```

## Any assumption or design decision 

### All the application is build on the following assumption , there is no cocurrency here . 

#### Order Uniqueness:
    •	Each order is uniquely identified by a Guid (OrderId).
    •	Idempotence is enforced: submitting the same OrderId twice does not create duplicate orders.
#### Product Existence:
    •	All ProductIds in an order must exist in the product stock table.
    •	Orders with non-existent products are rejected.
#### Inventory Management:
    •	The system checks inventory before confirming an order.
    •	Orders exceeding available inventory are rejected.
#### Atomicity:
    •	An order is only created if all items are valid and inventory is sufficient for every item.
#### Validation:
    •	Requests are validated for required fields (OrderId, CustomerName, Items, etc.) before processing.
#### Timestamps:
    •	CreatedAt is used for order creation time, but not for concurrency control.

