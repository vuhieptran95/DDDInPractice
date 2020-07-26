# Modern Vending Machine
This project is to practice DDD by simulating business logic of  modern vending machines with requirements changed frequently.
## From customer perspective
Customers come to these machines to buy stuff:
##### Start transaction
##### `Insert money` (VND) in the machines:
 - Can insert multiple times.
 - Machines accept 5k, 10k, 20k, 50k, 100k, 200k
##### `Choose slots` to buy:
 - Customers can buy multiple items each time
 - Customers can buy many times until ran out of money or ran out of items
##### Machines return items and subtract customer's money
##### Machines ask "Do customers want to buy more stuff or want returned money if have any?"
##### Customers click `return money`, take returned money if have any
 - Customers may or may not take returned money