#Manage Inventory

##Stories
As a propogator, I want to know what seeds I have available so I can sow them.
As a propogator, I want to know what plants I have that are ready to put into the ground.
As a propogator, I want to know if I have any space for my seeds that are ready to germinate.
As a propogator, I want to know if the seeds I have are expiring so I can use them before they expire.
As a propogator, I want to know where a seed packet came from so that if I got a bad rate of germination, I can avoid that supplier, or traversely a good rate so I can use that supplier.
As a propogator, I want to let other propogators know if I have any inventory available for trade.
As a propogator, I want to check my wishlist when I'm shopping so I can acquire items I want.

##Data design
- Item
  - Name
  - Type
    - Specimen - an instance of a plant
      - Plant - profile of a plant
    - Supply
    - Capacity
      - Pots, trays, space?
 - Status
 - DateAcquired
 - Origin
   - Source
   - An origin is a higher level of specificty than a Source
 - Source
   - Source - 
     - Type:
       - Nursery
       - Store
       - Location
       - Person
       - Event
     - Contact
     - URL
 - IsWishlist
 - Is a seed considered a specimen?
- Examples:
  - Root hormone
  - Packet of Joe Pye Weed seeds
  - Collected seeds
  - Bag of mushroom compost
  - Max-sea
