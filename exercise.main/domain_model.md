# Domain Modelling

## Core Tasks

1. Add items to basket
2. Remove items from basket
3. Notify when basket is full
4. Change basket capacity
5. Handle removing items that don’t exist in basket
6. View total cost of basket
7. View price of item before adding to basket
8. Add extra fillings to bagels
9. View price of fillings
10. Only order items that are in stock


| #  | Classes/Interfaces | Methods/Properties            | Scenario                                      | Outputs                      |
| -- | ------------------ | ----------------------------- | --------------------------------------------- | ---------------------------- |
| 1  | Basket             | AddItem()					  | Add a bagel or other product to basket        | Item added                   |
| 2  | Basket             | RemoveItem()				  | Remove product from basket                    | Item removed                 |
| 3  | Basket             | AddItem() - capacity check    | Notify when basket is full                    | BasketFullException          |
| 4  | Basket	          | SetCapacity()				  | Change the basket capacity                    | Updated capacity             |
| 5  | IBasket            | RemoveItem()				  | Attempt to remove item that doesn’t exist     | ItemNotInBasketException     |
| 6  | Basket             | Totalprice()                  | Get total price of all items in basket        | Decimal sum                  |
| 7  | ICatalog           | GetProduct().Price			  | View price of item before adding it to basket | Decimal price                |
| 8  | Basket             | AddItem()					  | Choose extra fillings for bagel               | Filling added	             |
| 9  | ICatalog           | GetProduct().Price			  | View price of each filling                    | Decimal price                |
| 10 | ICatalog           | ProductExists                 | Only allow adding items that exist in stock   | OK / NotInInventoryException |


